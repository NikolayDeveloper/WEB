using AutoMapper;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.ExpertSearch;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.Model.Mappings.ExpertSearch
{
    public class ExpertSearchDtoProfile : Profile
    {
        public ExpertSearchDtoProfile()
        {
            //Номальный отрефакториный код
            NormolizeMap();

            CreateMap<Domain.Entities.Request.Request, TrademarkDto>()
                .ForMember(dest => dest.PreviewImage,
                    opt => opt.MapFrom(src =>
                        src.PreviewImage != null
                            ? $"data:image/png;base64,{Convert.ToBase64String(src.PreviewImage)}"
                            : null))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Request))
                .ForMember(dest => dest.ToPriorityDate, opt => opt.MapFrom(src =>
                    src.EarlyRegs.Any()
                        ? src.EarlyRegs.OrderBy(er => er.PriorityDate).First().PriorityDate
                        : src.RequestDate))
                .ForMember(dest => dest.PriorityDates,
                    opt => opt.MapFrom(src => src.EarlyRegs.Select(er => er.PriorityDate)))
                .ForMember(dest => dest.Icgs,
                    opt => opt.MapFrom(src => src.ICGSRequests.Select(i => i.Icgs.NameRu).Join(", ")))
                .ForMember(dest => dest.Icfems,
                    opt => opt.MapFrom(src => src.Icfems.Select(i => i.DicIcfem.NameRu).Join(", ")));

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, TrademarkDto>()
                .ForMember(dest => dest.PreviewImage,
                    opt => opt.MapFrom(src =>
                        src.Request != null
                            ? (src.Request.PreviewImage != null
                                ? $"data:image/png;base64,{Convert.ToBase64String(src.Request.PreviewImage)}"
                                : null)
                            : null))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.ProtectionDoc))
                .ForMember(dest => dest.ToPriorityDate, opt => opt.MapFrom(src =>
                    src.EarlyRegs.Any()
                        ? src.EarlyRegs.OrderBy(er => er.PriorityDate).First().PriorityDate
                        : (src.Request != null ? src.Request.RequestDate : (DateTimeOffset?) null)))
                .ForMember(dest => dest.PriorityDates,
                    opt => opt.MapFrom(src => src.EarlyRegs.Select(er => er.PriorityDate)))
                .ForMember(dest => dest.Icgs,
                    opt => opt.MapFrom(src => src.IcgsProtectionDocs.Select(i => i.Icgs.NameRu).Join(", ")))
                .ForMember(dest => dest.Icfems,
                    opt => opt.MapFrom(src => src.Icfems.Select(i => i.DicIcfem.NameRu).Join(", ")));
            
            CreateMap<Domain.Entities.Request.Request, BaseExpertSearchDto>()
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Request))
                .ForMember(dest => dest.ToPriorityDate, opt => opt.MapFrom(src =>
                    src.EarlyRegs.Any()
                        ? src.EarlyRegs.OrderBy(er => er.PriorityDate).First().PriorityDate
                        : src.RequestDate))
                .ForMember(dest => dest.PriorityDates,
                    opt => opt.MapFrom(src => src.EarlyRegs.Select(er => er.PriorityDate)));

            CreateMap<Domain.Entities.ProtectionDoc.ProtectionDoc, BaseExpertSearchDto>()
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.ProtectionDoc))
                .ForMember(dest => dest.ToPriorityDate, opt => opt.MapFrom(src =>
                    src.EarlyRegs.Any()
                        ? src.EarlyRegs.OrderBy(er => er.PriorityDate).First().PriorityDate
                        : (src.Request != null ? src.Request.RequestDate : (DateTimeOffset?) null)))
                .ForMember(dest => dest.PriorityDates,
                    opt => opt.MapFrom(src => src.EarlyRegs.Select(er => er.PriorityDate)));


            CreateMap<ExpertSearchViewEntity, InventionSearchDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(
                    source => source.Request != null
                        ? source.Request.NameRu ?? (source.Request.NameKz ?? (source.Request.NameEn ?? string.Empty))
                        : source.ProtectionDoc != null
                            ? source.ProtectionDoc.NameRu ??
                              (source.ProtectionDoc.NameKz ?? (source.ProtectionDoc.NameEn ?? string.Empty))
                            : string.Empty))
                .ForMember(destination => destination.RegNumber, options => options.MapFrom(
                    source => source.Request != null ? source.Request.RequestNum ?? string.Empty :
                        source.ProtectionDoc != null ? source.ProtectionDoc.RegNumber ?? string.Empty : string.Empty))
                .ForMember(destination => destination.RegDate, opt => opt.MapFrom(
                    source => source.Request != null ? source.Request.RequestDate ??
                                                       (source.ProtectionDoc != null
                                                           ? source.ProtectionDoc.RegDate
                                                           : null)
                        : source.ProtectionDoc != null ? source.ProtectionDoc.RegDate : null))
                .ForMember(destination => destination.Status, opt => opt.MapFrom(
                    source => source.Request != null ? (source.Request.Status != null ? source.Request.Status.NameRu :
                            source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : null) :
                        source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : null))
                .ForMember(dest => dest.GosNumber,
                    options => options.MapFrom(source =>
                        source.ProtectionDoc != null ? source.ProtectionDoc.GosNumber : null))
                .ForMember(destination => destination.PublishDate, options => options.MapFrom(
                    source => source.Request != null ? source.Request.PublishDate ??
                                                       (source.ProtectionDoc != null
                                                           ? source.ProtectionDoc.PublishDate
                                                           : null)
                        : source.ProtectionDoc != null ? source.ProtectionDoc.PublishDate : null))
                .ForMember(destination => destination.Declarant, option => option.MapFrom(
                    source => string.Join(";", source.Request != null ? source.Request.RequestCustomers
                            .Where(customer => customer.CustomerRole != null &&
                                               customer.Customer != null &&
                                               customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer =>
                                customer.Customer.NameRu ??
                                customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty)
                        : source.ProtectionDoc != null ? source.ProtectionDoc.ProtectionDocCustomers
                            .Where(customer => customer.CustomerRole != null &&
                                               customer.Customer != null &&
                                               customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer =>
                                customer.Customer.NameRu ??
                                customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty)
                        : new string[] { })
                ))
                .ForMember(destination => destination.Ipc, options => options.MapFrom(
                    source => string.Join(";", source.Request != null ? source.Request.IPCRequests
                            .Where(request => request.Ipc != null)
                            .Select(request => request.Ipc.Code ?? string.Empty)
                        : source.ProtectionDoc != null ? source.ProtectionDoc.IpcProtectionDocs
                            .Where(protectionDoc => protectionDoc.Ipc != null)
                            .Select(protectionDoc => protectionDoc.Ipc.Code ?? string.Empty)
                        : new string[] { })
                ))
                .ForMember(destination => destination.Referat, options => options.MapFrom(
                    source => source.Request != null ? source.Request.Referat ?? string.Empty :
                        source.ProtectionDoc != null ? source.ProtectionDoc.Referat : string.Empty));
        }

        private void NormolizeMap()
        {
            BaseExpertSearchDtoMap();
            TrademarkSearchDtoMap();
            IndustrialDesignDtoMap();
            UsefulModelDtoMap();
            InventionDtoMap();
        }

        private void TrademarkSearchDtoMap()
        {
            CreateMap<ExpertSearchViewEntity, TrademarkSearchDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType))

                //Изображение
                .ForMember(dest => dest.PreviewImage, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PreviewImage != null
                        ? $"data:image/png;base64,{Convert.ToBase64String(source.Request.PreviewImage)}"
                        : null
                    : source.ProtectionDoc.PreviewImage != null
                        ? $"data:image/png;base64,{Convert.ToBase64String(source.ProtectionDoc.PreviewImage)}"
                        : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.IncomingNumber
                    : source.ProtectionDoc.RegNumber))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : source.ProtectionDoc.RegDate))
                //№ патента
                .ForMember(dest => dest.GosNumber, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Empty
                    : source.ProtectionDoc.GosNumber))
                //Заявитель
                .ForMember(dest => dest.DeclarantName, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Владелец
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Owner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Owner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Дискламация
                .ForMember(dest => dest.Disclamation, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.DisclaimerRu ?? source.Request.DisclaimerKz ?? source.Request.DisclaimerEn
                    : source.ProtectionDoc.DisclaimerRu ?? source.ProtectionDoc.DisclaimerKz))
                //МКТУ
                .ForMember(dest => dest.Icgs, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ICGSRequests.Where(request => request.Icgs != null).Select(request => request.Icgs.Code ?? string.Empty)
                    : source.ProtectionDoc.IcgsProtectionDocs.Where(protectionDoc => protectionDoc.Icgs != null).Select(protectionDoc => protectionDoc.Icgs.Code ?? string.Empty)))
                //Цвета
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.ColorTzs.Where(request => request.ColorTz != null).Select(request => request.ColorTz.Code ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ColorTzs.Where(protectionDoc => protectionDoc.ColorTz != null).Select(protectionDoc => protectionDoc.ColorTz.Code ?? string.Empty))))
                //МКИЭТЗ
                .ForMember(dest => dest.Icfem, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.Icfems.Where(request => request.DicIcfem != null).Select(request => request.DicIcfem.Code ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.Icfems.Where(protectionDoc => protectionDoc.DicIcfem != null).Select(protectionDoc => protectionDoc.DicIcfem.Code ?? string.Empty))))

                //Штрихкод
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? source.Request.Barcode
                        : source.ProtectionDoc.Barcode))
                //Тип заявки
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocTypeId
                    : source.ProtectionDoc.TypeId))
                .ForMember(dest => dest.RequestTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocType != null ? source.Request.ProtectionDocType.NameRu : string.Empty
                    : source.ProtectionDoc.Type != null ? source.ProtectionDoc.Type.NameRu : string.Empty))
                //Статус
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.StatusId
                    : source.ProtectionDoc.StatusId))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.Code : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.NameRu : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : string.Empty))
                //Дата ОД
                .ForMember(dest => dest.GosDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.ProtectionDoc
                    ? source.ProtectionDoc.GosDate
                    : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestNum
                    : null))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : null))
                //Наименование
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на рус
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на каз
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameKz
                    : source.ProtectionDoc.NameKz))
                //Наименование на англ
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameEn
                    : source.ProtectionDoc.NameEn))
                //Заявитель
                .ForMember(dest => dest.Declarant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Патентный поверенный
                .ForMember(dest => dest.PatentAttorney, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Адрес для переписки
                .ForMember(dest => dest.AddressForCorrespondence, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Доверенное лицо
                .ForMember(dest => dest.Confidant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Тип подачи заявк
                .ForMember(dest => dest.ReceiveTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveTypeId
                    : null))
                .ForMember(dest => dest.ReceiveTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveType != null ? source.Request.ReceiveType.NameRu : string.Empty
                    : string.Empty))
                //Номер бюллетеня
                .ForMember(dest => dest.NumberBulletin, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NumberBulletin
                    : string.Empty))
                //Дата публикации
                .ForMember(dest => dest.PublicDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PublicDate
                    : null))
                //Срок действия
                .ForMember(dest => dest.ValidDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.ValidDate))
                //Приоритетные данные
                .ForMember(dest => dest.PriorityData, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                ))
                .ForMember(dest => dest.PriorityRegCountryNames, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegCountry.NameRu))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegCountry.NameRu))
                ))
                .ForMember(dest => dest.PriorityRegNumbers, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegNumber))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegNumber))
                ))
                .ForMember(dest => dest.PriorityDates, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.EarlyRegs.Select(d => d.PriorityDate)
                    : source.ProtectionDoc.EarlyRegs.Select(d => d.PriorityDate)
                ))
                ;
        }

        private void InventionDtoMap()
        {
            CreateMap<ExpertSearchViewEntity, InventionDto>()
                //№ патента
                .ForMember(dest => dest.GosNumber, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Empty
                    : source.ProtectionDoc.GosNumber))
                //Патентообладатель
                .ForMember(dest => dest.PatentOwner, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //МПК
                .ForMember(dest => dest.Ipcs, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.IPCRequests.Where(request => request.Ipc != null).Select(request => request.Ipc.Code ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.IpcProtectionDocs.Where(protectionDoc => protectionDoc.Ipc != null).Select(protectionDoc => protectionDoc.Ipc.Code ?? string.Empty))))
                //Автор
                .ForMember(dest => dest.Author, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Дата досрочного прекращения
                .ForMember(dest => dest.EarlyTerminationDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.EarlyTerminationDate))
                //Дата вхождения в национальную фазу
                .ForMember(dest => dest.TransferDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.TransferDate
                    : source.ProtectionDoc.TransferDate))
                //Реферат
                .ForMember(dest => dest.Referat, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Referat
                    : source.ProtectionDoc.Referat))
                //Примечание
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Description
                    : source.ProtectionDoc.Description))
                //Формула
                //.ForMember(dest => dest.Formula, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                //    ? source.Request.Formula
                //    : source.ProtectionDoc.Formula))

                //Base
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType))
                //Штрихкод
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? source.Request.Barcode
                        : source.ProtectionDoc.Barcode))
                //Тип заявки
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocTypeId
                    : source.ProtectionDoc.TypeId))
                .ForMember(dest => dest.RequestTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocType != null ? source.Request.ProtectionDocType.NameRu : string.Empty
                    : source.ProtectionDoc.Type != null ? source.ProtectionDoc.Type.NameRu : string.Empty))
                //Статус
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.StatusId
                    : source.ProtectionDoc.StatusId))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.Code : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.NameRu : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : string.Empty))
                //Дата ОД
                .ForMember(dest => dest.GosDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.ProtectionDoc
                    ? source.ProtectionDoc.GosDate
                    : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestNum
                    : null))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : null))
                //Наименование
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на рус
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на каз
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameKz
                    : source.ProtectionDoc.NameKz))
                //Наименование на англ
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameEn
                    : source.ProtectionDoc.NameEn))
                //Заявитель
                .ForMember(dest => dest.Declarant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Патентный поверенный
                .ForMember(dest => dest.PatentAttorney, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Адрес для переписки
                .ForMember(dest => dest.AddressForCorrespondence, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Доверенное лицо
                .ForMember(dest => dest.Confidant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Тип подачи заявк
                .ForMember(dest => dest.ReceiveTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveTypeId
                    : null))
                .ForMember(dest => dest.ReceiveTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveType != null ? source.Request.ReceiveType.NameRu : string.Empty
                    : string.Empty))
                //Номер бюллетеня
                .ForMember(dest => dest.NumberBulletin, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NumberBulletin
                    : string.Empty))
                //Дата публикации
                .ForMember(dest => dest.PublicDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PublicDate
                    : null))
                //Срок действия
                .ForMember(dest => dest.ValidDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.ValidDate))
                //Приоритетные данные
                .ForMember(dest => dest.PriorityData, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                ))
                .ForMember(dest => dest.PriorityRegCountryNames, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegCountry.NameRu))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegCountry.NameRu))
                ))
                .ForMember(dest => dest.PriorityRegNumbers, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegNumber))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegNumber))
                ))
                .ForMember(dest => dest.PriorityDates, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.EarlyRegs.Select(d => d.PriorityDate)
                    : source.ProtectionDoc.EarlyRegs.Select(d => d.PriorityDate)
                ))
                ;
        }

        private void UsefulModelDtoMap()
        {
            CreateMap<ExpertSearchViewEntity, UsefulModelDto>()
                //№ патента
                .ForMember(dest => dest.GosNumber, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Empty
                    : source.ProtectionDoc.GosNumber))
                //Патентообладатель
                .ForMember(dest => dest.PatentOwner, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //МПК
                .ForMember(dest => dest.Ipcs, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.IPCRequests.Where(request => request.Ipc != null).Select(request => request.Ipc.Code ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.IpcProtectionDocs.Where(protectionDoc => protectionDoc.Ipc != null).Select(protectionDoc => protectionDoc.Ipc.Code ?? string.Empty))))
                //Автор
                .ForMember(dest => dest.Author, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Дата досрочного прекращения
                .ForMember(dest => dest.EarlyTerminationDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.EarlyTerminationDate))
                //Дата вхождения в национальную фазу
                .ForMember(dest => dest.TransferDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.TransferDate
                    : source.ProtectionDoc.TransferDate))
                //Реферат
                .ForMember(dest => dest.Referat, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Referat
                    : source.ProtectionDoc.Referat))
                //Примечание
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Description
                    : source.ProtectionDoc.Description))
                //Формула
                //.ForMember(dest => dest.Formula, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                //    ? source.Request.Formula
                //    : source.ProtectionDoc.Formula))

                //Base
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType))
                //Штрихкод
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? source.Request.Barcode
                        : source.ProtectionDoc.Barcode))
                //Тип заявки
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocTypeId
                    : source.ProtectionDoc.TypeId))
                .ForMember(dest => dest.RequestTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocType != null ? source.Request.ProtectionDocType.NameRu : string.Empty
                    : source.ProtectionDoc.Type != null ? source.ProtectionDoc.Type.NameRu : string.Empty))
                //Статус
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.StatusId
                    : source.ProtectionDoc.StatusId))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.Code : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.NameRu : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : string.Empty))
                //Дата ОД
                .ForMember(dest => dest.GosDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.ProtectionDoc
                    ? source.ProtectionDoc.GosDate
                    : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestNum
                    : null))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : null))
                //Наименование
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на рус
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на каз
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameKz
                    : source.ProtectionDoc.NameKz))
                //Наименование на англ
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameEn
                    : source.ProtectionDoc.NameEn))
                //Заявитель
                .ForMember(dest => dest.Declarant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Патентный поверенный
                .ForMember(dest => dest.PatentAttorney, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Адрес для переписки
                .ForMember(dest => dest.AddressForCorrespondence, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Доверенное лицо
                .ForMember(dest => dest.Confidant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Тип подачи заявк
                .ForMember(dest => dest.ReceiveTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveTypeId
                    : null))
                .ForMember(dest => dest.ReceiveTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveType != null ? source.Request.ReceiveType.NameRu : string.Empty
                    : string.Empty))
                //Номер бюллетеня
                .ForMember(dest => dest.NumberBulletin, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NumberBulletin
                    : string.Empty))
                //Дата публикации
                .ForMember(dest => dest.PublicDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PublicDate
                    : null))
                //Срок действия
                .ForMember(dest => dest.ValidDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.ValidDate))
                //Приоритетные данные
                .ForMember(dest => dest.PriorityData, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                ))
                .ForMember(dest => dest.PriorityRegCountryNames, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegCountry.NameRu))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegCountry.NameRu))
                ))
                .ForMember(dest => dest.PriorityRegNumbers, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegNumber))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegNumber))
                ))
                .ForMember(dest => dest.PriorityDates, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.EarlyRegs.Select(d => d.PriorityDate)
                    : source.ProtectionDoc.EarlyRegs.Select(d => d.PriorityDate)
                ))
                ;
        }

        private void IndustrialDesignDtoMap()
        {
            CreateMap<ExpertSearchViewEntity, IndustrialDesignDto>()
                //Изображение
                .ForMember(dest => dest.PreviewImage, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request 
                        ? source.Request.PreviewImage != null 
                            ? $"data:image/png;base64,{Convert.ToBase64String(source.Request.PreviewImage)}"
                            : null
                        : source.ProtectionDoc.PreviewImage != null
                            ? $"data:image/png;base64,{Convert.ToBase64String(source.ProtectionDoc.PreviewImage)}"
                            : null))
                //№ патента
                .ForMember(dest => dest.GosNumber, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? string.Empty
                        : source.ProtectionDoc.GosNumber))
                //Патентообладатель
                .ForMember(dest => dest.PatentOwner, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //МКПО
                .ForMember(dest => dest.Icis, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.ICISRequests.Where(request => request.Icis != null).Select(request => request.Icis.Code ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.IcisProtectionDocs.Where(protectionDoc => protectionDoc.Icis != null).Select(protectionDoc => protectionDoc.Icis.Code ?? string.Empty))))
                //Автор
                .ForMember(dest => dest.Author, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Author)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Существенные признаки
                .ForMember(dest => dest.Referat, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Referat
                    : source.ProtectionDoc.Referat))
                //Дата досрочного прекращения
                .ForMember(dest => dest.EarlyTerminationDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.EarlyTerminationDate))

                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType))
                //Штрихкод
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? source.Request.Barcode
                        : source.ProtectionDoc.Barcode))
                //Тип заявки
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocTypeId
                    : source.ProtectionDoc.TypeId))
                .ForMember(dest => dest.RequestTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocType != null ? source.Request.ProtectionDocType.NameRu : string.Empty
                    : source.ProtectionDoc.Type != null ? source.ProtectionDoc.Type.NameRu : string.Empty))
                //Статус
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.StatusId
                    : source.ProtectionDoc.StatusId))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.Code : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.NameRu : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : string.Empty))
                //Дата ОД
                .ForMember(dest => dest.GosDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.ProtectionDoc
                    ? source.ProtectionDoc.GosDate
                    : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestNum
                    : null))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : null))
                //Наименование
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на рус
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на каз
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameKz
                    : source.ProtectionDoc.NameKz))
                //Наименование на англ
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameEn
                    : source.ProtectionDoc.NameEn))
                //Заявитель
                .ForMember(dest => dest.Declarant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Патентный поверенный
                .ForMember(dest => dest.PatentAttorney, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Адрес для переписки
                .ForMember(dest => dest.AddressForCorrespondence, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Доверенное лицо
                .ForMember(dest => dest.Confidant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Тип подачи заявк
                .ForMember(dest => dest.ReceiveTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveTypeId
                    : null))
                .ForMember(dest => dest.ReceiveTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveType != null ? source.Request.ReceiveType.NameRu : string.Empty
                    : string.Empty))
                //Номер бюллетеня
                .ForMember(dest => dest.NumberBulletin, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NumberBulletin
                    : string.Empty))
                //Дата публикации
                .ForMember(dest => dest.PublicDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PublicDate
                    : null))
                //Срок действия
                .ForMember(dest => dest.ValidDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.ValidDate))
                //Приоритетные данные
                .ForMember(dest => dest.PriorityData, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                ))
                .ForMember(dest => dest.PriorityRegCountryNames, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegCountry.NameRu))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegCountry.NameRu))
                ))
                .ForMember(dest => dest.PriorityRegNumbers, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegNumber))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegNumber))
                ))
                .ForMember(dest => dest.PriorityDates, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.EarlyRegs.Select(d => d.PriorityDate)
                    : source.ProtectionDoc.EarlyRegs.Select(d => d.PriorityDate)
                ))
                ;
        }

        private void BaseExpertSearchDtoMap()
        {
            CreateMap<ExpertSearchViewEntity, BaseExpertSearchDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType))
                //Штрихкод
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                        ? source.Request.Barcode
                        : source.ProtectionDoc.Barcode))
                //Тип заявки
                .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocTypeId
                    : source.ProtectionDoc.TypeId))
                .ForMember(dest => dest.RequestTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ProtectionDocType != null ? source.Request.ProtectionDocType.NameRu : string.Empty
                    : source.ProtectionDoc.Type != null ? source.ProtectionDoc.Type.NameRu : string.Empty))
                //Статус
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.StatusId
                    : source.ProtectionDoc.StatusId))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.Code : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.Code : string.Empty))
                .ForMember(dest => dest.StatusNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.Status != null ? source.Request.Status.NameRu : string.Empty
                    : source.ProtectionDoc.Status != null ? source.ProtectionDoc.Status.NameRu : string.Empty))
                //Дата ОД
                .ForMember(dest => dest.GosDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.ProtectionDoc
                    ? source.ProtectionDoc.GosDate
                    : null))
                //Рег. номер заявки
                .ForMember(dest => dest.RequestNum, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestNum
                    : null))
                //Дата подачи заявки
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.RequestDate
                    : null))
                //Наименование
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на рус
                .ForMember(dest => dest.NameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameRu
                    : source.ProtectionDoc.NameRu))
                //Наименование на каз
                .ForMember(dest => dest.NameKz, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameKz
                    : source.ProtectionDoc.NameKz))
                //Наименование на англ
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NameEn
                    : source.ProtectionDoc.NameEn))
                //Заявитель
                .ForMember(dest => dest.Declarant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                            .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Патентный поверенный
                .ForMember(dest => dest.PatentAttorney, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Адрес для переписки
                .ForMember(dest => dest.AddressForCorrespondence, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Доверенное лицо
                .ForMember(dest => dest.Confidant, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(";", source.Request.RequestCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))
                    : string.Join(";", source.ProtectionDoc.ProtectionDocCustomers.Where(customer => customer.CustomerRole != null && customer.Customer != null && customer.CustomerRole.Code == DicCustomerRoleCodes.Confidant)
                        .Select(customer => customer.Customer.NameRu ?? customer.Customer.NameKz ?? customer.Customer.NameEn ?? string.Empty))))
                //Тип подачи заявк
                .ForMember(dest => dest.ReceiveTypeId, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveTypeId
                    : null))
                .ForMember(dest => dest.ReceiveTypeNameRu, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.ReceiveType != null ? source.Request.ReceiveType.NameRu : string.Empty
                    : string.Empty))
                //Номер бюллетеня
                .ForMember(dest => dest.NumberBulletin, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.NumberBulletin
                    : string.Empty))
                //Дата публикации
                .ForMember(dest => dest.PublicDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.PublicDate
                    : null))
                //Срок действия
                .ForMember(dest => dest.ValidDate, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? null
                    : source.ProtectionDoc.ValidDate))
                //Приоритетные данные
                .ForMember(dest => dest.PriorityData, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => $"{d.RegNumber} {d.PriorityDate.ToString()} {d.RegCountry.Code}").ToList())
                ))
                .ForMember(dest => dest.PriorityRegCountryNames, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegCountry.NameRu))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegCountry.NameRu))
                ))
                .ForMember(dest => dest.PriorityRegNumbers, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? string.Join(", ", source.Request.EarlyRegs.Select(d => d.RegNumber))
                    : string.Join(", ", source.ProtectionDoc.EarlyRegs.Select(d => d.RegNumber))
                ))
                .ForMember(dest => dest.PriorityDates, opt => opt.MapFrom(source => source.OwnerType == Owner.Type.Request
                    ? source.Request.EarlyRegs.Select(d => d.PriorityDate)
                    : source.ProtectionDoc.EarlyRegs.Select(d => d.PriorityDate)
                ))
                ;
        }
    }
}