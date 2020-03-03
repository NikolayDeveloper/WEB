using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Info.RequestInfos;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.BusinessLogic.Dictionaries.DicTypeTrademarks;
using Iserv.Niis.BusinessLogic.RequestCustomers;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Domain.OldNiisEntities;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Services.Dapper;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Utils.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request;
using Microsoft.Extensions.Configuration;
using GetDicProtectionDocTypeByIdQuery = Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes.GetDicProtectionDocTypeByIdQuery;

namespace Iserv.Niis.Services.Implementations
{
    public class ImportRequestHelper : BaseImportHelper, IImportRequestHelper
    {
        #region Constructor

        private readonly IGenerateHash _generateHash;
        private readonly IFileStorage _fileStorage;
        private readonly IImportPaymentsHelper _importPaymentsHelper;
        private readonly IImportDocumentsHelper _importDocumentsHelper;
        private readonly IImportContractsHelper _importContractsHelper;


        public const int DeveloperUserId = 1;

        private List<Request> SavedRequests { get; }

        public ImportRequestHelper(
            IConfiguration configuration,
            IImportContractsHelper importContractsHelper,
            IImportPaymentsHelper importPaymentsHelper,
            IImportDocumentsHelper importDocumentsHelper,
            DictionaryHelper dictionaryHelper,
            IGenerateHash generateHash, 
            IFileStorage fileStorage) : base(configuration, dictionaryHelper)
        {
            _importPaymentsHelper = importPaymentsHelper;
            _importDocumentsHelper = importDocumentsHelper;
            _importContractsHelper = importContractsHelper;
            _generateHash = generateHash;
            _fileStorage = fileStorage;

            SavedRequests = new List<Request>();
        }

        #endregion

        /// <summary>
        /// Импорт всех заявок за датау
        /// </summary>
        /// <param name="date">Дата сбора заявок</param>
        /// <returns>Список id импортированных заявок</returns>
        public async Task<IList<int>> ImportRequestByDate(DateTime date)
        {
            var ids = new List<int>();

            var requestSqlQuery = string.Format(ImportSqlQueryHelper.RequestsByDateSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), date.ToString("dd-MM-yyyy"));
            var oldRequests = await SqlDapperConnection.QueryAsync<string>(requestSqlQuery, TargetConnectionString);
            var oldRequestNumbers = oldRequests.ToList();

            if (!oldRequestNumbers.Any())
                return null;

            foreach (var oldRequest in oldRequestNumbers)
            {
                var id = await ImportFromDb(oldRequest, true);
                if (id == null) continue;
                ids.Add(id.Value);
            }

            return ids;
        }

        /// <summary>
        /// Импорт заявки по номеру
        /// </summary>
        /// <param name="number">Входящий Номер или номер заявки</param>
        /// <param name="returnNullIfExist">Вернуть NULL если уже создан</param>
        /// <returns>Id импортированной заявки</returns>
        public async Task<int?> ImportFromDb(string number, bool returnNullIfExist = false)
        {
            var validResult = Valid(number);
            if (validResult != null && validResult != 0)
                return !returnNullIfExist ? validResult : null;

            //Request
            var newRequest = await FillRequest(number);
            var newRequestId = newRequest.Id;
            number = newRequest.IncomingNumber;

            //Workflow
            await FillWorkflows(newRequestId, number);
            //Info
            await FillInfo(newRequestId, number);
            //Color
            await FillColors(newRequestId, number);
            //Customers
            await FillCustomers(newRequestId, number);
            //Icfems
            await FillIcfems(newRequestId, number);
            //Icgs
            await FillIcgs(newRequestId, number);
            //Icis
            await FillIcis(newRequestId, number);
            //Ipc
            await FillIpc(newRequestId, number);
            //EarlyReg
            await FillEarlyReg(newRequestId, number);
            //Attachments
            await FillAttachments(newRequestId);

            try
            {
                //BeneficiaryType
                var beneficiarySqlQuery = string.Format(ImportSqlQueryHelper.BeneficiarySqlQuery, number);
                await SqlDapperConnection.ExecuteAsync(beneficiarySqlQuery, SourceConnectionString);
                //CurrentWorkflow
                var currentWorkflowSqlQuery = string.Format(ImportSqlQueryHelper.CurrentWorkflowSqlQuery, newRequestId);
                await SqlDapperConnection.ExecuteAsync(currentWorkflowSqlQuery, SourceConnectionString);
                //MainAttachment
                var setMainAttachmentSqlQuery = string.Format(ImportSqlQueryHelper.SetRequestMainAttachmentSqlQuery, newRequestId);
                await SqlDapperConnection.ExecuteAsync(setMainAttachmentSqlQuery, SourceConnectionString);
            }
            catch (Exception)
            {
                // ignored
            }

            //Payments
            await _importPaymentsHelper.ImportFromDb(number, newRequestId);

            //Documents
            await _importDocumentsHelper.ImportFromDb(number, newRequestId);

            //Contract
            await _importContractsHelper.ImportFromDb(number, newRequestId);

            return newRequestId;
        }

