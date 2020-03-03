using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Search;

namespace Iserv.Niis.Model.Mappings.Search
{
    public class SearchDtoProfile : Profile
    {
        public SearchDtoProfile()
        {
            CreateMap<SearchViewEntity, SearchDto>();

            CreateMap<IQueryable<Domain.Entities.Request.Request>, IQueryable<RequestSearchDto>>()
                .ConvertUsing<RequestSearchDtoConverter>();

            CreateMap<IQueryable<Domain.Entities.ProtectionDoc.ProtectionDoc>, IQueryable<ProtectionDocSearchDto>>()
                .ConvertUsing<ProtectionDocSearchDtoConverter>();

            CreateMap<IQueryable<Document>, IQueryable<DocumentSearchDto>>()
                .ConvertUsing<DocumentSearchDtoConverter>();

            CreateMap<IQueryable<Domain.Entities.Contract.Contract>, IQueryable<ContractSearchDto>>()
                .ConvertUsing<ContractSearchDtoConverter>();


            CreateMap<Domain.Entities.Request.Request, IntellectualPropertySearchDto>()
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.ProtectionDocType.NameRu))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.RequestNum))
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(src => src.ProtectionDocTypeId))
                .ForMember(dest => dest.IsIndustrial,
                    opt => opt.MapFrom(src =>
                        new[]
                        {
                            DicProtectionDocTypeCodes.RequestTypeInventionCode,
                            DicProtectionDocTypeCodes.RequestTypeUsefulModelCode,
                            DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode
                        }.Contains(src.ProtectionDocType.Code)))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.ProtectionDocTypeId))
                .ForMember(dest => dest.Type, opt => opt.UseValue(Owner.Type.Request))
                .ForMember(dest => dest.Addressee, opt => opt.MapFrom(src => src.Addressee));
                //.ForMember(dest => dest.Addressee,
                //    opt => opt.MapFrom(src => src.RequestCustomers.Any(customer => customer.CustomerRole != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                //        ? src.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer).FirstOrDefault()
                //        : null)
                //        );

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, IntellectualPropertySearchDto>()
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.Type.NameRu))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.GosNumber))
                .ForMember(dest => dest.Type, opt => opt.UseValue(Owner.Type.ProtectionDoc))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.Addressee, opt => opt.MapFrom(src => src.Addressee));
            //.ForMember(dest => dest.Addressee,
            //    opt => opt.MapFrom(src => src.ProtectionDocCustomers.Any(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
            //        ? src.ProtectionDocCustomers.Where(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer).FirstOrDefault()
            //        : null)
            //        );

            CreateMap<Domain.Entities.Contract.Contract, IntellectualPropertySearchDto>()
                .ForMember(dest => dest.TypeNameRu, opt => opt.MapFrom(src => src.TypeId.HasValue ? src.Type.NameRu : string.Empty))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.ContractNum))
                .ForMember(dest => dest.ProtectionDocTypeId, opt => opt.MapFrom(src => src.ProtectionDocTypeId))
                .ForMember(dest => dest.Type, opt => opt.UseValue(Owner.Type.Contract))
                .ForMember(dest => dest.Addressee, opt => opt.MapFrom(src => src.Addressee));
            //.ForMember(dest => dest.Addressee,
            //    opt => opt.MapFrom(src => src.ContractCustomers.Any(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
            //        ? src.ContractCustomers.Where(customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer).FirstOrDefault()
            //        : null)
            //        );
        }
        

        private class RequestSearchDtoConverter : ITypeConverter<IQueryable<Domain.Entities.Request.Request>, IQueryable<RequestSearchDto>>
        {
            public IQueryable<RequestSearchDto> Convert(IQueryable<Domain.Entities.Request.Request> source, IQueryable<RequestSearchDto> destination, ResolutionContext context)
            {
                if (context == null)
                    return null;

                var result = source.Select(r => new RequestSearchDto
                {
                    Id = r.Id,
                    StatusId = r.StatusId,
                    StatusNameRu = r.Status != null ? r.Status.NameRu : null,
                    ProtectionDocTypeId = r.ProtectionDocTypeId,
                    ProtectionDocTypeNameRu = r.ProtectionDocType != null ? r.ProtectionDocType.NameRu : null,
                    RequestTypeId = r.RequestTypeId,
                    RequestTypeNameRu = r.RequestType != null ? r.RequestType.NameRu : null,
                    CurrentStageId = r.CurrentWorkflow != null ? r.CurrentWorkflow.CurrentStageId : null,
                    CurrentStageNameRu = r.CurrentWorkflow != null
                        ? (r.CurrentWorkflow.CurrentStage != null ? r.CurrentWorkflow.CurrentStage.NameRu : null)
                        : null,
                    WorkflowDate = r.CurrentWorkflow != null ? r.CurrentWorkflow.DateCreate : (DateTimeOffset?)null,
                    DepartmentNameRu = r.CurrentWorkflow != null
                        ? (r.CurrentWorkflow.CurrentUser != null
                            ? (r.CurrentWorkflow.CurrentUser.Department != null ? r.CurrentWorkflow.CurrentUser.Department.NameRu : null)
                            : null)
                        : null,
                    UserId = r.CurrentWorkflow != null ? r.CurrentWorkflow.CurrentUserId : null,
                    UserNameRu = r.CurrentWorkflow != null
                        ? (r.CurrentWorkflow.CurrentUser != null ? r.CurrentWorkflow.CurrentUser.NameRu : null)
                        : null,
                    RequestNum = r.RequestNum,
                    Barcode = r.Barcode,
                    IncomingNumber = r.IncomingNumber,
                    RequestDate = r.RequestDate,
                    Name = r.NameRu + ' ' + r.NameKz + ' ' + r.NameEn,
                    ReceiveTypeId = r.ReceiveTypeId,
                    ReceiveTypeNameRu = r.ReceiveType != null ? r.ReceiveType.NameRu : null,

                    CustomerXin = r.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Xin).FirstOrDefault()
                        : null,
                    CustomerNameRu = r.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.NameRu).FirstOrDefault()
                        : null,
                    CustomerAddress = r.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault() != null
                            ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault()
                            : r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Address).FirstOrDefault()
                        : null,
                    CustomerCountryId = r.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.CountryId).FirstOrDefault()
                        : null,
                    CustomerCountryNameRu = r.RequestCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? r.RequestCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.Country.NameRu).FirstOrDefault()
                        : null,
                });

                return result;
            }

            /// <summary>
            /// Получить Адресата для переписки.
            /// </summary>
            /// <param name="requestCustomers">Список контрагентов.</param>
            /// <param name="fieldName">Поле для получение объекта.</param>
            /// <param name="subFieldName">Поле из выбранного объекта.</param>
            /// <returns></returns>
            private TResult GetRequestCorrespondence<TResult>(ICollection<RequestCustomer> requestCustomers, string fieldName, string subFieldName = null)
            {
                var correspondence = requestCustomers.FirstOrDefault(rc => rc.CustomerRole != null &&
                                                                           rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

                if (correspondence == null)
                {
                    return default(TResult);
                }

                return GetCustomerCorrespondence<TResult>(correspondence.Customer, fieldName, subFieldName);
            }
        }

        private class ProtectionDocSearchDtoConverter : ITypeConverter<IQueryable<Domain.Entities.ProtectionDoc.ProtectionDoc>, IQueryable<ProtectionDocSearchDto>>
        {
            public IQueryable<ProtectionDocSearchDto> Convert(IQueryable<Domain.Entities.ProtectionDoc.ProtectionDoc> source, IQueryable<ProtectionDocSearchDto> destination, ResolutionContext context)
            {
                if (context == null)
                    return null;

                return source.Select(pd => new ProtectionDocSearchDto
                {
                    Id = pd.Id,
                    statusId = pd.StatusId,
                    statusNameRu = pd.Status != null ? pd.Status.NameRu : null,
                    TypeId = pd.TypeId,
                    TypeNameRu = pd.Type != null ? pd.Type.NameRu : null,
                    CurrentStageId = pd.CurrentWorkflow != null ? pd.CurrentWorkflow.CurrentStageId : null,
                    CurrentStageNameRu = pd.CurrentWorkflow != null
                            ? (pd.CurrentWorkflow.CurrentStage != null ? pd.CurrentWorkflow.CurrentStage.NameRu : null)
                            : null,
                    WorkflowDate = pd.CurrentWorkflow != null ? pd.CurrentWorkflow.DateCreate : (DateTimeOffset?)null,
                    PublicDate = pd.PublicDate,
                    GosNumber = pd.GosNumber,
                    GosDate = pd.GosDate,
                    Name = pd.NameRu + ' ' + pd.NameKz + ' ' + pd.NameEn,
                    ValidDate = pd.ValidDate,

                    CustomerXin = pd.ProtectionDocCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Xin).FirstOrDefault()
                        : null,
                    CustomerNameRu = pd.ProtectionDocCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.NameRu).FirstOrDefault()
                        : null,
                    CustomerAddress = pd.ProtectionDocCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault() != null
                            ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault()
                            : pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Address).FirstOrDefault()
                        : null,
                    CustomerCountryId = pd.ProtectionDocCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.CountryId).FirstOrDefault()
                        : null,
                    CustomerCountryNameRu = pd.ProtectionDocCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? pd.ProtectionDocCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.Country.NameRu).FirstOrDefault()
                        : null,
                });
            }

            /// <summary>
            /// Получить Адресата для переписки.
            /// </summary>
            /// <param name="requestCustomers">Список контрагентов.</param>
            /// <param name="fieldName">Поле для получение объекта.</param>
            /// <param name="subFieldName">Поле из выбранного объекта.</param>
            /// <returns></returns>
            private TResult GetProtectionDocCorrespondence<TResult>(ICollection<ProtectionDocCustomer> protectionDocCustomer, string fieldName, string subFieldName)
            {
                var correspondence = protectionDocCustomer.FirstOrDefault(rc => rc.CustomerRole != null && 
                                                                                rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);

                if (correspondence == null)
                    return default(TResult);

                return GetCustomerCorrespondence<TResult>(correspondence.Customer, fieldName, subFieldName);
            }
        }

        private class DocumentSearchDtoConverter : ITypeConverter<IQueryable<Document>, IQueryable<DocumentSearchDto>>
        {
            public IQueryable<DocumentSearchDto> Convert(IQueryable<Document> source, IQueryable<DocumentSearchDto> destination, ResolutionContext context)
            {
                if (context == null)
                    return null;

                return source.Select(d => new DocumentSearchDto
                    {
                        Id = d.Id,
                        Barcode = d.Barcode,
                        Description = d.Description,
                        DocumentType = d.DocumentType,
                        TypeId = d.TypeId,
                        ReceiveTypeId = d.ReceiveTypeId,
                        ReceiveTypeNameRu = d.ReceiveType != null ? d.ReceiveType.NameRu : null,
                        TypeNameRu = d.Type != null ? d.Type.NameRu : null,
                        DepartmentNameRu = d.CurrentWorkflows.Any()
                            ? string.Join(", ", d.CurrentWorkflows.Where(cwf => cwf.CurrentUser != null && cwf.CurrentUser.Department != null).Select(cwf => cwf.CurrentUser.Department.NameRu))
                            : string.Empty,
                        UserNameRu = d.CurrentWorkflows.Any()
                            ? string.Join(", ", d.CurrentWorkflows.Where(cwf => cwf.CurrentUser != null).Select(cwf => cwf.CurrentUser.NameRu))
                            : string.Empty,
                        DocumentNum = d.DocumentNum,
                        DocumentDate = d.DateCreate,
                        Name = d.NameRu + ' ' + d.NameKz + ' ' + d.NameEn,
                        CustomerXin = d.Addressee != null ? d.Addressee.Xin : null,
                        CustomerNameRu = d.Addressee != null ? d.Addressee.NameRu : null,
                        CustomerAddress = d.Addressee != null ? d.Addressee.Address : null,
                        CustomerCountryId = d.Addressee != null ? d.Addressee.CountryId : null,
                        CustomerCountryNameRu = d.Addressee != null ? (d.Addressee.Country != null ? d.Addressee.Country.NameRu : null) : null,
                        OutgoingNumber = d.OutgoingNumber,
                        SendingDate = d.SendingDate
                    });
            }
        }

        private class ContractSearchDtoConverter : ITypeConverter<IQueryable<Domain.Entities.Contract.Contract>, IQueryable<ContractSearchDto>>
        {
            public IQueryable<ContractSearchDto> Convert(IQueryable<Domain.Entities.Contract.Contract> source, IQueryable<ContractSearchDto> destination, ResolutionContext context)
            {
                if (context == null)
                    return null;

                return source.Select(contract => new ContractSearchDto
                {
                    Id = contract.Id,
                    StatusId = contract.StatusId,
                    StatusNameRu = contract.Status != null ? contract.Status.NameRu : null,
                    ContractTypeId = contract.TypeId,
                    ContractTypeNameRu = contract.Type != null ? contract.Type.NameRu : null,
                    CategoryId = contract.CategoryId,
                    CategoryNameRu = contract.Category != null ? contract.Category.NameRu : null,
                    CurrentStageId = contract.CurrentWorkflow != null
                                ? contract.CurrentWorkflow.CurrentStageId
                                : null,
                    CurrentStageNameRu = contract.CurrentWorkflow != null
                                ? (contract.CurrentWorkflow.CurrentStage != null ? contract.CurrentWorkflow.CurrentStage.NameRu : null)
                                : null,
                    WorkflowDate = contract.CurrentWorkflow != null ? contract.CurrentWorkflow.DateCreate : (DateTimeOffset?)null,
                    DepartmentNameRu = contract.CurrentWorkflow != null
                                ? (contract.CurrentWorkflow.CurrentUser != null
                                    ? (contract.CurrentWorkflow.CurrentUser.Department != null ? contract.CurrentWorkflow.CurrentUser.Department.NameRu : null)
                                    : null)
                                : null,
                    UserId = contract.CurrentWorkflow != null ? contract.CurrentWorkflow.CurrentUserId : null,
                    UserNameRu = contract.CurrentWorkflow != null
                                    ? (contract.CurrentWorkflow.CurrentUser != null ? contract.CurrentWorkflow.CurrentUser.NameRu : null)
                                    : null,
                    DateCreate = contract.DateCreate,
                    ApplicationNum = contract.ApplicationNum,
                    ContractNum = contract.ContractNum,
                    RegDate = contract.RegDate,
                    ProtectionDocTypeId = contract.ProtectionDocTypeId,
                    ProtectionDocTypeNameRu = contract.ProtectionDocType != null ? contract.ProtectionDocType.NameRu : null,
                    Name = contract.NameRu + ' ' + contract.NameKz + ' ' + contract.NameEn,

                    CustomerXin = contract.ContractCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Xin).FirstOrDefault()
                        : null,
                    CustomerNameRu = contract.ContractCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.NameRu).FirstOrDefault()
                        : null,
                    CustomerAddress = contract.ContractCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault() != null
                            ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Address).FirstOrDefault()
                            : contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).Select(d => d.Customer.Address).FirstOrDefault()
                        : null,
                    CustomerCountryId = contract.ContractCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.CountryId).FirstOrDefault()
                        : null,
                    CustomerCountryNameRu = contract.ContractCustomers.Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        ? contract.ContractCustomers.Where(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence && rc.Customer.Country != null).Select(d => d.Customer.Country.NameRu).FirstOrDefault()
                        : null,
                    RegistrationPlace = contract.RegistrationPlace,
                    ValidDate = contract.ValidDate
                });
            }
        }

        /// <summary>
        /// Получить Адресата для переписки из контрагента.
        /// </summary>
        /// <param name="customer">Контрагент.</param>
        /// <param name="fieldName">Поле для получение объекта.</param>
        /// <param name="subFieldName">Поле из выбранного объекта.</param>
        /// <returns></returns>
        private static TResult GetCustomerCorrespondence<TResult>(DicCustomer customer, string fieldName, string subFieldName)
        {
            var property = customer.GetType().GetProperty(fieldName);

            var result = property?.GetValue(customer, null);

            if (result is null)
            {
                return default(TResult);
            }

            var subFieldProperty = result.GetType().GetProperty(subFieldName);

            if (subFieldProperty != null)
            {
                result = subFieldProperty.GetValue(result, null);
            }

            return (TResult)result;
        }
    }
}