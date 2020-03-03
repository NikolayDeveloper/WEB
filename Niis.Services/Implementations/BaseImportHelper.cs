using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers;
using Iserv.Niis.BusinessLogic.Users;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.OldNiisEntities;
using Iserv.Niis.Services.Dapper;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Services.Implementations
{
    public class BaseImportHelper : BaseService, IBaseImportHelper
    {
        protected string TargetConnectionString;
        protected string SourceConnectionString;
        protected string TargetAttachmentConnectionString;

        protected readonly IConfiguration Configuration;
        protected readonly DictionaryHelper DictionaryHelper;


        public BaseImportHelper(
            IConfiguration configuration, 
            DictionaryHelper dictionaryHelper
            )
        {
            Configuration = configuration;
            DictionaryHelper = dictionaryHelper;

            TargetAttachmentConnectionString = Configuration.GetConnectionString("NiisDesctopAttachmentsConnection");
            TargetConnectionString = Configuration.GetConnectionString("NiisDesctopConnection");
            SourceConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        protected int GetProtectionDocTypeByCode(string code)
        {
            var pdd = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(d => d.Execute(code));

            if (pdd == null)
                pdd = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(d => d.Execute(DicProtectionDocType.Codes.Other));

            return pdd.Id;
        }

        protected int? GetUserId(int? externalId)
        {
            if (externalId is null || externalId == 0) return null;

            var resultId = Executor.GetQuery<GetUserByExternalId>().Process(d => d.Execute(externalId.Value));

            return resultId;
        }

        protected int? GetObjectId<TEntity>(int? externalId) where TEntity : Entity<int>
        {
            if (externalId is null || externalId == 0) return null;

            var resultId = Executor.GetQuery<GetIdentityFromObjectQuery>().Process(d => d.Execute<TEntity>(externalId.Value));

            return resultId;
        }

        protected DateTimeOffset? GetNullableDate(DateTime? date)
        {
            return date.HasValue
                ? new DateTimeOffset(date.Value)
                : (DateTimeOffset?)null;
        }

        /// <summary>
        /// Получает папку для Minio.
        /// </summary>
        /// <param name="requestId">ID заявки.</param>
        /// <param name="documentId">ID документа.</param>
        /// <param name="contractId">ID контракта.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="extentionPath">Расширенный путь</param>
        /// <returns></returns>
        protected string GetFolderWithOriginalName(int? requestId, int? documentId, int? contractId, string fileName, string extentionPath)
        {
            string bucketName = string.Empty;
            var extention = Path.GetExtension(fileName);
            var newFileName = $"{Guid.NewGuid().ToString()}{extention}";

            var extentionPathResult = extentionPath != null ? $"{extentionPath}/" : string.Empty;

            if (requestId != null)
            {
                bucketName = $"old/{requestId}/{newFileName}";
            }

            if (documentId != null)
            {
                bucketName = $"old/{extentionPathResult}{documentId}/{newFileName}";
            }

            if (contractId != null)
            {
                bucketName = $"old/{contractId}/{newFileName}";
            }

            return bucketName;
        }

        public static string GetDocumentTypeName(byte documentType)
        {
            switch (documentType)
            {
                case (byte)DocumentType.Incoming: //Обработка входящей корреспонденции 
                    return "Incoming";
                case (byte)DocumentType.Outgoing: //Обработка исходящей корреспонденции
                    return "Outgoing";
                case (byte)DocumentType.Internal: //Внутренняя переписка
                    return "Internal";
                case (byte)DocumentType.DocumentRequest: //Документ даявки
                    return "DocumentRequest";
                default:
                    return "DocumentRequest";
            }
        }

        /// <summary>
        /// Получает Bucket для Minio.
        /// </summary>
        /// <param name="requestId">ID заявки.</param>
        /// <param name="documentId">ID документа.</param>
        /// <param name="contractId">ID контракта.</param>
        /// <returns>Bucket для Minio.</returns>
        protected string GetBucketName(int? requestId, int? documentId, int? contractId)
        {
            var bucketName = string.Empty;

            if (requestId != null)
            {
                bucketName = "requests";
            }

            if (documentId != null)
            {
                bucketName = "documents";
            }

            if (contractId != null)
            {
                bucketName = "contracts";
            }

            return bucketName;
        }

        /// <summary>
        /// Создание контрагента
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        protected async Task<int?> GetCustomer(int customerId)
        {
            var customersSqlQuery = string.Format(ImportSqlQueryHelper.DicCustomersSqlQuery, customerId);
            var customers = await SqlDapperConnection.QueryAsync<CustomerAddressStringExtension>(customersSqlQuery, TargetConnectionString);
            
            var ddCustomers = customers.ToList();
            if (!ddCustomers.Any()) return null;
            var customer = CreateCustomer(ddCustomers.FirstOrDefault());
            if (customer == null) return null;

            var newCustomerId = await Executor.GetCommand<CreateDicCustomerCommand>().Process(d => d.ExecuteAsync(customer));

            customer.Id = newCustomerId;

            if (!string.IsNullOrEmpty(ddCustomers.First().Phone))
            {
                var phone = FillContactInfo(DicContactTypesCodes.Phone, newCustomerId, ddCustomers.First().Phone);
                customer.ContactInfos.Add(phone);
            }

            if (!string.IsNullOrEmpty(ddCustomers.First().Fax))
            {
                var fax = FillContactInfo(DicContactTypesCodes.Fax, newCustomerId, ddCustomers.First().Fax);
                customer.ContactInfos.Add(fax);
            }

            if (!string.IsNullOrEmpty(ddCustomers.First().Email))
            {
                var email = FillContactInfo(DicContactTypesCodes.Email, newCustomerId, ddCustomers.First().Email);
                customer.ContactInfos.Add(email);
            }

            await Executor.GetCommand<UpdateDicCustomerCommand>().Process(d => d.ExecuteAsync(customer));

            //var customerInfo = CreateCustomerInfo(newCustomerId, ddCustomers.FirstOrDefault());
            //if (customerInfo == null) return null;
            //await Executor.GetCommand<CreateAttorneyInfoCommand>().Process(d => d.ExecuteAsync(customerInfo));

            return newCustomerId;
        }

        private ContactInfo FillContactInfo(string type, int newCustomerId, string info)
        {
            var contactInfo = new ContactInfo
            {
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                ExternalId = newCustomerId,
                Info = info,
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                TypeId = DictionaryHelper.GetDictionaryIdByCode(nameof(DicContactInfoType), type)
            };

            return contactInfo;
        }
        
        //private CustomerAttorneyInfo CreateCustomerInfo(int customerId, CustomerAddressStringExtension oldCustomer)
        //{
        //    var attorneyInfo = new CustomerAttorneyInfo
        //    {
        //        CustomerId = customerId,
        //        DateBeginStop = GetNullableDate(oldCustomer.AttDateBeginStop),
        //        DateCard = GetNullableDate(oldCustomer.AttDateCard),
        //        DateCreate = DateTimeOffset.Now,
        //        DateDisCard = GetNullableDate(oldCustomer.AttDateDiscard),
        //        DateEndStop = GetNullableDate(oldCustomer.AttDateEndStop),
        //        DateUpdate = DateTimeOffset.Now,
        //        Education = oldCustomer.AttEducation,
        //        ExternalId = oldCustomer.Id,
        //        FieldOfActivity = oldCustomer.AttSphereWork,
        //        FieldOfKnowledge = oldCustomer.AttSphereKnow,
        //        GovReg = oldCustomer.AttStatReg,
        //        GovRegDate = GetNullableDate(oldCustomer.AttStatRegDate),
        //        Language = oldCustomer.AttLang,
        //        PaymentOrder = oldCustomer.AttPlatpor,
        //        PublicRedefine = oldCustomer.AttPublicRedefine,
        //        Redefine = oldCustomer.AttRedefine,
        //        RegCode = oldCustomer.AttCode,
        //        SomeDate = oldCustomer.AttSomeDate,
        //        Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
        //        WorkPlace = oldCustomer.AttWorkPlace
        //    };

        //    return attorneyInfo;
        //}

        private DicCustomer CreateCustomer(CustomerAddressStringExtension oldCustomer)
        {
            var typeId = GetObjectId<DicCustomerType>(oldCustomer.TypeId);
            if(typeId == null || typeId == 0) return null;

            var customer = new DicCustomer
            {
                Address = oldCustomer.AddresNameRu,
                ApplicantsInfo = oldCustomer.FlApplicantsInfo,
                CertificateNumber = oldCustomer.FlCertificateNumber,
                CertificateSeries = oldCustomer.FlCertificateSeries,
                ContactName = oldCustomer.ContactFace,
                CountryId = GetObjectId<DicCountry>(oldCustomer.CountryId),
                DateCreate = new DateTimeOffset(oldCustomer.DateCreate.GetValueOrDefault(DateTime.Now)),
                DateUpdate = new DateTimeOffset(oldCustomer.Stamp.GetValueOrDefault(DateTime.Now)),
                Email = oldCustomer.Email,
                ExternalId = oldCustomer.Id,
                IsBeneficiary = false,
                IsNotMention = false,
                IsSMB = GenerateHelper.StringToNullableBool(oldCustomer.FlIsSmb),
                JurRegNumber = oldCustomer.JurRegNumber,
                Login = oldCustomer.Login,
                NameEn = oldCustomer.CusNameMlEn,
                NameEnLong = oldCustomer.CusNameMlEnLong,
                NameKz = oldCustomer.CusNameMlKz,
                NameKzLong = oldCustomer.CusNameMlKzLong,
                NameRu = oldCustomer.CusNameMlRu,
                NameRuLong = oldCustomer.CusNameMlRuLong,
                NotaryName = oldCustomer.FlNotaryName,
                Opf = oldCustomer.FlOpf,
                Password = oldCustomer.Password,
                Phone = oldCustomer.Phone,
                PhoneFax = oldCustomer.Fax,
                PowerAttorneyDateIssue = GetNullableDate(oldCustomer.FlPowerAttorneyDateIssue),
                PowerAttorneyFullNum = oldCustomer.FlPowerAttorneyFullNum,
                RegDate = GetNullableDate(oldCustomer.FlRegDate),
                Rnn = oldCustomer.Rtn,
                ShortDocContent = oldCustomer.FlShortDocContent,
                Subscript = oldCustomer.Subscript,
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                Xin = oldCustomer.Xin,
                TypeId = typeId.Value,
                AddressEn = oldCustomer.AddresNameEn,
                AddressKz = oldCustomer.AddresNameKz,
                City = oldCustomer.City,
                Oblast = oldCustomer.Oblast,
                Republic = oldCustomer.Republic,
                IsDeleted = false,
                Region = oldCustomer.Region,
                ShortAddress = oldCustomer.AddresNameRu
            };

            return customer;
        }
    }
}