        #region Request

        private async Task<Request> FillRequest(string number)
        {
            try
            {
                var requestSqlQuery = string.Format(ImportSqlQueryHelper.RequestSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
                var oldRequest = await SqlDapperConnection.QueryAsync<DdDocument>(requestSqlQuery, TargetConnectionString);
                var ddDocuments = oldRequest.ToList();
                if (!ddDocuments.Any()) throw new NotSupportedException("Заявка не найдена, проверьте наличие Входящего и Регистрационного номера заявки");
                var ddDocument = ddDocuments.First();
                var request = await CreateRequest(ddDocument);
                var newRequestId = await Executor.GetCommand<CreateRequestCommand>().Process(d => d.ExecuteAsync(request));

                request.Id = newRequestId;

                request.DateCreate = new DateTimeOffset(ddDocument.DateCreate.GetValueOrDefault(DateTime.Now));

                await Executor.GetCommand<UpdateRequestCommand>().Process(d => d.ExecuteAsync(request));

                SavedRequests.Add(request);

                return request;
            }
            catch (Exception e)
            {
                throw new NotSupportedException($"Ошибка при импорте заявки №{number}, обратитесь к Администратору", e);
            }
        }

        private async Task<Request> CreateRequest(DdDocument ddDocument)
        {
            var customerId = GetObjectId<DicCustomer>(ddDocument.CustomerId);
            if ((customerId == null || customerId == 0) && ddDocument.CustomerId.HasValue)
                customerId = await GetCustomer(ddDocument.CustomerId.Value);

            var result = new Request
            {
                AddresseeId = customerId,
                Barcode = ddDocument.Id,
                ConventionTypeId = GetObjectId<DicConventionType>(ddDocument.TypeiiId),
                CopyCount = ddDocument.CopyCount,
                DateCreate = new DateTimeOffset(ddDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                DateUpdate = new DateTimeOffset(ddDocument.Stamp.GetValueOrDefault(DateTime.Now)),
                DepartmentId = GetObjectId<DicDepartment>(ddDocument.DepartmentId),
                DisclaimerKz = ddDocument.DisclamKz,
                DisclaimerRu = ddDocument.DisclamRu,
                DivisionId = GetObjectId<DicDivision>(ddDocument.DivisionId),
                ExternalId = ddDocument.Id,
                FlDivisionId = GetObjectId<DicDivision>(ddDocument.FlDivisionId),
                Image = ddDocument.Image,
                IncomingNumber = ddDocument.InoutNum,
                IncomingNumberFilial = ddDocument.InnumAdd,
                IsComplete = GenerateHelper.StringToBool(ddDocument.IsComplete),
                IsDeleted = false,
                IsImageFromName = false,
                IsRead = true,
                NameEn = ddDocument.DescriptionMlEn,
                NameKz = ddDocument.DescriptionMlKz,
                NameRu = ddDocument.DescriptionMlRu,
                NumberBulletin = ddDocument.Nby,
                OutgoingNumber = ddDocument.Outnum,
                PageCount = ddDocument.PageCount,
                PreviewImage = ddDocument.SysImagesmall,
                ProtectionDocTypeId = GetProtectionDocTypeByCode(MapOldRequestTypeToProtectionDocType.Get(ddDocument.DoctypeId)),
                PublicDate = GetNullableDate(ddDocument.PublicationDate),
                ReceiveTypeId = GetObjectId<DicReceiveType>(ddDocument.SendType),
                Referat = ddDocument.Ref57,
                RequestDate = GetNullableDate(ddDocument.DocumDate),
                RequestNum = ddDocument.DocumNum,
                RequestTypeId = InitializeSubType(ddDocument),
                SpeciesTradeMarkId = GetSpeciesTradeMarkId(ddDocument),
                SelectionFamily = ddDocument.SelectionFamily,
                StatusId = GetObjectId<DicRequestStatus>(ddDocument.StatusId),
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                TransferDate = GetNullableDate(ddDocument.Date85),
                Transliteration = ddDocument.Trasliteration,
                UserId = GetUserId(ddDocument.UserId),
                PublishDate = GetNullableDate(ddDocument.Dby)
            };

            InitializeTypeTrademark(result);

            return result;
        }

        private int? GetSpeciesTradeMarkId(DdDocument ddDocument)
        {
            var protDocTypeId = GetProtectionDocTypeByCode(MapOldRequestTypeToProtectionDocType.Get(ddDocument.DoctypeId));

            var protDocTypeCode = DictionaryHelper.GetDictionaryCodeById(nameof(DicProtectionDocType), protDocTypeId);

            if (!GetObjectId<DicProtectionDocSubType>(ddDocument.SubtypeId).HasValue && protDocTypeCode == DicProtectionDocType.Codes.Trademark)
            {
                return DictionaryHelper.GetSpeciesTradeMarkId(false);
            }

            return null;
        }

        private int? InitializeSubType(DdDocument ddDocument)
        {
            var oldTypeId = GetProtectionDocTypeByCode(MapOldRequestTypeToProtectionDocType.Get(ddDocument.DoctypeId));
            int? dicProtectionDocSubTypeId;

            switch (oldTypeId)
            {
                case ClDocumentId.Eapo:
                    dicProtectionDocSubTypeId = DictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocSubType), "EAPO");
                    break;

                case ClDocumentId.Rst:
                    dicProtectionDocSubTypeId = DictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocSubType), "PCT");
                    break;

