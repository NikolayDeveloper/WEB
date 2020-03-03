using System;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetPayTariffService : IGetPayTariffService
    {
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly NiisWebContext _niisWebContext;

        public GetPayTariffService(NiisWebContext niisWebContext, DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisWebContext;
            _dictionaryHelper = dictionaryHelper;
        }

        public DateTime? GetValidity(
            string protectionDocType,
            DateTime? flValidityExtention)
        {
            return (protectionDocType == DicProtectionDocType.Codes.Trademark ||
                    protectionDocType == DicProtectionDocType.Codes.PlaceOfOrigin) && flValidityExtention != null
                ? flValidityExtention
                : null;
        }

        public bool GetDiscontinued(
            int? protectionDocState,
            string protectionDocType,
            DateTime? flNotValidity,
            DateTime? flValidityExtention)
        {
            var discontinued = protectionDocState != _dictionaryHelper.GetDictionaryIdByCode(nameof(DicProtectionDocStatus), DicProtectionDocStatusCodes.D);

            if (discontinued == false && flNotValidity < DateTime.Now)
            {
                if ((protectionDocType == DicProtectionDocType.Codes.Trademark ||
                     protectionDocType == DicProtectionDocType.Codes.PlaceOfOrigin)
                    && flValidityExtention != null)
                {
                    flNotValidity = flValidityExtention.Value.AddMonths(6);
                    if (flNotValidity < DateTime.Now)
                    {
                        discontinued = true;
                    }
                }
                else
                {
                    discontinued = true;
                }
            }

            return discontinued;
        }

        public ProtectionDoc GetProtectionDoc(
            string protectionDocType,
            string protectionDocNumber,
            GetPayTarifResult result)
        {
            switch (protectionDocType)
            {
                case DicProtectionDocType.Codes.Trademark:
                case DicProtectionDocType.Codes.PlaceOfOrigin:
                    result.Result = PayTarifResult.Error;
                    result.Message = "Поддержание в силе товарного знака не предусмотрено законодательстом";
                    break;
                default:
                    return _niisWebContext.ProtectionDocs
                        .Include(i => i.Status)
                        .Include(i => i.Type)
                        .OrderBy(p => p.Id)
                        .FirstOrDefault(p =>
                            p.Type.Code == protectionDocType
                            && p.GosNumber == protectionDocNumber
                            && new[]
                            {
                                DicProtectionDocStatusCodes.D, DicProtectionDocStatusCodes._03_38,
                                DicProtectionDocStatusCodes._03_39
                            }.Contains(p.Status.Code));
            }

            return null;
        }

        public void GetTariffId(
            string protectionDocType,
            int? protectionDocId,
            GetPayTarifResult result)
        {
            switch (protectionDocType)
            {
                case DicProtectionDocType.Codes.Trademark:
                case DicProtectionDocType.Codes.PlaceOfOrigin:
                    var flCount = _niisWebContext.ICGSProtectionDocs
                                      .Count(p => p.ProtectionDocId == protectionDocId) - 3;
                    result.Message = IntegrationNiisRefTariff.TradeMarkNameOfOriginTariffId.ToString();

                    while (flCount > 3)
                    {
                        result.Message =
                            $"{result.Message};{IntegrationNiisRefTariff.TradeMarkNameOfOriginOverThreeClassTariffId}";
                        flCount--;
                    }

                    break;
                default:
                    var tariffs = GetTariffList(protectionDocType);
                    var previousNiisTariff = tariffs.FirstOrDefault();
                    var currentNiisTariff = tariffs.FirstOrDefault();
                    var repeat = 0;
                    var b = 1;

                    while (b == 1)
                    {
                        var paymentInvoices = GetPaymentInvoiceList(protectionDocType, currentNiisTariff);

                        if (paymentInvoices.Any())
                        {
                            result.Result = PayTarifResult.Error;
                            result.Message = "Дальнейшее поддержание невозможно";
                            break;
                        }

                        foreach (var niisTariff in tariffs)
                        {
                            previousNiisTariff = currentNiisTariff;
                            currentNiisTariff = niisTariff;
                            paymentInvoices = GetPaymentInvoiceList(protectionDocType, currentNiisTariff);
                            if (paymentInvoices.Any())
                            {
                                result.Message = previousNiisTariff?.FirstOrDefault()?.NiisTariffId.ToString();
                                break;
                            }
                        }


                        if (repeat == 1)
                        {
                            result.Message = currentNiisTariff?.FirstOrDefault()?.NiisTariffId.ToString();
                            b = 0;
                        }
                        else
                        {
                            switch (protectionDocType)
                            {
                                case DicProtectionDocType.Codes.Invention:
                                case DicProtectionDocType.Codes.SelectiveAchievement:
                                    repeat = 1;
                                    GetTariffList(protectionDocType);
                                    break;
                                case DicProtectionDocType.Codes.UsefulModel:
                                    repeat = 1;
                                    protectionDocType = DicProtectionDocType.Codes.IndustrialModel;
                                    GetTariffList(protectionDocType);
                                    break;
                                case DicProtectionDocType.Codes.IndustrialModel:
                                    repeat = 1;
                                    GetTariffList(protectionDocType);
                                    break;
                                default:
                                    result.Message = currentNiisTariff?.FirstOrDefault()?.NiisTariffId.ToString();
                                    b = 0;
                                    break;
                            }
                        }
                    }

                    break;
            }
        }

        #region Private methods

        private IQueryable<PaymentInvoice> GetPaymentInvoiceList(string protectionDocType,
            IGrouping<int?, DicTariff> currentNiisTariff)
        {
            return _niisWebContext.PaymentInvoices
                .Include(p => p.ProtectionDoc).ThenInclude(p => p.Type)
                .Where(p =>
                    currentNiisTariff.Select(t => t.Id).Contains(p.TariffId)
                    && p.ProtectionDoc.Type.Code == protectionDocType);
        }

        private IQueryable<IGrouping<int?, DicTariff>> GetTariffList(string protectionDocType)
        {
            return _niisWebContext.DicTariffs
                .Include(p => p.TariffProtectionDocTypes).ThenInclude(d => d.ProtectionDocType)
                .OrderByDescending(d => d.MaintenanceYears)
                .Where(d => d.TariffProtectionDocTypes.Any(e => e.ProtectionDocType.Code == protectionDocType)
                    && d.MaintenanceYears != null)
                .GroupBy(d => d.MaintenanceYears);
        }

        #endregion
    }
}