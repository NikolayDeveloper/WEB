using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic._1CIntegration;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class ChargePaymentsByDocumentsOutgoingDateHandler: BaseHandler
    {
        private static readonly List<DocumentTypeTariffRelation> DocumentTypesTariffConfig =
            new List<DocumentTypeTariffRelation>
            {
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.CollectiveTmFormalExpertizeDigital,
                        DicTariffCodes.CollectiveTmFormalExpertizePaper,
                        DicTariffCodes.TmNmptFormalExpertizeDigital,
                        DicTariffCodes.TmNmptFormalExpertizePaper,
                        DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesPaper
                    },
                    DocumentTypeCodes = new List<string>
                    {                        
                        DicDocumentTypeCodes.TZPRED1,
                        //DicDocumentTypeCodes.OUT_Uv_pred_rassm_reg_TZ_v1_19,
                        //DicDocumentTypeCodes.OUT_Uv_pred_prekr_del_otv_zap_v1_19,
                        DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTerminationFormal
                    }
                },
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.TmNmptFullExpertizeDigital,
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.TmNmptFullExpertizePaper,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizePaper
                    },
                    DocumentTypeCodes = new List<string>
                    {
                        DicDocumentTypeCodes.PriorityRequest,
                        DicDocumentTypeCodes.TranslationRequest,
						DicDocumentTypeCodes.IcgsMismatchRequest,
						DicDocumentTypeCodes.DeclarantAddressMismatchRequest,
						DicDocumentTypeCodes.EcoBioOrganicDesignationRequest,
						DicDocumentTypeCodes.IcgsOrImageMissingRequest,
						DicDocumentTypeCodes.DesignationTranslationRequest,
						DicDocumentTypeCodes.TZ_ZAP_O_IZOBR,
                        //todo проверить типы документов вроде заменились на один
                        //DicDocumentTypeCodes.OUT_Zap_Pred_Prior_v1_19,
                        //DicDocumentTypeCodes.OUT_Zap_Pred_Perevod_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_Nesootv_MKTU_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_Adres_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_ECO_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_otsuts_MKTU_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_perevod_oboz_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pol_perevod_oboz_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pred_kach_izobr_v1_19,
						//DicDocumentTypeCodes.OUT_Zap_Pol_kach_izobr_v1_19,
                        DicDocumentTypeCodes.OUT_Uv_pol_prekr_del_otv_zap_v1_19,
                        DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTerminationFormal

						/*TODO check with analyst
						DicDocumentTypeCodes.ProxyLetterRequest
						DicDocumentTypeCodes.MatchingIconsRequest
						DicDocumentTypeCodes.StateSymbolsAndOtherRequest*/
					}
                },
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.TimeRestore
                    },
                    DocumentTypeCodes = new List<string>
                    {
                        DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTermination,
                    }
                },
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.PreliminaryRejectionObjectionConsideration
                    },
                    DocumentTypeCodes = new List<string>
                    {
                        DicDocumentTypeCodes.TrademarkRegistrationDecision,
						DicDocumentTypeCodes.TrademarkPartialRegistrationDecision,
						DicDocumentTypeCodes.TrademarkRegistrationRejectionDecision
                    }
                },
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.TmConvert
                    },
                    DocumentTypeCodes = new List<string>
                    {
                        DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19
                    }
                },
                new DocumentTypeTariffRelation
                {
                    TariffCodes = new List<string>
                    {
                        DicTariffCodes.TmChange,
                        DicTariffCodes.TmSameTypeChange
                    },
                    DocumentTypeCodes = new List<string>
                    {
                        DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19,
                        DicDocumentTypeCodes.OUT_UV_Pred_izmen_adres_v1_19,
                        DicDocumentTypeCodes.OUT_UV_Pred_izmen_naim_adr_v1_19,
                        DicDocumentTypeCodes.UV_TZ_VN_IZM_NAIMEN_ZAYAV,
                        DicDocumentTypeCodes.UV_TZ_VN_IZM,
                        DicDocumentTypeCodes.UV_TZ_VN_IZM_PERECH_TOV,
                        DicDocumentTypeCodes.UV_TZ_VN_IZM_PRED_ZAYAV,
                        DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19,
                        DicDocumentTypeCodes.UV_TZ_VN_IZM_YUR_ADR_PER
                    }
                }
            };
		public async Task Execute(Owner.Type ownerType, int ownerId, string documentTypeCode)
		{
			try
			{
				List<PaymentInvoice> paymentInvoices;
				switch (ownerType)
				{
					case Owner.Type.Request:
						paymentInvoices = await Executor.GetQuery<GetPaymentInvoicesByRequestIdQuery>()
							.Process(q => q.ExecuteAsync(ownerId));
						break;
					default:
						throw new NotImplementedException();
				}
				var chargedStatus = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>()
					.Process(q => q.Execute(DicPaymentStatusCodes.Charged));
				var tariffCodes = DocumentTypesTariffConfig
					.Where(c => c.DocumentTypeCodes.Contains(documentTypeCode)).SelectMany(c => c.TariffCodes);
				var invoicesToCharge = paymentInvoices.Where(pi =>
					pi.Status.Code == DicPaymentStatusCodes.Credited && tariffCodes.Contains(pi.Tariff.Code));
				var systemUser = Executor.GetQuery<GetUserByXinQuery>()
					.Process(q => q.Execute(UserConstants.SystemUserXin));

				//TODO?
				foreach (var paymentInvoice in invoicesToCharge)
				{
					var date = DateTimeOffset.UtcNow;
					paymentInvoice.Status = chargedStatus;
					paymentInvoice.StatusId = chargedStatus?.Id ?? 0;
					paymentInvoice.DateComplete = date;
					paymentInvoice.WriteOffUserId = systemUser?.Id;

					var isExportedTo1C = await Executor.GetHandler<ExportPaymentTo1CHandler>().Process(r => r.ExecuteAsync(ownerType,
						paymentInvoice.Id,
						PaymentInvoiveChangeFlag.NewChargedPaymentInvoice, chargedStatus.Code, date));
					if (isExportedTo1C)
					{
						//Устанавливаем дату экспорта в 1С если экспорт произошол успешно.
						paymentInvoice.DateExportedTo1C = DateTimeOffset.UtcNow;
					}
					Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoice));
				}
			}
			catch (Exception ex)
			{
				var log = new LogRecord();
				log.LogType = LogType.General;
				log.LogErrorType = LogErrorType.Error;
				log.Message = "Charge Paymenrt Invoice Error " + ex.StackTrace;
				var logId = await Executor.GetCommand<CreateLogRecordCommand>().Process(q => q.ExecuteAsync(log));
			}
		}

        internal class DocumentTypeTariffRelation
        {
            internal List<string> DocumentTypeCodes { get; set; }
            internal List<string> TariffCodes { get; set; }
        }
    }
}