                default:
                    dicProtectionDocSubTypeId = GetObjectId<DicProtectionDocSubType>(ddDocument.SubtypeId);
                    break;
            }

            return dicProtectionDocSubTypeId;
        }

        private void InitializeTypeTrademark(Request request)
        {
            var dicProtectionDocType = Executor.GetQuery<GetDicProtectionDocTypeByIdQuery>().Process(q => q.Execute(request.ProtectionDocTypeId));
            var typeCode = dicProtectionDocType?.Code;

            if (DicProtectionDocType.Codes.Trademark.Equals(typeCode) ||
                DicProtectionDocType.Codes.InternationalTrademark.Equals(typeCode))
            {
                var dicTypeTrademark = Executor.GetQuery<GetDicTypeTrademarkByCodeQuery>().Process(q => q.Execute(DicTypeTrademark.Codes.Combined));

                request.TypeTrademarkId = dicTypeTrademark.Id;
            }
        }

        #endregion

        #region Workflows

        private async Task FillWorkflows(int requestId, string number)
        {
            var wfSqlQuery = string.Format(ImportSqlQueryHelper.WfSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var oldWorkflows = await SqlDapperConnection.QueryAsync<WtPtWorkoffice>(wfSqlQuery, TargetConnectionString);
            foreach (var oldWorkflow in oldWorkflows)
            {
                var resuestWorkflow = CreateWorkflow(oldWorkflow, requestId);
                if (resuestWorkflow == null) continue;
                await Executor.GetCommand<CreateRequestWorkflowCommand>().Process(d => d.ExecuteAsync(resuestWorkflow));
                
                resuestWorkflow.DateCreate = new DateTimeOffset(oldWorkflow.DateCreate.GetValueOrDefault(DateTime.Now));

                await Executor.GetCommand<UpdateRequestWorkflowCommand>().Process(d => d.ExecuteAsync(resuestWorkflow));
            }
        }

        private RequestWorkflow CreateWorkflow(WtPtWorkoffice oldWorkoffice, int requestId)
        {
            try
            {
                var wf = new RequestWorkflow
                {
                    ControlDate = GetNullableDate(oldWorkoffice.ControlDate),
                    CurrentStageId = GetObjectId<DicRouteStage>(oldWorkoffice.ToStageId),
                    CurrentUserId = GetUserId(oldWorkoffice.ToUserId),
                    DateCreate = new DateTimeOffset(oldWorkoffice.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldWorkoffice.Stamp.GetValueOrDefault(DateTime.Now)),
                    Description = oldWorkoffice.Description,
                    ExternalId = oldWorkoffice.Id,
                    FromStageId = GetObjectId<DicRouteStage>(oldWorkoffice.FromStageId),
                    FromUserId = GetUserId(oldWorkoffice.FromUserId),
                    IsComplete = GenerateHelper.StringToNullableBool(oldWorkoffice.IsComplete),
                    IsMain = false,
                    IsSystem = GenerateHelper.StringToNullableBool(oldWorkoffice.IsSystem),
                    OwnerId = requestId,
                    RouteId = GetObjectId<DicRoute>(oldWorkoffice.TypeId),
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return wf;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Info

        private async Task FillInfo(int requestId, string number)
        {
            var infoSqlQuery = string.Format(ImportSqlQueryHelper.InfoSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var oldInfo = await SqlDapperConnection.QueryAsync<DdInfo>(infoSqlQuery, TargetConnectionString);
            var ddInfos = oldInfo.ToList();
            if (!ddInfos.Any()) return;
            var resuestInfo = CreateInfo(ddInfos.FirstOrDefault(), requestId);
            await Executor.GetCommand<CreateRequestInfoCommand>().Process(d => d.ExecuteAsync(resuestInfo));

            resuestInfo.DateCreate = new DateTimeOffset(ddInfos.First().DateCreate.GetValueOrDefault(DateTime.Now));

            await Executor.GetCommand<UpdateRequestInfoCommand>().Process(d => d.ExecuteAsync(resuestInfo));
        }

        private RequestInfo CreateInfo(DdInfo oldInfo, int requestId)
        {
            var info = new RequestInfo
            {
                AcceptAgreement = GenerateHelper.StringToNullableBool(oldInfo.RegTz),
                Breed = oldInfo.SelRoot,
                BreedCountryId = GetObjectId<DicCountry>(oldInfo.FlBreedCountry),
                BreedingNumber = oldInfo.SelNomer,
                DateCreate = new DateTimeOffset(oldInfo.DateCreate.GetValueOrDefault(DateTime.Now)),
                DateUpdate = new DateTimeOffset(oldInfo.Stamp.GetValueOrDefault(DateTime.Now)),
                ExternalId = oldInfo.Id,
                FlagHeirship = GenerateHelper.StringToBool(oldInfo.FlagTn),
                FlagNine = GenerateHelper.StringToBool(oldInfo.FlagNine),
                FlagTat = GenerateHelper.StringToBool(oldInfo.FlagTat),
                FlagTpt = GenerateHelper.StringToBool(oldInfo.FlagTpt),
                FlagTth = GenerateHelper.StringToBool(oldInfo.FlagTth),
                FlagTtw = GenerateHelper.StringToBool(oldInfo.FlagTtw),
                Genus = oldInfo.SelFamily,
                IsColorPerformance = GenerateHelper.StringToNullableBool(oldInfo.ColorTz),
                IsConventionPriority = GenerateHelper.StringToNullableBool(oldInfo.FlagTn),
                IsExhibitPriority = GenerateHelper.StringToNullableBool(oldInfo.AwardTz),
                IsStandardFont = GenerateHelper.StringToNullableBool(oldInfo.FontTz),
                IsVolumeTZ = GenerateHelper.StringToNullableBool(oldInfo.D3Tz),
                IzCollectiveTZ = GenerateHelper.StringToNullableBool(oldInfo.ColTz),
                Priority = oldInfo.TmPrioritet,
                ProductPlace = $"{oldInfo.PnPlace ?? string.Empty} {oldInfo.FlProductPalce ?? string.Empty}",
                ProductSpecialProp = $"{oldInfo.PnDsc ?? string.Empty} {oldInfo.FlProductSpecialProp ?? string.Empty}",
                ProductType = oldInfo.PnGoods,
                RequestId = requestId,
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                Translation = oldInfo.TmTranslate,
                Transliteration = oldInfo.TmTranslit
            };

            return info;
        }

        #endregion

        #region Colors

        private async Task FillColors(int requestId, string number)
        {
            var colorSqlQuery = string.Format(ImportSqlQueryHelper.ColorSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var colors = await SqlDapperConnection.QueryAsync<RfTmIcfem>(colorSqlQuery, TargetConnectionString);
            foreach (var color in colors)
            {
                var resuestColor = CreateColor(color, requestId);
                if (resuestColor == null) continue;
                Executor.GetCommand<CreateRequestColorTzCommand>().Process(d => d.Execute(resuestColor));
            }
        }

        private DicColorTZRequestRelation CreateColor(RfTmIcfem oldColor, int requestId)
        {
            try
            {
                var colorId = GetObjectId<DicColorTZ>(oldColor.LcfemId);

                if (colorId == null || colorId == 0) return null;

                var color = new DicColorTZRequestRelation
                {
                    ColorTzId = colorId.Value,
                    RequestId = requestId
                };

                return color;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Customers

        private async Task FillCustomers(int requestId, string number)
        {
            var customersSqlQuery = string.Format(ImportSqlQueryHelper.CustomersSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var customers = await SqlDapperConnection.QueryAsync<RfCustomerAddressExtention>(customersSqlQuery, TargetConnectionString);
            foreach (var customer in customers)
            {
                var resuestCustomer = await CreateCustomer(customer, requestId);
                if (resuestCustomer == null) continue;
                await Executor.GetCommand<CreateRequestCustomerCommand>().Process(d => d.ExecuteAsync(resuestCustomer));

                resuestCustomer.DateCreate = new DateTimeOffset(customer.DateCreate.GetValueOrDefault(DateTime.Now));

                await Executor.GetCommand<UpdateRequestCustomerCommand>().Process(d => d.ExecuteAsync(resuestCustomer));
            }
        }

        private async Task<RequestCustomer> CreateCustomer(RfCustomerAddressExtention oldCustomer, int requestId)
        {
            try
            {
                var customerId = GetObjectId<DicCustomer>(oldCustomer.CustomerId);

                if ((customerId == null || customerId == 0) && oldCustomer.CustomerId.HasValue)
                    customerId = await GetCustomer(oldCustomer.CustomerId.Value);

                if (customerId == null || customerId == 0)
                    return null;

                var customer = new RequestCustomer
                {
                    Address = oldCustomer.AddresNameRu,
                    CustomerId = customerId,
                    CustomerRoleId = GetObjectId<DicCustomerRole>(oldCustomer.CType),
                    DateCreate = new DateTimeOffset(oldCustomer.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldCustomer.Stamp.GetValueOrDefault(DateTime.Now)),
                    DateBegin = GetNullableDate(oldCustomer.DateBegin),
                    DateEnd = GetNullableDate(oldCustomer.DateEnd),
                    ExternalId = oldCustomer.Id,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    AddressEn = oldCustomer.AddresNameEn,
                    AddressKz = oldCustomer.AddresNameKz
                };

                return customer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Icfems

        private async Task FillIcfems(int requestId, string number)
        {
            var icfemsSqlQuery = string.Format(ImportSqlQueryHelper.IcfemsSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var icfems = await SqlDapperConnection.QueryAsync<RfTmIcfem>(icfemsSqlQuery, TargetConnectionString);
            foreach (var icfem in icfems)
            {
                var resuestIcfem = CreateIcfem(icfem, requestId);
                if (resuestIcfem == null) continue;
                Executor.GetCommand<CreateRequestIcfemCommand>().Process(d => d.Execute(resuestIcfem));
            }
        }
        
        private DicIcfemRequestRelation CreateIcfem(RfTmIcfem oldIcfem, int requestId)
        {
            try
            {
                var icfemId = GetObjectId<DicICFEM>(oldIcfem.LcfemId);

                if (icfemId == null || icfemId == 0) return null;

                var icfem = new DicIcfemRequestRelation
                {
                    DicIcfemId = icfemId.Value,
                    RequestId = requestId
                };

                return icfem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Icgs

        private async Task FillIcgs(int requestId, string number)
        {
            var icgsSqlQuery = string.Format(ImportSqlQueryHelper.IcgsSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var icgs = await SqlDapperConnection.QueryAsync<RfTmIcgs>(icgsSqlQuery, TargetConnectionString);
            foreach (var icg in icgs)
            {
                var resuestIcg = CreateIcg(icg, requestId);
                if (resuestIcg == null) continue;
                Executor.GetCommand<CreateRequestIcgsCommand>().Process(d => d.Execute(resuestIcg));

                resuestIcg.DateCreate = new DateTimeOffset(icg.DateCreate.GetValueOrDefault(DateTime.Now));

                await Executor.GetCommand<UpdateIcgsRequestCommand>().Process(d => d.ExecuteAsync(resuestIcg));
            }
        }

        private ICGSRequest CreateIcg(RfTmIcgs oldIcg, int requestId)
        {
            try
            {
                var icgId = GetObjectId<DicICGS>(oldIcg.IcpsId);

                if (icgId == null || icgId == 0) return null;

                var icg = new ICGSRequest
                {
                    ClaimedDescription = oldIcg.FlDscStarted,
                    DateCreate = new DateTimeOffset(oldIcg.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldIcg.Stamp.GetValueOrDefault(DateTime.Now)),
                    Description = oldIcg.Dsc,
                    DescriptionKz = oldIcg.DscKz,
                    ExternalId = oldIcg.Id,
                    IcgsId = icgId.Value,
                    IsNegative = GenerateHelper.StringToNullableBool(oldIcg.IsNegative),
                    IsNegativePartial = GenerateHelper.StringToNullableBool(oldIcg.FlIsNegativePartial),
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return icg;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Icis

        private async Task FillIcis(int requestId, string number)
        {
            var icisSqlQuery = string.Format(ImportSqlQueryHelper.IcisSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var icises = await SqlDapperConnection.QueryAsync<RfIcis>(icisSqlQuery, TargetConnectionString);
            foreach (var icis in icises)
            {
                var resuestIcis = CreateIcis(icis, requestId);
                if (resuestIcis == null) continue;
                Executor.GetCommand<CreateRequestIcisCommand>().Process(d => d.Execute(resuestIcis));

                resuestIcis.DateCreate = new DateTimeOffset(icis.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateRequestIcisCommand>().Process(d => d.Execute(resuestIcis));
            }
        }

        private ICISRequest CreateIcis(RfIcis oldIcis, int requestId)
        {
            try
            {
                var icisId = GetObjectId<DicICIS>(oldIcis.TypeId);

                if (icisId == null || icisId == 0) return null;

                var icis = new ICISRequest
                {
                    DateCreate = new DateTimeOffset(oldIcis.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = DateTimeOffset.Now,
                    ExternalId = oldIcis.Id,
                    IcisId = icisId.Value,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return icis;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Ipc

        private async Task FillIpc(int requestId, string number)
        {
            var ipcSqlQuery = string.Format(ImportSqlQueryHelper.IpcSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var ipces = await SqlDapperConnection.QueryAsync<RfIpc>(ipcSqlQuery, TargetConnectionString);
            foreach (var ipc in ipces)
            {
                var resuestIpc = CreateIpc(ipc, requestId);
                if (resuestIpc == null) continue;
                Executor.GetCommand<CreateRequestIpcCommand>().Process(d => d.Execute(resuestIpc));

                resuestIpc.DateCreate = new DateTimeOffset(ipc.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateRequestIpcCommand>().Process(d => d.Execute(resuestIpc));
            }
        }

        private IPCRequest CreateIpc(RfIpc oldIpc, int requestId)
        {
            try
            {
                var ipcId = GetObjectId<DicIPC>(oldIpc.TypeId);

                if (ipcId == null || ipcId == 0) return null;

                var ipc = new IPCRequest
                {
                    DateCreate = new DateTimeOffset(oldIpc.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = DateTimeOffset.Now,
                    ExternalId = oldIpc.Id,
                    IpcId = ipcId.Value,
                    IsMain = GenerateHelper.StringToNullableBool(oldIpc.FlIsMain) ?? false,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return ipc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region EarlyReg

        private async Task FillEarlyReg(int requestId, string number)
        {
            var earlyRegsSqlQuery = string.Format(ImportSqlQueryHelper.EarlyRegsSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), number);
            var earlyRegs = await SqlDapperConnection.QueryAsync<WtPtEarlyreg>(earlyRegsSqlQuery, TargetConnectionString);
            foreach (var earlyReg in earlyRegs)
            {
                var resuestEarlyReg = CreateEarlyReg(earlyReg, requestId);
                if (resuestEarlyReg == null) continue;
                Executor.GetCommand<CreateRequestEarlyRegCommand>().Process(d => d.Execute(resuestEarlyReg));

                resuestEarlyReg.DateCreate = new DateTimeOffset(earlyReg.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateRequestEarlyRegCommand>().Process(d => d.Execute(resuestEarlyReg));
            }
        }

        private RequestEarlyReg CreateEarlyReg(WtPtEarlyreg oldEarlyreg, int requestId)
        {
            try
            {
                var earlyRegTypeId = GetObjectId<DicEarlyRegType>(oldEarlyreg.EtypeId);

                if (earlyRegTypeId == null || earlyRegTypeId == 0) return null;

                var earlyReg = new RequestEarlyReg
                {
                    DateCreate = new DateTimeOffset(oldEarlyreg.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = DateTimeOffset.Now,
                    EarlyRegTypeId = earlyRegTypeId.Value,
                    ExternalId = oldEarlyreg.Id,
                    NameSD = oldEarlyreg.SaName,
                    RegCountryId = GetObjectId<DicCountry>(oldEarlyreg.ReqCountry),
                    RegDate = GetNullableDate(oldEarlyreg.ReqDate),
                    RegNumber = oldEarlyreg.ReqNumber,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return earlyReg;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Attachments

        private async Task FillAttachments(int newRequestId)
        {
            var request = SavedRequests.FirstOrDefault(d => d.Id == newRequestId);
            if (request?.ExternalId == null) return;

            var attachmentsSqlQuery = string.Format(ImportSqlQueryHelper.AttachmentsSqlQuery, request.ExternalId);
            var oldAttachments = await SqlDapperConnection.QueryAsync<DocumentData>(attachmentsSqlQuery, TargetAttachmentConnectionString);
            foreach (var oldAttachment in oldAttachments)
            {
                var attachment = await CreateAttachment(oldAttachment, request);
                if (attachment == null) continue;
                await Executor.GetCommand<CreateAttachmentCommand>().Process(d => d.ExecuteAsync(attachment));
            }
        }

        private async Task<Attachment> CreateAttachment(DocumentData oldAttachment, Request request)
        {
            try
            {
                var fileName = oldAttachment.FileName;
                var file = oldAttachment.File;
                var bucketName = GetBucketName(request.Id, null, null);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"{Guid.NewGuid()}.{FileTypes.Pdf}";
                }

                var originalName = GetFolderWithOriginalName(request.Id, null, null, fileName, null);
                var validName = fileName.MakeValidFileName();
                var contentType = FileTypeHelper.GetContentType(fileName);

                await _fileStorage.AddAsync(bucketName, originalName, file, contentType);

                var attachment = new Attachment
                {
                    AuthorId = DeveloperUserId,
                    ContentType = contentType,
                    BucketName = bucketName,
                    IsMain = true,
                    CopyCount = request.CopyCount,
                    PageCount = request.PageCount,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now,
                    OriginalName = originalName,
                    Length = file.Length,
                    ValidName = validName,
                    Hash = _generateHash.GenerateFileHash(file),
                    ExternalId = request.ExternalId,
                    RequestId = request.Id,
                    IsDeleted = false
                };

                return attachment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        private int? Valid(string number)
        {
            var existRequestId = Executor
                .GetQuery<GetRequestIdByIncomingNumberQuery>()
                .Process(d => d.Execute(number));

            return existRequestId;
        }
    }
}