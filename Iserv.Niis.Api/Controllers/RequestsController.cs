using AutoMapper.QueryableExtensions;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocSubTypes;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Dictionaries.DicTypeTrademarks;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.RequestCustomers;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Infrastructure.CustomResults;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.Model.Mappings;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Search;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using CreateDocumentHandler = Iserv.Niis.BusinessLogic.Documents.CreateDocumentHandler;
using CreateRequestCommand = Iserv.Niis.BusinessLogic.Requests.CreateRequestCommand;
using CreateRequestCustomerCommand = Iserv.Niis.WorkflowBusinessLogic.Customers.CreateRequestCustomerCommand;
using CreateRequestWorkflowCommand = Iserv.Niis.WorkflowBusinessLogic.Workflows.CreateRequestWorkflowCommand;
using GenerateRequestNumberHandler = Iserv.Niis.BusinessLogic.Requests.GenerateRequestNumberHandler;
using GetDicProtectionDocTypeByIdQuery = Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes.GetDicProtectionDocTypeByIdQuery;
using GetDicRouteStageByCodeQuery = Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage.GetDicRouteStageByCodeQuery;
using GetRequestByIdQuery = Iserv.Niis.BusinessLogic.Requests.GetRequestByIdQuery;
using SetCoefficientComplexityRequestHandler = Iserv.Niis.BusinessLogic.Requests.SetCoefficientComplexityRequestHandler;
using UpdateRequestCommand = Iserv.Niis.BusinessLogic.Requests.UpdateRequestCommand;
using Microsoft.Extensions.Primitives;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Intergrations;
using Iserv.Niis.Domain.Intergrations.SendRegNumber;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Helpers;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/[controller]")]
    public class RequestsController : BaseNiisApiController
    {
        #region Fields
        private readonly ISendRequestToLkService _sendRequestToLkService;
        private readonly ILkIntergarionHelper _lkIntergarionHelper;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly ICustomerUpdater _customerUpdater;
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
        private readonly ILogoUpdater _logoUpdater;
        private readonly IImportRequestHelper _importRequestHelper;
        private readonly List<string> _expirationStartsFromDateCreateOnStageCodes = new List<string>
        {
            RouteStageCodes.NMPT_03_2,
            RouteStageCodes.NMPT_02_2_0,
            RouteStageCodes.UM_02_2_7,
            RouteStageCodes.UM_02_2_2_2,
            RouteStageCodes.UM_03_2
        };
        private readonly List<string> _legalPersonsStageCodes = new List<string>
        {
            RouteStageCodes.NMPT_03_2_4,
            RouteStageCodes.NMPT_03_3_2
        };
        private readonly List<string> _cumulativeStageCodes = new List<string>
        {
            RouteStageCodes.UM_02_2_1
        };

        private readonly string _defaultCustomerRole = DicCustomerRole.Codes.Addressee;
        private readonly string _сorrespondenceCustomerRole = DicCustomerRole.Codes.Correspondence;
        #endregion

        #region Constructor

        public RequestsController(
            ILogoUpdater logoUpdater,
            DictionaryHelper dictionaryHelper,
            IDocumentGeneratorFactory templateGeneratorFactory,
            ICustomerUpdater customerUpdater, 
            IImportRequestHelper importRequestHelper, 
            ILkIntergarionHelper lkIntergarionHelper, 
            ISendRequestToLkService sendRequestToLkService)
        {
            _customerUpdater = customerUpdater;
            _importRequestHelper = importRequestHelper;
            _lkIntergarionHelper = lkIntergarionHelper;
            _sendRequestToLkService = sendRequestToLkService;
            _dictionaryHelper = dictionaryHelper;
            _templateGeneratorFactory = templateGeneratorFactory;
            _logoUpdater = logoUpdater;
        }

        #endregion

        #region Api Methods

        /// <summary>
        /// Производит поиск заявок, охранных документов (ОД) и договоров.
        /// </summary>
        /// <returns>Список из <see cref="IntellectualPropertyDto"/>.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            return GetByUserId(currentUserId);
        }

        [HttpGet("users/{userId}")]
        public IActionResult GetBy(int userId)
        {
            return GetByUserId(userId);
        }

        [HttpGet("shortInformation/{protectionDocCode}")]
        public async Task<IActionResult> GetList(string protectionDocCode)
        {
            if (string.IsNullOrWhiteSpace(protectionDocCode))
                return new ErrorResult($"Argument: {nameof(protectionDocCode)} is empty!");

            var requests = await Executor.GetQuery<GetRequestsByProtectionDocCodesQuery>()
                .Process(q => q.ExecuteAsync(protectionDocCode));
            var requestItemDtos = Mapper.Map<List<RequestItemDto>>(requests);
            return Ok(requestItemDtos);
        }

        [HttpPost("icgsRequests")]
        public async Task<IActionResult> GetICGSRequestsList([FromBody] int[] requestIds)
        {
            var icgsRequests = await Executor.GetQuery<GetICGSRequestsByRequestIdsQuery>().Process(q => q.ExecuteAsync(requestIds));
            var dtos = icgsRequests.ProjectTo<ICGSRequestItemDto>();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (request == null)
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, id);

            if (request.IsRead == false && request.CurrentWorkflow?.CurrentUserId == NiisAmbientContext.Current.User.Identity.UserId)
            {
                Executor.GetCommand<UpdateRequestAsReadCommand>().Process(c => c.Execute(id));
            }

            var requestDetailDto = Mapper.Map<Request, RequestDetailDto>(request, opt => opt.Items["RequestCustomers"] = request.RequestCustomers);

            requestDetailDto.HasRequiredOnCreate = await Executor.GetHandler<CheckIfRequestHasRequiredPropertiesOnCreateHandler>().Process(h => h.ExecuteAsync(id));

            return Ok(requestDetailDto);
        }

        //TODO: У нас есть серверная фильтрация, надо вызывать Get(), передавая параметры фильтрации
        [HttpGet("bycode/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var pdCode = code.Substring(code.IndexOf(";", StringComparison.Ordinal)).Split(';');

            var requests = await Executor.GetQuery<GetRequestsByProtectionDocCodesQuery>().Process(q => q.ExecuteAsync(pdCode));
            //TODO: Что за коды? Если есть отличительные признаки, может быть стоит вынести в БД?
            var onSpecificStageRequests = requests.Where(r => new[]
            {
                "B03.3.3",
                "TM03.3.4",
                "U03.4",
                "PO03.5",
                "NMPT03.4",
                "TMI03.3.4",
                "SA03.2.4" //SA03.2.4
            }.Contains(r.CurrentWorkflow?.CurrentStage?.Code ?? string.Empty)).ToList();

            var result = onSpecificStageRequests.AsQueryable().ProjectTo<IntellectualPropertySearchDto>();


            return Ok(result);
        }

        //TODO: У нас есть серверная фильтрация, надо вызывать Get(), передавая параметры фильтрации
        [HttpGet("bynumber")]
        public async Task<IActionResult> GetByNumber()
        {
            var paramString = Request.QueryString.Value.Replace("?", "").Replace("=", "").Split('$');
            if (paramString.Length < 2)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Read, Request.QueryString.Value);

            var requestNum = paramString[0];
            var protectionDocTypeId = Convert.ToInt32(paramString[1]);
            var request = await Executor.GetQuery<GetRequestByProtectionDocTypeIdCodeAndRequestNumQuery>()
                .Process(q => q.ExecuteAsync(protectionDocTypeId, requestNum));
            if (request == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Read, $"Parameters: {nameof(protectionDocTypeId)}, {nameof(requestNum)}");

            if (request.IsRead == false && request.CurrentWorkflow?.CurrentUserId == NiisAmbientContext.Current.User.Identity.UserId)
            {
                Executor.GetCommand<UpdateRequestAsReadCommand>().Process(c => c.Execute(request.Id));
            }

            var requestDetailDto = Mapper.Map<Request, RequestDetailDto>(request, opt => opt.Items["RequestCustomers"] = request.RequestCustomers);

            return Ok(requestDetailDto);
        }

        [HttpPost("numbers")]
        public async Task<IActionResult> GetNumbers([FromBody] RequestDetailDto detail)
        {
            var request = Mapper.Map<Request>(detail);
            Executor.GetHandler<GenerateRequestIncomingNumberHandler>().Process(h => h.Execute(request));
            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(request));
            request.DateCreate = DateTimeOffset.Now;
            return Ok(new
            {
                Barcode = request.Barcode,
                IncomingNumber = request.IncomingNumber,
                DateCreate = request.DateCreate
            });
        }

        /// <summary>
        /// Создает заявку.
        /// </summary>
        /// <param name="detailDto">Информация о заявке.</param>
        /// <returns>Созданная заявка.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestDetailDto detailDto)
        {
            var requestId = await Executor.GetCommand<CreateRequestCommand>().Process(c => c.ExecuteAsync(detailDto));
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));

            var addressee = request.Addressee;
            if (addressee != null)
            {
                addressee.Apartment = detailDto.Apartment;
                addressee.Address = detailDto.AddresseeAddress;
                addressee.ShortAddress = detailDto.AddresseeShortAddress;
                addressee.Republic = detailDto.Republic;
                addressee.Oblast = detailDto.Oblast;
                addressee.Region = detailDto.Region;
                addressee.City = detailDto.City;
                addressee.Street = detailDto.Street;
                addressee.ContactInfos = Mapper.Map<List<ContactInfo>>(detailDto.ContactInfos);
                //await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
                _customerUpdater.Update(addressee);


                var customerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), _defaultCustomerRole);
                var addresseeCustomer = new RequestCustomer
                {
                    CustomerId = request.AddresseeId,
                    RequestId = requestId,
                    CustomerRoleId = customerRoleId,
                    Address = addressee.Address,
                    AddressEn = addressee.AddressEn,
                    AddressKz = addressee.AddressKz,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now
                };
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(q => q.Execute(addresseeCustomer));

                var сorrespondenceСustomerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), _сorrespondenceCustomerRole);
                var сorrespondenceCustomer = new RequestCustomer
                {
                    CustomerId = request.AddresseeId,
                    RequestId = requestId,
                    CustomerRoleId = сorrespondenceСustomerRoleId,
                    Address = addressee.Address,
                    AddressEn = addressee.AddressEn,
                    AddressKz = addressee.AddressKz,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now
                };
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(q => q.Execute(сorrespondenceCustomer));
            }

            //request.Addressee = null;



            InitializeTypeTrademark(request);
            InitializeProtectionDocSubType(request);
            InitializeSpeciesTrademark(request);

            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var requestWorkflow = await Executor.GetQuery<GetInitialRequestWorkflowQuery>().Process(q => q.ExecuteAsync(request, userId));
            if (requestWorkflow != null)
            {
                await Executor.GetHandler<ProcessRequestWorkflowHandler>().Process(h => h.Handle(requestWorkflow, request));
            }

            if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeUsefulModelCode)
            {
                var subType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeAndProtectionDocTypeCodeQuery>()
                    .Process(q => q.Execute(DicProtectionDocTypeCodes.RequestTypeUsefulModelCode, DicProtectionDocSubtypeCodes.NationalUsefulModel));
                request.RequestTypeId = subType.Id;
            }

            await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));

            var createdRequestDetailDto = Mapper.Map<Request, RequestDetailDto>(request);

            return Ok(createdRequestDetailDto);
        }

        [HttpGet("canSplit/{id}")]
        public async Task<IActionResult> CanSplitRequest(int id)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (request.CurrentWorkflowId == null) return Ok(false);

            var canSplit = request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.TZ_03_3_2_2;

            return Ok(canSplit);
        }

        [HttpPost("split/{id}")]
        public async Task<IActionResult> SplitRequest(int id, [FromBody] IcgsDto[] icgsDtos)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            var newRequest = Mapper.Map<Request>(request);

            #region Создание заявки

            newRequest.ProtectionDocTypeId = request.ProtectionDocTypeId;
            newRequest.RequestTypeId = request.RequestTypeId;
            newRequest.DateCreate = DateTimeOffset.Now;
            await Executor.GetHandler<GenerateNumberForSplitRequestsHandler>().Process(h => h.ExecuteAsync(request, newRequest));
            var newRequestId = await Executor.GetCommand<CreateRequestCommand>().Process(c => c.ExecuteAsync(newRequest));

            var newWorkflows = Mapper.Map<RequestWorkflow[]>(request.Workflows);
            foreach (var newWorkflow in newWorkflows)
            {
                newWorkflow.OwnerId = newRequestId;
                Executor.GetCommand<CreateRequestWorkflowCommand>().Process(c => c.Execute(newWorkflow));
            }

            var initialStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.Execute(request.CurrentWorkflow.CurrentStage.Code));
            var previousStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.Execute(request.CurrentWorkflow.FromStage.Code));

            var previousWorkflow = new RequestWorkflow
            {
                CurrentUserId = NiisAmbientContext.Current.User.Identity.UserId,
                OwnerId = newRequestId,
                CurrentStageId = previousStage.Id,
                RouteId = previousStage.RouteId,
                IsComplete = previousStage.IsLast,
                IsSystem = previousStage.IsSystem,
                IsMain = previousStage.IsMain,
            };
            Executor.GetCommand<CreateRequestWorkflowCommand>().Process(c => c.Execute(previousWorkflow));

            var initialWorkflow = new RequestWorkflow
            {
                CurrentUserId = NiisAmbientContext.Current.User.Identity.UserId,
                OwnerId = newRequestId,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
                FromStageId = previousStage.Id
            };
            Executor.GetCommand<CreateRequestWorkflowCommand>().Process(c => c.Execute(initialWorkflow));


            newRequest.CurrentWorkflowId = initialWorkflow.Id;
            await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(newRequest));

            #endregion

            #region МКТУ

            var splitIcgsRequests = new List<ICGSRequest>();

            foreach (var icgsDto in icgsDtos)
            {
                var newIcgsRequest = Mapper.Map<ICGSRequest>(icgsDto);
                newIcgsRequest.RequestId = newRequestId;
                if (icgsDto.IsSplit == true)
                {
                    var oldIcgsRequest = await Executor.GetQuery<GetIcgsRequestByIdQuery>()
                        .Process(q => q.Execute(newIcgsRequest.Id));
                    oldIcgsRequest.Description = icgsDto.DescriptionNew;
                    await Executor.GetCommand<UpdateIcgsRequestCommand>().Process(c => c.ExecuteAsync(oldIcgsRequest));

                    newIcgsRequest.Id = 0;
                    Executor.GetCommand<CreateRequestIcgsCommand>().Process(c => c.Execute(newIcgsRequest));
                }
                else
                {
                    splitIcgsRequests.Add(newIcgsRequest);
                }
            }

            await Executor.GetCommand<UpdateIcgsRequestRangeCommand>()
                .Process(c => c.ExecuteAsync(id, splitIcgsRequests.ToList()));

            #endregion

            #region Биб. данные

            var newConventionInfos = Mapper.Map<RequestConventionInfo[]>(request.RequestConventionInfos);
            foreach (var conventionInfo in newConventionInfos)
            {
                conventionInfo.RequestId = newRequestId;
                Executor.GetCommand<CreateConventionInfoCommand>().Process(c => c.Execute(conventionInfo));
            }

            var newEarlyRegs = Mapper.Map<RequestEarlyReg[]>(request.EarlyRegs);
            foreach (var earlyReg in newEarlyRegs)
            {
                earlyReg.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestEarlyRegCommand>().Process(c => c.Execute(earlyReg));
            }

            var newIpcRequests = Mapper.Map<IPCRequest[]>(request.IPCRequests);
            foreach (var ipcRequest in newIpcRequests)
            {
                ipcRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateIpcRequestCommand>().Process(c => c.Execute(ipcRequest));
            }

            var newIcisRequests = Mapper.Map<ICISRequest[]>(request.ICISRequests);
            foreach (var icisRequest in newIcisRequests)
            {
                icisRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestIcisCommand>().Process(c => c.Execute(icisRequest));
            }

            foreach (var colorTz in request.ColorTzs)
            {
                var newColorTz = new DicColorTZRequestRelation
                {
                    ColorTzId = colorTz.ColorTzId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestColorTzCommand>().Process(c => c.Execute(newColorTz));
            }

            foreach (var icfem in request.Icfems)
            {
                var newIcfem = new DicIcfemRequestRelation
                {
                    DicIcfemId = icfem.DicIcfemId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestIcfemCommand>().Process(c => c.Execute(newIcfem));
            }

            foreach (var requestMediaFile in request.MediaFiles)
            {
                var file = await Executor.GetQuery<GetAttachmentFileQuery>().Process(q => q.Execute(requestMediaFile.Id));
                using (var ms = new MemoryStream(file))
                {
                    var formFile = new FormFile(ms, 0, ms.Length, requestMediaFile.OriginalName,
                        requestMediaFile.ValidName)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = requestMediaFile.ContentType
                    };
                    await Executor.GetHandler<UploadMediaFileHandler>()
                        .Process(h => h.ExecuteAsync(newRequestId, formFile));
                }
            }

            #endregion

            #region Контрагенты

            var newCustomers = Mapper.Map<RequestCustomer[]>(request.RequestCustomers);
            foreach (var customer in newCustomers)
            {
                customer.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(c => c.Execute(customer));
            }

            #endregion

            var relation = new RequestRequestRelation
            {
                ChildId = newRequestId,
                ParentId = id
            };
            Executor.GetCommand<CreateRequestRequestRelationCommand>().Process(c => c.Execute(relation));

            var children = await Executor.GetQuery<GetChildRequestsByParentRequestIdQuery>().Process(q => q.Execute(id));

            var notificationId = children
                .FirstOrDefault(r => r.Documents.Any(rd => rd.Document.Type.Code == DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification))
                ?.Documents.FirstOrDefault(rd => rd.Document.Type.Code == DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification)?.DocumentId;

            if (notificationId.HasValue)
            {
                Executor.GetCommand<CreateRequestDocumentCommand>().Process(c => c.Execute(new RequestDocument
                {
                    DocumentId = notificationId.Value,
                    RequestId = newRequestId
                }));
            }
            else
            {
                var userInputDto = new UserInputDto
                {
                    Code = DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification,
                    Fields = new List<KeyValuePair<string, string>>(),
                    OwnerId = newRequestId,
                    OwnerType = Owner.Type.ProtectionDoc
                };
                await Executor.GetHandler<CreateDocumentHandler>().Process(h => h.ExecuteAsync(newRequestId, Owner.Type.Request, DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification, DocumentType.Outgoing, userInputDto));
            }

            return Ok(newRequestId);
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertRequest([FromBody] ConvertDto convertDto)
        {
            var result = await Executor.GetHandler<ConvertRequestHandler>().Process(h => h.ExecuteAsync(convertDto));

            return Ok(result);
        }

        [HttpGet("printAddressee/{ownerId}/{ownerType}")]
        public IActionResult PrintAddressee(int ownerId, Owner.Type ownerType)
        {
            var documentGenerator = _templateGeneratorFactory.Create("Addressee");
            if (documentGenerator != null)
            {
                var dic = new Dictionary<string, object>
                {
                    {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                    {"RequestId", ownerId},
                    {"OwnerType", ownerType}
                };

                var generatedFile = documentGenerator.Process(dic);
                var file = generatedFile.File;
                var contentType = ContentType.Pdf;
                return File(file, contentType);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RequestDetailDto detailDto)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (request == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Update, id);

            Mapper.Map(detailDto, request);
            _logoUpdater.Update(request);

            //Todo workaround для изменения значений, связанных со справочниками
            request.Department = null;
            request.Division = null;
            request.ProtectionDocType = null;
            request.RequestType = null;
            request.ReceiveType = null;
            request.Addressee = null;
            request.ConventionType = null;
            request.SpeciesTradeMark = null;
            request.TypeTrademark = null;

            try
            {
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                await Executor.GetHandler<UpdateRequestHandler>().Process(r => r.Handle(request, detailDto));
                var updatedRequest = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));

                var addressee = updatedRequest.Addressee;
                if (addressee != null)
                {
                    addressee.Apartment = detailDto.Apartment;
                    addressee.ShortAddress = detailDto.AddresseeShortAddress;
                    addressee.Address = detailDto.AddresseeAddress;
                    addressee.Republic = detailDto.Republic;
                    addressee.Oblast = detailDto.Oblast;
                    addressee.Region = detailDto.Region;
                    addressee.City = detailDto.City;
                    addressee.Street = detailDto.Street;
                    addressee.ContactInfos = Mapper.Map<List<ContactInfo>>(detailDto.ContactInfos);
                    //addressee.ContactInfos = Mapper.Map<List<ContactInfo>>(detailDto.ContactInfos);
                    //await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
                    _customerUpdater.Update(addressee);
                    var correspondence = updatedRequest.RequestCustomers.FirstOrDefault(rc =>
                        rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);
                    if (correspondence != null)
                    {
                        correspondence.Customer = null;
                        correspondence.CustomerId = addressee.Id;
                        correspondence.Address = addressee.Address;
                        await Executor.GetCommand<UpdateRequestCustomerCommand>()
                            .Process(c => c.ExecuteAsync(correspondence));
                    }
                }

                var updatedRequestDetailDto = Mapper.Map<Request, RequestDetailDto>(updatedRequest);

                var requestWorkFlowRequest = new RequestWorkFlowRequest
                {
                    RequestId = updatedRequest.Id,
                    NextStageUserId = updatedRequest.CurrentWorkflow.CurrentUserId.Value,
                    NextStageCode = updatedRequest.CurrentWorkflow.CurrentStage.Code
                };
                NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
                await SetCoefficientComplexityRequest(updatedRequest);
                return Ok(updatedRequestDetailDto);
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException?.Message ?? e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Executor.GetCommand<DeleteRequestCommand>().Process(c => c.Execute(id));
            return NoContent();
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> Upload(int id)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (request == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Update, id);
            try
            {
                _logoUpdater.Update(request, Request.Form.Files[0]);
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));

                var result = $"/api/requests/{id}/image?{DateTimeOffset.Now.Ticks}";
                return Ok(new { url = result });
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException?.Message ?? e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}/image/{isPreview?}")]
        public async Task<IActionResult> Image(int id, bool isPreview = false)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (request == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Update, id);

            var imageArray = isPreview ? request.PreviewImage : request.Image;

            return File(imageArray, "image/png");
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}/{ownerType}/imageBase64")]
        public async Task<IActionResult> ImageBase64(int ownerId, Owner.Type ownerType)
        {
            var imageArray = new byte[0];

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    imageArray = request.Image;
                    break;
                case Owner.Type.Contract:
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    imageArray = protectionDoc.Image;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var imageDto = new ImageDto();

            if (imageArray != null)
                imageDto.Base64 = Convert.ToBase64String(imageArray);
            else imageDto.Base64 = null;

            return Ok(imageDto);
        }

        [AllowAnonymous]
        [HttpGet("{id}/media")]
        public async Task<IActionResult> MediaFile(int id)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            var mediaFile = request.MediaFiles.FirstOrDefault(m => m.IsDeleted != true);
            var bytes = await Executor.GetQuery<GetAttachmentFileQuery>()
                .Process(q => q.Execute(mediaFile?.Id ?? 0));

            return File(bytes, mediaFile?.ContentType);
        }

        [HttpPost("{id}/uploadmedia")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadMediaFile(int id)
        {
            await Executor.GetHandler<UploadMediaFileHandler>().Process(h => h.ExecuteAsync(id, Request.Form.Files[0]));

            return Ok($"/api/requests/{id}/media?{DateTimeOffset.Now.Ticks}");
        }

        /// <summary>
        /// Генерация рег. номера заявки
        /// </summary>
        /// <param name="id">Идентефикатор заявки</param>
        /// <param name="subtypeId">Подтип заявки</param>
        /// <returns>Рег. номер и статус отправки в ЛК</returns>
        [HttpGet("generateNumber/{id}/{subtypeId}")]
        public async Task<IActionResult> GenerateRequestNumber(int id, int subtypeId)
        {
            var subtype = Executor.GetQuery<GetDicProtectionDocSubtypeByIdQuery>().Process(q => q.Execute(subtypeId));
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            request.RequestType = subtype;
            request.RequestTypeId = subtype?.Id;
            await Executor.GetHandler<GenerateRequestNumberHandler>().Process(h => h.ExecuteAsync(request));

            ServerStatus status = null;

            if (request.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeed)
            {
                status = SendRegNumber(request);

                if (status.Code == SendRegNumberStatusCodes.Successfully)
                {
                    request.IsSyncRequestNum = true;
                    await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                }
            }

            return Ok(new
            {
                number = request.RequestNum,
                status
            });
        }

        /// <summary>
        /// Отправка рег. номера заявки в ЛК
        /// </summary>
        /// <param name="requestId">Идентефикатор заявки</param>
        /// <returns>Статус запроса</returns>
        [HttpGet("sendRegNumber/{requestId}")]
        public async Task<IActionResult> SendRegNumber(int requestId)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));

            var status = SendRegNumber(request);

            if (status.Code == SendRegNumberStatusCodes.Successfully)
            {
                request.IsSyncRequestNum = true;
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
            }

            return Ok(status);
        }

        /// <summary>
        /// Отправка заявки в ЛК
        /// </summary>
        /// <param name="requestId">Идентефикатор заявки</param>
        /// <returns>Статус запроса</returns>
        [HttpGet("requisitionSend/{requestId}")]
        public async Task<IActionResult> RequisitionSend(int requestId)
        {
            var status = await _sendRequestToLkService.Send(requestId);

            if (status.Code == SendRegNumberStatusCodes.Successfully)
            {
                var request = await Executor.GetQuery<GetBaseRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
                request.IsFromLk = true;
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
            }

            return Ok(status);
        }

        [HttpPost("workflow/{userId}")]
        public async Task<IActionResult> WorkflowCreateMultiple([FromBody] int[] ids, int userId)
        {
            //var isAllSelected = Convert.ToBoolean(Request.Query["isAllSelected"].ToString());
            //SelectionMode selectionMode;
            //switch (Request.Query["selectionMode"].ToString())
            //{
            //    case "0":
            //        selectionMode = SelectionMode.Including;
            //        break;
            //    case "1":
            //        selectionMode = SelectionMode.Except;
            //        break;
            //    default:
            //        throw new NotImplementedException();
            //}
            //ids = Executor.GetHandler<GetRequestSelectionFromJournalHandler>()
            //    .Process(h => h.Execute(isAllSelected, selectionMode, ids));
            //var multipleStages = new[]
            //{
            //    RouteStageCodes.TZFirstFullExpertize,
            //    RouteStageCodes.TZSecondFullExpertize,
            //    RouteStageCodes.TZ_03_2_2,
            //    RouteStageCodes.TZ_02_1
            //};

            //foreach (var id in ids)
            //{
            //    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(id));
            //    var nextStages = await Executor.GetQuery<GetNotAutomaticNextStagesByCurrentStageIdQuery>()
            //        .Process(q => q.ExecuteAsync(request.CurrentWorkflow.CurrentStageId ?? default(int)));
            //    var nextStageCode = nextStages.FirstOrDefault(n => multipleStages.Contains(n.Code))?.Code;
            //    var requestWorkFlowRequest = new RequestWorkFlowRequest
            //    {
            //        RequestId = id,
            //        NextStageCode = nextStageCode,
            //        NextStageUserId = userId
            //    };
            //    NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
            //}

            return NoContent();
        }

        [HttpGet("completeCreate/{requestId}")]
        public async Task<IActionResult> GetDoesRequestHaveRequiredPropertiesOnCreate(int requestId)
        {
            var formationStageCodes = new[]
            {
                         RouteStageCodes.TZFormationPerformerChoosing,
                         RouteStageCodes.POFormationPerformerChoosing,
                         RouteStageCodes.UMFormationPerformerChoosing,
            	//RouteStageCodes.PO_02_1,
                         RouteStageCodes.BFormationPerformerChoosing,
                         //RouteStageCodes.I_02_1,
                         RouteStageCodes.SAFormationPerformerChoosing
                         //RouteStageCodes.SA_02_1
            };

            var stages = await Executor.GetHandler<FilterNextStagesByRequestIdHandler>()
                .Process(h => h.ExecuteAsync(requestId));
            var requestWorkFlowRequest = new RequestWorkFlowRequest
            {
                RequestId = requestId,
                NextStageCode = stages.FirstOrDefault(s => formationStageCodes.Contains(s.Code))?.Code,
                IsAuto = true
            };
            NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);

            var request = Executor.GetQuery<WorkflowBusinessLogic.Requests.GetRequestByIdQuery>().Process(r => r.Execute(requestId));

            var addressee = request.RequestCustomers.FirstOrDefault(rc =>
                rc.CustomerRole.Code == DicCustomerRoleCodes.Addressee);
            if (addressee == null)
            {
                //addressee.Apartment = detailDto.Apartment;
                //addressee.Address = detailDto.AddresseeAddress;
                //addressee.ShortAddress = detailDto.AddresseeShortAddress;
                //addressee.Republic = detailDto.Republic;
                //addressee.Oblast = detailDto.Oblast;
                //addressee.Region = detailDto.Region;
                //addressee.City = detailDto.City;
                //addressee.Street = detailDto.Street;
                //addressee.ContactInfos = Mapper.Map<List<ContactInfo>>(detailDto.ContactInfos);
                ////await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
                //_customerUpdater.Update(addressee);


                var customerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), _defaultCustomerRole);
                var addresseeCustomer = new RequestCustomer
                {
                    CustomerId = request.AddresseeId,
                    RequestId = requestId,
                    CustomerRoleId = customerRoleId,
                    Address = request.Addressee.Address,
                    AddressEn = request.Addressee.AddressEn,
                    AddressKz = request.Addressee.AddressKz,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now
                };
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(q => q.Execute(addresseeCustomer));
            }

            var сorrespondence = request.RequestCustomers.FirstOrDefault(rc =>
                rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);
            if (сorrespondence == null)
            {
                var сorrespondenceСustomerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), _сorrespondenceCustomerRole);
                var сorrespondenceCustomer = new RequestCustomer
                {
                    CustomerId = request.AddresseeId,
                    RequestId = requestId,
                    CustomerRoleId = сorrespondenceСustomerRoleId,
                    Address = request.Addressee.Address,
                    AddressEn = request.Addressee.AddressEn,
                    AddressKz = request.Addressee.AddressKz,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now
                };
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(q => q.Execute(сorrespondenceCustomer));
            }

            await SetCoefficientComplexityRequest(request);
            var requestWorkflow = await Executor.GetQuery<GetRequestWorkflowByIdQuery>().Process(r => r.ExecuteAsync(request.CurrentWorkflowId ?? default(int)));
            var responseWorkflowDto = Mapper.Map<RequestWorkflow, WorkflowDto>(requestWorkflow);

            return Ok(responseWorkflowDto);
        }

        [HttpGet("importRequest/{number}")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportRequest(string number)
        {
            var requestId = await _importRequestHelper.ImportFromDb(number);

            return Ok(requestId);
        }

        [HttpGet("importRequestByDate/{date}")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportRequestByDate(string date)
        {
            var requestId = await _importRequestHelper.ImportRequestByDate(DateTime.Parse(date));

            return Ok(requestId);
        }

        /// <summary>
        /// Создание уведомлений в заявке по кодам, ТОЛЬКО ИСХОДЯЩИЕ!!!. 
        /// </summary>
        /// <param name="documentCodes">Коды уведомлений.</param>
        /// <param name="requestId">Идентефикатор заявки.</param>
        /// <returns></returns>
        [HttpPost("generateNotification/{requestId}")]
        public async Task<IActionResult> GenerateNotification([FromBody] string[] documentCodes, int requestId)
        {
            var existNotifications = Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(h => h.Execute(requestId));

            foreach (var documentCode in documentCodes)
            {
                if (string.IsNullOrEmpty(documentCode))
                    continue;

                //Проверка на наличие ранее созданного подобного типа.
                if (existNotifications.Any(d => d.Type.Code == documentCode))
                    continue;

                var userInputDto = new UserInputDto
                {
                    Code = documentCode,
                    Fields = new List<KeyValuePair<string, string>>(),
                    OwnerId = requestId,
                    OwnerType = Owner.Type.Request
                };
                Executor.GetHandler<WorkflowBusinessLogic.Document.CreateDocumentHandler>()
                    .Process(h => h.Execute(requestId, Owner.Type.Request, documentCode, DocumentType.Outgoing, userInputDto));
            }

            return Ok();
        }

        [HttpPost("arePDAccelerated")]
        public IActionResult ArePDAccelerated([FromBody]int[] protectionDocIds)
        {
            foreach (var protectionDocId in protectionDocIds)
            {
                var documents = Executor.GetQuery<GetDocumentsByProtectionDocIdAndTypeCodesQuery>().Process(q => q.Execute(protectionDocId, new string[] { "001.004G.1" }));
                if (documents != null && documents.Count > 0)
                    return Ok(true);
                var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(protectionDocId)).Result;
                if(protectionDoc != null && protectionDoc.RequestId.HasValue)
                {
                    documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(q => q.Execute(protectionDoc.RequestId.Value, new string[] { "001.004G.1" }));
                    if (documents != null && documents.Count > 0)
                        return Ok(true);
                }

            }

            return Ok(false);
        }

        /// <summary>
        /// Отправка статуса заявки
        /// </summary>
        /// <param name="requestId">Идентефикатор заявки</param>
        /// <param name="statusId">Идентефикатор статуса</param>
        /// <returns>Статус запроса</returns>
        [HttpPost("sendStatus/{requestId}/{statusId}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendStatus(int requestId, int statusId)
        {
            var request = await Executor.GetQuery<GetBaseRequestByIdQuery>().Process(d => d.ExecuteAsync(requestId));
            if (request == null)
            {
                throw new NotSupportedException("Заявка не найдена");
            }

            var requestProtectionDocType = _dictionaryHelper.GetDictionaryById(nameof(DicProtectionDocType), request.ProtectionDocTypeId);
            var protectionDocTypeId = requestProtectionDocType.ExternalId ?? requestProtectionDocType.Id;
            var status = (DicOnlineRequisitionStatus)_dictionaryHelper.GetDictionaryById(nameof(DicOnlineRequisitionStatus), statusId);

            var sendStatusBody = new SendStatusBody
            {
                Input = new SendStatus
                {
                    DocumentId = request.Barcode,
                    PatentTypeId = protectionDocTypeId,
                    StatusId = status?.ExternalId ?? statusId
                }
            };

            var result = _lkIntergarionHelper.CallWebService(sendStatusBody, SoapActions.SendStatus);

            if (result.Code == ServerStatusCodes.Successfully)
            {
                request.LastOnlineRequisitionStatusId = statusId;
                await Executor.GetCommand<UpdateRequestCommand>().Process(d => d.ExecuteAsync(request));
            }

            return Ok(result);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Отправка рег. номера заявка в ЛК
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Статус запроса</returns>
        private ServerStatus SendRegNumber(Request request)
        {
            var protectionDocTypeId = request.ProtectionDocType.ExternalId ?? request.ProtectionDocType.Id;

            var sendStatusBody = new SendRegNumberBody
            {
                Input = new SendRegNumber
                {
                    DocumentId = request.Barcode,
                    PatentTypeId = protectionDocTypeId,
                    DocumentRegNumber = request.RequestNum,
                    ApplicationDate = DateTime.Now.ToString("dd-MM-yyyy")
                }
            };

            var result = _lkIntergarionHelper.CallWebService(sendStatusBody, SoapActions.SendRegNumber);

            return result;
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

        /// <summary>
        /// Инициализирует подтип заявки.
        /// </summary>
        /// <param name="request">Заявка.</param>
        private void InitializeProtectionDocSubType(Request request)
        {
            string typeCode = request.ProtectionDocType.Code;
            string subtypeCode;

            switch (typeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalTradeMark;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalInvention;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalUsefulModel;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalIndustrialSample;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalNameOfOrigin;
                    break;

                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    subtypeCode = DicProtectionDocSubtypeCodes.NationalSelectionAchieve;
                    break;

                default:
                    subtypeCode = null;
                    break;
            }

            if (!(subtypeCode is null))
            {
                var subtype = Executor
                    .GetQuery<GetDicProtectionDocSubTypeByCodeAndProtectionDocTypeCodeQuery>()
                    .Process(query => query.Execute(typeCode, subtypeCode));

                request.RequestTypeId = subtype?.Id;
            }
        }

        /// <summary>
        /// Инициализирует тип заявки товарного знака.
        /// </summary>
        /// <param name="request">Заявка на товарный знак.</param>
        private void InitializeSpeciesTrademark(Request request)
        {
            if (request.ProtectionDocType.Code == DicProtectionDocTypeCodes.RequestTypeTrademarkCode)
            {
                request.SpeciesTradeMarkId = Executor
                    .GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                    .Process(query => query.Execute(DicProtectionDocSubtypeCodes.RegularTradeMark)).Id;
            }
        }

        private async Task SetCoefficientComplexityRequest(Request request)
        {
            var requestWorkflow = request.CurrentWorkflow;
            if (requestWorkflow.CurrentStage.Code == RouteStageCodes.I_03_2_4)
            {
                await Executor.GetHandler<SetCoefficientComplexityRequestHandler>().Process(h => h.ExecuteAsync(request.Id));
            }
        }

        #region Search Logic

        /// <summary>
        /// Возвращает отфильтрованный и отсортированный список с <see cref="IntellectualPropertyDto"/>
        /// по идентификатору пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список с <see cref="IntellectualPropertyDto"/>.</returns>
        private IActionResult GetByUserId(int userId)
        {
            IQueryable<IntellectualPropertyDto> query =
                GetIntellectualPropertyDtoQuery(userId);

            IQueryable<IntellectualPropertyDto> filteredQuery =
                FilterIntellectualPropertyDtoQuery(query, Request.Query);

            IQueryable<IntellectualPropertyDto> sortedQuery =
                SortIntellectualPropertyDtoQuery(filteredQuery, Request.Query);

            IPagedList<IntellectualPropertyDto> pagedList =
                PaginateIntellectualPropertyDtoQuery(sortedQuery, Request.GetPaginationParams());

            var result = pagedList.AsOkObjectResult(Response);
            return result;
        }

        /// <summary>
        /// Конвертирует отсортированный запрос к <see cref="IntellectualPropertyDto"/>
        /// в пагинированный список, используя параметры пагинации.
        /// </summary>
        /// <param name="sortedQuery">Отсортированный запрос.</param>
        /// <param name="paginationParams">Параметры пагинации.</param>
        /// <returns>Пагинированный список.</returns>
        private IPagedList<IntellectualPropertyDto> PaginateIntellectualPropertyDtoQuery(
            IQueryable<IntellectualPropertyDto> sortedQuery,
            PaginationParams paginationParams)
        {
            IPagedList<IntellectualPropertyDto> pagedList = sortedQuery.ToPagedList(paginationParams);
            List<IntellectualPropertyDto> pagedListItems = new List<IntellectualPropertyDto>();

            foreach (var dto in pagedList.Items)
            {
                dto.Priority = GetTaskPriority(dto);
                dto.ReviewDaysStage = GetStageDays(dto);
                dto.ReviewDaysAll = GetTotalDays(dto);
                dto.IpcCodes = GetIpcCodes(dto);
                pagedListItems.Add(dto);
            }

            pagedList.Items = pagedListItems;

            return pagedList;
        }

        /// <summary>
        /// Сортирует запрос, используя информацию из <see cref="IQueryCollection"/>.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для сортировки.</param>
        /// <returns>Отсортированный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> SortIntellectualPropertyDtoQuery(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string SortKey = "_sort";

            const string IpcCodes = "ipcCodes";
            const string IsIndustrial = "IsIndustrial_eq";

            IQueryable<IntellectualPropertyDto> sortedQuery = query;

            if (queryCollection.ContainsKey(SortKey))   
            {
                if (queryCollection[SortKey] == IpcCodes
                    && queryCollection.ContainsKey(IsIndustrial))
                {
                    if (queryCollection.TryGetValue(IsIndustrial, out StringValues isIndustrialString))
                    {
                        if (bool.TryParse(isIndustrialString.FirstOrDefault(), out bool isIndustrial)
                            && isIndustrial)
                        {
                            sortedQuery = sortedQuery
                                            .Where(dto => dto.ProtectionDocTypeId > 0
                                                && dto.IsIndustrial
                                                && (dto.RegNumber == null || dto.RegNumber == string.Empty))
                                            .OrderBy(r => r.RegNumber);
                        }
                        else
                        {
                            sortedQuery = sortedQuery
                                            .Where(r => r.ProtectionDocTypeId > 0
                                                && !r.IsIndustrial)
                                            .OrderBy(r => r.RegNumber);
                        }
                    }
                }
                else
                {
                    sortedQuery = sortedQuery.Sort(queryCollection);
                }
            }
            else
            {
                sortedQuery = query.OrderByDescending(dto => dto.DateCreate);
            }

            return sortedQuery;
        }

        /// <summary>
        /// Возвращает запрос к <see cref="IntellectualPropertyDto"/> по идентификатору пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Запрос к <see cref="IntellectualPropertyDto"/>.</returns>
        private IQueryable<IntellectualPropertyDto> GetIntellectualPropertyDtoQuery(int userId)
        {
            const string OwnerTypeKey = "ownerType_eq";

            if (!Request.Query.ContainsKey(OwnerTypeKey))
            {
                throw new ArgumentException("В веб-запросе отсутствует ключ фильтрации по типам объектов!!!");
            }

            string ownerType = Request.Query[OwnerTypeKey];

            return GetIntellectualPropertyQueryByOwnerType(ownerType, userId);
        }

        /// <summary>
        /// Возвращает запрос к <see cref="IntellectualPropertyDto"/> по типу родительской сущности и идентификатору пользователя.
        /// </summary>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Запрос с типом <see cref="IntellectualPropertyDto"/>.</returns>
        private IQueryable<IntellectualPropertyDto> GetIntellectualPropertyQueryByOwnerType(
            string ownerType, int userId)
        {
            const string RequestOwnerType = "1";
            const string ProtectionDocOwnerType = "2";
            const string ContractOwnerType = "3";

            switch (ownerType)
            {
                case RequestOwnerType:
                    var result1 = Executor.GetQuery<GetRequestsByUserIdQuery>()
                        .Process(q => q.Execute(userId))
                        .Select(IntellectualPropertyDto.MapFromRequest);
                    return result1;
                case ProtectionDocOwnerType:
                    //var result2 = Executor.GetQuery<GetProtectionDocsByUserIdQuery>()
                    //    .Process(q => q.Execute(userId))
                    //    .Select(p => new IntellectualPropertyDto.ProtectionDocWithCurentUserId { ProtectionDoc = p, UserId = userId })
                    //    .Select(IntellectualPropertyDto.MapFromProtectionDocument);
                    //return result2;
                    var result2 = Executor.GetQuery<GetProtectionDocsByUserIdQuery>()
                            .Process(q => q.Execute(userId))
                            .Select(p => new IntellectualPropertyDto.ProtectionDocWithCurentUserId { ProtectionDoc = p, UserId = userId });
                    
                   // var t = result2.Take(50).ToList();


                    var result21 = result2.Select(IntellectualPropertyDto.MapFromProtectionDocument);

                    //var t2 = result21.Take(50).ToList();
                    //if (t2 == null)
                    //{
                    //    return result21;
                    //}
                    return result21;

                case ContractOwnerType:
                    var result3 = Executor.GetQuery<GetContractsByUserIdQuery>()
                        .Process(q => q.Execute(userId))
                        .Select(IntellectualPropertyDto.MapFromContract);
                    return result3;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Фильтрует запрос, используя значения из <see cref="IQueryCollection"/> http запроса.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterIntellectualPropertyDtoQuery(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            query = FilterByInteger(query, queryCollection);
            query = FilterByBarcode(query, queryCollection);
            query = FilterByProtectionDocTypeName(query, queryCollection);
            query = FilterByProtectionDocTypeId(query, queryCollection);
            query = FilterByIncomingNumber(query, queryCollection);
            query = FilterByReqNumber(query, queryCollection);
            query = FilterByCurrentStageCode(query, queryCollection);
            query = FilterByCreateDateFrom(query, queryCollection);
            query = FilterByCreateDateTo(query, queryCollection);
            query = FilterByCurrentStageDateFrom(query, queryCollection);
            query = FilterByCurrentStageDateTo(query, queryCollection);
            query = FilterByCompleteness(query, queryCollection);
            query = FilterByProtectionDocumentActiveness(query, queryCollection);
            query = FilterByGosNumberGenerationPossibility(query, queryCollection);
            query = FilterByManualStageStatus(query, queryCollection);
            query = FilterByStatusId(query, queryCollection);

            return query;
        }

        /// <summary>
        /// Фильтрует запрос используя цифру в <see cref="IQueryCollection"/>.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллеция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByInteger(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)

        {
            const string QueryParameterWordName = "_q";

            string filterString = string.Empty;

            if (queryCollection.ContainsKey(QueryParameterWordName)
                && queryCollection.TryGetValue(QueryParameterWordName, out var value))
            {
                filterString = value.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(filterString))
            {
                if (int.TryParse(filterString, out int _))
                {
                    query = query.Where(r =>//r.ReviewDaysStage == integerFilterString
                                            //|| r.ReviewDaysAll == integerFilterString
                                            r.Barcode == filterString
                                            || r.RegNumber.Contains(filterString));
                }
                else
                {
                    query = query.Where(r => r.ProtectionDocTypeValue.Contains(filterString)
                                           || r.CurrentStageValue.Contains(filterString)
                                           || r.RegNumber.Contains(filterString)
                                           || r.CurrentStageValue == filterString);
                }
            }

            return query;
        }

        /// <summary>
        /// Фильрует запрос по штрихкоду в <see cref="IQueryCollection"/>.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByBarcode(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string QueryParameterBarcodeKey = "Barcode_eq";

            if (queryCollection.ContainsKey(QueryParameterBarcodeKey)
                && queryCollection.TryGetValue(QueryParameterBarcodeKey, out StringValues barcodeValue))
            {
                string barcode = barcodeValue.ToString();

                if (!string.IsNullOrWhiteSpace(barcode))
                {
                    query = query.Where(r => r.Barcode.Contains(barcode));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по названию защитного документа.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByProtectionDocTypeName(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string ProtectionDocTypeKey = "protectionDocTypeValue_eq";

            if (queryCollection.ContainsKey(ProtectionDocTypeKey))
            {
                if (queryCollection.TryGetValue(ProtectionDocTypeKey, out StringValues protectionDocTypeValue))
                {
                    string protectionDocTypeName = protectionDocTypeValue.FirstOrDefault();

                    if (!string.IsNullOrEmpty(protectionDocTypeName))
                    {
                        int[] protDocTypeIds = Executor
                            .GetQuery<GetDicProtectionDocTypeLikeNameQuery>()
                            .Process(f => f.Execute(protectionDocTypeName));
                        query = query.Where(r => protDocTypeIds.Contains(r.ProtectionDocTypeId));
                    }
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по идентификатору охранного документа.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByProtectionDocTypeId(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string ProtectionDocTypeIdKey = "protectionDocTypeId_eq";

            if (queryCollection.ContainsKey(ProtectionDocTypeIdKey)
                && queryCollection.TryGetValue(ProtectionDocTypeIdKey, out StringValues protectionDocTypeIdValue))
            {
                int.TryParse(protectionDocTypeIdValue.FirstOrDefault(), out int protectionDocTypeIdValueWorkflowStage);
                query = query.Where(r => r.ProtectionDocTypeId == protectionDocTypeIdValueWorkflowStage);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по входящему номеру.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByIncomingNumber(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string IncomingNumberKey = "incomingNumber_eq";

            if (queryCollection.ContainsKey(IncomingNumberKey)
                && queryCollection.TryGetValue(IncomingNumberKey, out StringValues incomingNumberValue))
            {
                string incomingNumber = incomingNumberValue.ToString();

                if (!string.IsNullOrWhiteSpace(incomingNumber))
                {
                    query = query.Where(r => r.IncomingNumber.Contains(incomingNumber));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по регистрационному номеру.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByReqNumber(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {

            const string RegNumberKey = "regNumber_eq";

            if (queryCollection.ContainsKey(RegNumberKey)
                && queryCollection.TryGetValue(RegNumberKey, out StringValues regNumberValue))
            {
                string regNumber = regNumberValue.ToString();

                if (!string.IsNullOrWhiteSpace(regNumber))
                {
                    query = query.Where(r => r.RegNumber.Contains(regNumber));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по коду текущего этапа.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCurrentStageCode(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CurrentStageKey = "currentStageCode_eq";

            if (queryCollection.ContainsKey(CurrentStageKey)
                && queryCollection.TryGetValue(CurrentStageKey, out StringValues currentStageCodeValue))
            {
                string currentStageCodesString = currentStageCodeValue.ToString();

                if (!string.IsNullOrWhiteSpace(currentStageCodesString))
                {
                    const char CodeSeparator = ';';

                    string[] currentStageCodes = currentStageCodesString
                                                                .Split(CodeSeparator)
                                                                .Select(id => id.TrimEnd())
                                                                .ToArray();

                    query = query.Where(r => currentStageCodes.Contains(r.CurrentStageCode));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по дате создания, задавая дату, с которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCreateDateFrom(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CreateDateFromKey = "createDate_from";

            if (queryCollection.ContainsKey(CreateDateFromKey)
                && queryCollection.TryGetValue(CreateDateFromKey, out StringValues createDateFromValue))
            {
                DateTimeOffset.TryParse(createDateFromValue.FirstOrDefault(),
                    out DateTimeOffset createDateFrom);

                query = query.Where(r => r.DateCreate >= createDateFrom);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по дате создания, задавая дату, до которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCreateDateTo(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CreateDateToKey = "createDate_to";

            if (queryCollection.ContainsKey(CreateDateToKey)
                && queryCollection.TryGetValue(CreateDateToKey, out var createDateToValue))
            {
                DateTimeOffset.TryParse(createDateToValue.FirstOrDefault(),
                    out DateTimeOffset createDateTo);

                query = query.Where(r => r.DateCreate <= createDateTo);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по дате текущего этапа, задавая дату, с которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCurrentStageDateFrom(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CurrentStageDateFromKey = "currentStageDate_from";

            if (queryCollection.ContainsKey(CurrentStageDateFromKey)
                && queryCollection.TryGetValue(CurrentStageDateFromKey, out StringValues currentStageDateFromValue))
            {
                DateTimeOffset.TryParse(currentStageDateFromValue.FirstOrDefault(),
                    out DateTimeOffset currentStageDateFrom);

                query = query.Where(r => r.CurrentStageDate >= currentStageDateFrom);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по дате текущего этапа, задавая дату, до которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCurrentStageDateTo(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CurrentStageDateToKey = "currentStageDate_to";

            if (queryCollection.ContainsKey(CurrentStageDateToKey)
                && queryCollection.TryGetValue(CurrentStageDateToKey, out StringValues currentStageDateToValue))
            {
                DateTimeOffset.TryParse(currentStageDateToValue.FirstOrDefault(),
                    out DateTimeOffset currentStageDateTo);

                query = query.Where(r => r.DateCreate <= currentStageDateTo);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по состоянию окончания.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByCompleteness(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string IsCompleteKey = "isComplete_eq";

            if (queryCollection.ContainsKey(IsCompleteKey)
                && queryCollection.TryGetValue(IsCompleteKey, out StringValues isCompleteValue))
            {
                bool.TryParse(isCompleteValue.FirstOrDefault(), out bool isComplete);

                query = query.Where(r => r.IsComplete == isComplete);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по активности охранного документ (ОД).
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByProtectionDocumentActiveness(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string IsActiveProtectionDocumentKey = "isActiveProtectionDocument_eq";

            if (queryCollection.ContainsKey(IsActiveProtectionDocumentKey)
                && queryCollection.TryGetValue(IsActiveProtectionDocumentKey, out StringValues isActiveProtectionDocumentValue))
            {
                bool.TryParse(isActiveProtectionDocumentValue.FirstOrDefault(), out bool isActiveProtectionDocument);

                query = query.Where(r => r.IsActiveProtectionDocument == isActiveProtectionDocument);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по возможности генерации гос.номера.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByGosNumberGenerationPossibility(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string CanGenerateGosNumberKey = "canGenerateGosNumber_eq";

            if (queryCollection.ContainsKey(CanGenerateGosNumberKey)
                && queryCollection.TryGetValue(CanGenerateGosNumberKey, out StringValues canGenerateGosNumberValue))
            {

                bool.TryParse(canGenerateGosNumberValue.FirstOrDefault(), out bool canGenerateGosNumber);

                query = query.Where(r => r.CanGenerateGosNumber == canGenerateGosNumber);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по типу (ручной, авто) текущего этапа.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByManualStageStatus(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string IsManualStageKey = "isManualStage_eq";

            if (queryCollection.ContainsKey(IsManualStageKey)
                && queryCollection.TryGetValue(IsManualStageKey, out StringValues value))
            {
                bool.TryParse(value.FirstOrDefault(), out bool isManualStage);
                //todo! сделать выборку для ручных этапов из базы. Ждем лута.
                var stages = new[]
                {
                        RouteStageCodes.TZFormationPerformerChoosing,
                        RouteStageCodes.TZFirstFullExpertizePerformerChoosing,
                        RouteStageCodes.TZSecondFullExpertizePerformerChoosing,
                        RouteStageCodes.TZ_03_2_1
                    };

                query = query
                    .Where(r => isManualStage ? stages.Contains(r.CurrentStageCode) : !stages.Contains(r.CurrentStageCode));
            }

            return query;
        }

        /// <summary>
        /// Фильтрует запрос по идентификатору статуса.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации запроса.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<IntellectualPropertyDto> FilterByStatusId(
            IQueryable<IntellectualPropertyDto> query,
            IQueryCollection queryCollection)
        {
            const string StatusIdKey = "statusId_eq";

            if (queryCollection.ContainsKey(StatusIdKey)
                && queryCollection.TryGetValue(StatusIdKey, out StringValues statusIdValue))
            {
                string statusIdsValueStr = statusIdValue.ToString();

                if (!string.IsNullOrWhiteSpace(statusIdsValueStr))
                {
                    int[] statusIds = statusIdsValueStr
                        .Split(';')
                        .Select(id => Convert.ToInt32(id))
                        .ToArray();

                    query = query.Where(r => r.StatusId.HasValue && statusIds.Contains(r.StatusId.Value));
                }
            }

            return query;
        }

        #endregion

        private TaskPriority GetTaskPriority(IntellectualPropertyDto dto)
        {
            //если прошли сроки оплаты предварительной экспертизы, то подсвечивать оранжевым
            if (dto.IsFormalExamFeeNotPaidInTime == true)
            {
                return TaskPriority.Orange;
            }
            var expirationType = dto.ExpirationType;
            var expirationValue = dto.ExpirationValue;
            DateTimeOffset? dateFrom = dto.CurrentStageDate;
            var currentStageCode = dto.CurrentStageCode;
            //Исключительные случаи
            if (_expirationStartsFromDateCreateOnStageCodes.Contains(currentStageCode))
            {
                //Отсчет идет от даты подачи заявки
                dateFrom = dto?.RequestDate ?? null;
            }
            if (_legalPersonsStageCodes.Contains(currentStageCode))
            {
                if (dto.HasLegalDeclarants)
                {
                    expirationType = ExpirationType.CalendarMonth;
                    expirationValue = 1;
                }
            }
            if (expirationType == ExpirationType.None || dateFrom == null || expirationValue == null)
            {
                return TaskPriority.Normal;
            }
            var executionDate = Executor.GetHandler<CalculateExecutionDateHandler>()
                .Process(h => h.Execute(dateFrom.Value, expirationType, expirationValue ?? 0));


            if (_cumulativeStageCodes.Contains(currentStageCode))
            {
                var currentStageDay = dto.CurrentStageDate.Day;
                var currentStageDate = dto.CurrentStageDate;
                if (currentStageDay < 10)
                {
                    executionDate =
                        new DateTimeOffset(currentStageDate.Year, currentStageDate.Month, 10, 0, 0, 0, currentStageDate.Offset);
                }
                else if (currentStageDay < 20)
                {
                    executionDate = new DateTimeOffset(currentStageDate.Year, currentStageDate.Month, 20, 0, 0, 0,
                        currentStageDate.Offset);
                }
                else
                {
                    executionDate = new DateTimeOffset(currentStageDate.AddMonths(1).Year,
                        currentStageDate.AddMonths(1).Month, 1, 0, 0, 0, currentStageDate.Offset);
                }
            }

            if (RouteStageCodes.UM_03_8 == currentStageCode)
            {
                //Расчет сроков публикации полезных моделей
                executionDate = Executor.GetHandler<CalculateExecutionDateHandler>()
                    .Process(h => h.Execute(dto.RequestDate ?? dto.DateCreate, ExpirationType.CalendarMonth, 12));
                var registerDifferenceInDays = (executionDate - DateTimeOffset.Now).Days;
                if (registerDifferenceInDays < 4)
                {
                    return TaskPriority.Red;
                }
                if (registerDifferenceInDays < 6)
                {
                    return TaskPriority.Yellow;
                }
                return TaskPriority.Normal;
            }

            var differenceInDays = (DateTimeOffset.Now - executionDate).Days;
            if (differenceInDays < 1)
            {
                return TaskPriority.Normal;
            }
            if (differenceInDays < 5)
            {
                return TaskPriority.Yellow;
            }

            return TaskPriority.Red;
        }

        private int GetStageDays(IntellectualPropertyDto dto)
        {
            return (DateTimeOffset.Now - dto.CurrentStageDate).Days;
        }

        private int GetTotalDays(IntellectualPropertyDto dto)
        {
            return (DateTimeOffset.Now - dto.DateCreate).Days;
        }

        private string GetIpcCodes(IntellectualPropertyDto dto)
        {
            switch (dto.OwnerType)
            {
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdWithIpcsQuery>().Process(q => q.Execute(dto.Id));
                    if (new[]
                {
                    DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                    DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                    DicProtectionDocTypeCodes.RequestTypeInventionCode,
                    DicProtectionDocTypeCodes.RequestTypeUsefulModelCode
                }.Contains(protectionDoc.Type.Code))
                    {
                        return protectionDoc.IpcProtectionDocs.Any(i => i.IsMain)
                            ? protectionDoc.IpcProtectionDocs.Where(i => i.IsMain).Select(i => i.Ipc.Code)
                            .First()
                        : protectionDoc.RegNumber;
                    }
                    return protectionDoc.RegNumber;
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdWithIpcQuery>().Process(q => q.Execute(dto.Id));
                    if (request.IPCRequests.Any())
                    {
                        return string.Join(", ", request.IPCRequests.Select(i => i.Ipc.Code));
                    }
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}