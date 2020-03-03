using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Payment;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Model.Mappings
{
    public class PaymentDtoProfile : Profile
    {
        public PaymentDtoProfile()
        {
            CreateMap<Payment, PaymentListDto>();
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.RemainderAmount,
                    opt => opt.MapFrom(p => 
                        p.Amount - ((decimal?)p.PaymentUses.Where(u => !u.IsDeleted).Sum(pu => pu.Amount) ?? 0) - (p.ReturnedAmount ?? 0) - (p.BlockedAmount ?? 0)))
                .ForMember(dest => dest.PaymentUseAmountSum,
                    opt => opt.MapFrom(p => (decimal) (decimal?)p.PaymentUses.Where(u => !u.IsDeleted).Sum(pu => pu.Amount)))
                ;

            CreateMap<PaymentDto, Payment>();

            CreateMap<PaymentUse, PaymentUseDto>()
                .ForMember(dest => dest.AmountWithoutNds,
                    opt => opt.MapFrom(pu => pu.Amount - pu.Amount / (pu.PaymentInvoice.Nds * 100 + 100) *
                                             pu.PaymentInvoice.Nds * 100));

            CreateMap<PaymentUseDto, PaymentUse>();

            CreateMap<PaymentInvoice, PaymentInvoiceDto>()
                .ForMember(dest => dest.TotalAmount,
                    opt => opt.ResolveUsing((source, dest, member, context) =>
                        GetPaymentInvoiceSum(source, GetDeclarantCustomer(context))))
                .ForMember(dest => dest.TotalAmountNds,
                    opt => opt.ResolveUsing((source, dest, member, context) =>
                        GetPaymentInvoiceWithNds(source, GetDeclarantCustomer(context))))
                .ForMember(dest => dest.AmountUseSum,
                    opt => opt.MapFrom(pi => GetPaymentInvoiceUseSum(pi)))
                .ForMember(dest => dest.Remainder,
                    opt => opt.ResolveUsing((source, dest, member, context) =>
                        GetPaymentInvoiceWithNds(source, GetDeclarantCustomer(context)) -
                        GetPaymentInvoiceUseSum(source)))
                .ForMember(dest => dest.TariffPrice,
                    opt => opt.ResolveUsing((source, dest, member, context) =>
                        GetTariffPrice(source, GetDeclarantCustomer(context))))
                .ForMember(dest => dest.CreditUser,
                    opt => opt.MapFrom(src => src.WhoBoundUserId.HasValue ? src.WhoBoundUser.NameRu : string.Empty))
                .ForMember(dest => dest.CreditDate, opt => opt.MapFrom(src => src.DateFact))
                .ForMember(dest => dest.WriteOffUser,
                    opt => opt.MapFrom(src => src.WriteOffUserId.HasValue ? src.WriteOffUser.NameRu : string.Empty))
                .ForMember(dest => dest.WriteOffDate, opt => opt.MapFrom(src => src.DateComplete))
                .ForMember(dest => dest.CreateUser,
                    opt => opt.MapFrom(src => src.CreateUserId.HasValue ? src.CreateUser.NameRu : string.Empty))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.DateCreate))
                .ForMember(dest => dest.OwnerId,
                    opt => opt.MapFrom(src => src.RequestId ?? src.ContractId ?? src.ProtectionDocId));

            CreateMap<PaymentInvoiceDto, PaymentInvoice>()
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dest, member, context) =>
                    {
                        Owner.Type type = (Owner.Type)context.Options.Items["OwnerType"];
                        if (type == Owner.Type.Contract)
                            return src.OwnerId;
                        return null;
                    }))
                .ForMember(dest => dest.RequestId, opt => opt.ResolveUsing((src, dest, member, context) =>
                {
                    Owner.Type type = (Owner.Type)context.Options.Items["OwnerType"];
                    if (type == Owner.Type.Request)
                        return src.OwnerId;
                    return null;
                }))
                .ForMember(dest => dest.ProtectionDocId, opt => opt.ResolveUsing((src, dest, member, context) =>
                {
                    Owner.Type type = (Owner.Type)context.Options.Items["OwnerType"];
                    if (type == Owner.Type.ProtectionDoc)
                        return src.OwnerId;
                    return null;
                }));

            CreateMap<PaymentInvoice, PaymentInvoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicantType, opt => opt.Ignore())
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.CreateUser, opt => opt.Ignore())
                .ForMember(dest => dest.ProtectionDoc, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Tariff, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentUses, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());
        }

        private static List<DicCustomer> GetDeclarantCustomer(ResolutionContext context)
        {
            List<DicCustomer> resultDeclarants = null;
            if (context.Items.ContainsKey("ContractCustomers"))
            {
                var contractCustomers = context.Options.Items["ContractCustomers"] as ICollection<ContractCustomer>;
                if (contractCustomers != null)
                {
                    var declarants = contractCustomers?.Where(c =>
                        new[]
                        {
                            DicCustomerRole.Codes.PatentAttorney, DicCustomerRole.Codes.Confidant,
                            DicCustomerRole.Codes.Correspondence
                        }.Contains(c.CustomerRole?.Code));

                    if (declarants == null || !declarants.Any()) return null;

                    resultDeclarants = declarants.Select(d => d.Customer).ToList();
                }
            }
            if (context.Items.ContainsKey("RequestCustomers"))
            {
                var requestCustomers = context.Options.Items["RequestCustomers"] as ICollection<RequestCustomer>;

                if (requestCustomers != null)
                {
                    var declarants = requestCustomers?.Where(c => c.CustomerRole?.Code == DicCustomerRole.Codes.Declarant);

                    if (declarants == null || !declarants.Any()) return null;

                    resultDeclarants = declarants.Select(d => d.Customer).ToList();
                }
            }
            if (context.Items.ContainsKey("ProtectionDocCustomers"))
            {
                var protectionDocCustomers = context.Options.Items["ProtectionDocCustomers"] as ICollection<ProtectionDocCustomer>;

                if (protectionDocCustomers != null)
                {
                    var declarants = protectionDocCustomers?.Where(c => c.CustomerRole?.Code == DicCustomerRole.Codes.PatentOwner);

                    if (declarants == null || !declarants.Any()) return null;

                    resultDeclarants = declarants.Select(d => d.Customer).ToList();
                }
            }

            return resultDeclarants;
        }

        private static decimal GetPaymentInvoiceUseSum(PaymentInvoice pi)
        {
            return pi.PaymentUses.Any(u => !u.IsDeleted) ? pi.PaymentUses.Where(u => !u.IsDeleted).Sum(u => u.Amount) : 0;
        }

        private static decimal? GetPaymentInvoiceWithNds(PaymentInvoice pi, List<DicCustomer> customers)
        {
            var invoiceSum = GetPaymentInvoiceSum(pi, customers);
            return invoiceSum + invoiceSum * pi.Nds;
        }

        private static decimal? GetPaymentInvoiceSum(PaymentInvoice pi, List<DicCustomer> customers)
        {
            var tariffPrice = GetTariffPrice(pi, customers);
            return tariffPrice * pi.Coefficient * pi.TariffCount +
                   tariffPrice * pi.Coefficient * pi.TariffCount * pi.PenaltyPercent;
        }

        private static decimal? GetTariffPrice(PaymentInvoice pi, List<DicCustomer> customers)
        {
            List<decimal?> invoiceSums = new List<decimal?>();

            // Workaround для миграции. Старые тарифы
            if (customers == null || customers.Count == 0)
            {
                return pi.Tariff?.Price ?? 0;
            }

            foreach (var customer in customers)
            {
                // Workaround для миграции. Старые тарифы
                if (pi.Tariff.Price != null)
                    invoiceSums.Add(pi.Tariff.Price);

                //не зависит от типа заявителя
                //if (new[] { DicProtectionDocType.Codes.Trademark, DicProtectionDocType.Codes.InternationalTrademark }
                //    .Contains(GetOwnerProtectionDocType(pi)))
                //{
                //    invoiceSums.Add(pi.Tariff.Price);
                //    continue;
                //}

                //var isBeneficiary = customer?.IsBeneficiary ?? false;
                //if (isBeneficiary && customer?.BeneficiaryType?.Code != "SMB") // Субъекты малого и среднего бизнеса
                //{
                //    invoiceSums.Add(pi.Tariff.Price);
                //    continue;
                //}

                //if (customer?.BeneficiaryType?.Code == "SMB")
                //{
                //    invoiceSums.Add(pi.Tariff.Price);
                //    continue;
                //}

                //switch (customer?.Type?.Code)
                //{
                //    case DicCustomerTypeCodes.Physical:
                //        invoiceSums.Add(pi.Tariff.PriceFl);
                //        break;
                //    case DicCustomerTypeCodes.Juridical:
                //        invoiceSums.Add(pi.Tariff.PriceUl);
                //        break;
                //    case DicCustomerTypeCodes.SoloEntrepreneur:
                //    case DicCustomerTypeCodes.LimitedPartnership:
                //        invoiceSums.Add(pi.Tariff.PriceBusiness);
                //        break;
                //    default: return null;
                //}
            }

            return invoiceSums.Max();
        }

        private static string GetOwnerProtectionDocType(PaymentInvoice pi)
        {
            if (pi.RequestId != null)
            {
                return pi.Request.ProtectionDocType.Code;
            }

            if (pi.ProtectionDocId != null)
            {
                return pi.ProtectionDoc.Type.Code;
            }

            if (pi.ContractId != null)
            {
                return pi.Contract.ProtectionDocType.Code;
            }

            return string.Empty;
        }
    }
}