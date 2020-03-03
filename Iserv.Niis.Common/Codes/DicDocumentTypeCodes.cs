using System;
using System.Collections.Generic;

namespace Iserv.Niis.Common.Codes
{
    //todo! Коды типов материалов, коды в классе DicDocumentType не доступны по всему проекту, буду переносить по мере надобности
    public static class DicDocumentTypeCodes
    {
        public static List<string> Internal()
        {
            return new List<string>
            {
                "REESTR_INPUT_KANC",
                "REESTR_OUT_KANC",
                "REESTR_POSTAGE-STAMP_KANC",
                "KANC_1",
                "KANC_2",
                "REESTR_INPUT_DEPART",
                "REESTR_OUTPUT_DEPART",
                "REESTR_RASPREDELENIA"
            };
        }

        public static List<string> DocumentRequest()
        {
            return new List<string>
            {
                SAInformationAboutPreviouslyMadeSale,
                "PN.1",
                "AP_ATTACHED_FILE",
                StatementCopyrights,
                "AP_UD_L",
                "AP_COPY",
                "AP_IMAGE",
                "AP_REFERAT",
                "AP_DESCRIP",
                "AP_FDESCRIP",
                "AP_DOGOVOR",
                "AP_PEREVOD",
                "001.052.1",
                "001.001.1",
                "IMAGE",
                "ZTM",
                "TIM_ZAYAVLENIE_SCAN",
                "DOG_ISKL_LIC_TZ"
            };
        }

        public static List<string> IgnoreGenerateIncomingNumber()
        {
            return new List<string>
            {
                RequestForIndustrialSample,
                RequestForUsefulModel,
                _001_001E,
                RequestForInvention,
                RequestForSelectiveAchievement,
                A,
                RequestForNmpt,
                RequestForInternationalTrademark,
                _001_001B_PCT,
                _001_001B_EAPO,
                Tim001
            };
        }

        #region [Obsolete]

        #region PRED

        /// <summary>
        /// Уведомление о принятии к рассмотрению заявки на регистрацию ТЗ_знака обслуживания (без оплаты за полную экспертизу)
        /// </summary>
        public const string NotificationOfTmRequestReviewingAcceptance = "TZPRED1.0";

        /// <summary>
        /// 85_Уведомление о прекращении делопроизводства (в связи с неуплатой за полную экспертизу)
        /// </summary>
        public const string NotificationOfPaymentlessOfficeWorkTermination = "TZPRED5";

        #endregion

        #region POL

        /// <summary>
        /// Экспертное заключение об отказе (окончательное)
        /// </summary>
        public const string ExpertRefusalOpinionFinal = "TZPOL01";

        /// <summary>
        /// Экспертное заключение о регистрации ТЗ
        /// </summary>
        public const string ExpertTmRegisterOpinion = "TZPOL3";

        /// <summary>
        /// 7.12_Уведомление о принятии решения о регистрации
        /// </summary>
        public const string NotificationOfRegistrationDecision = "TZPOL4";

        /// <summary>
        /// ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ (знака обслуживания)
        /// </summary>
        public const string ExpertTmRegisterFinalOpinion = "TZPOL32";

        /// <summary>
        /// ЗАКЛЮЧЕНИЕ О ЧАСТИЧНОЙ РЕГИСТРАЦИИ ТЗ
        /// </summary>
        public const string ExpertTmRegistrationOpinionWithDisclaimer = "TZPOL311";

        #endregion

        #region NEW

        /// <summary>
        /// Уведомление о внесении изменений в соответствии с объемом внесенных изменений
        /// </summary>
        public const string ChangesNotification = "NOT4_NEW";

        #endregion

        #region OTHER

        /// <summary>
        /// Ответ на входящее письмо
        /// </summary>
        public const string IncomingAnswerLetter = "001.00.ИСХ";

        /// <summary>
        /// 
        /// </summary>
        public const string Tim001 = "TIM_001.001";

        /// <summary>
        /// 29_Заключение об отказе в выдаче патента на полезную модель (ОТРИЦАТЕЛЬНОЕ)
        /// </summary>
        public const string RefusalToGratConclusion = "PM4P";

        /// <summary>
        /// 26_Заключение о выдаче полезной модели.
        /// </summary>
        public const string GrantingUsefulModel = "PM3";

        /// <summary>
        /// Заключение о выдча полезной модели (каз).
        /// </summary>
        public const string GrantingUsefulModelKz = "PM3KAZ";

        /// <summary>
        /// 32_Запрос экспертизы по ПМ
        /// </summary>
        public const string ExpertizeRequest = "PM2";

        /// <summary>
        /// 40_Уведомление об отзыве заявки на ПМ из-за непредоставления ответа на запрос (УВ-2пм(З))
        /// </summary>
        public const string UV_2PMZ = "UV-2PM(Z)";

        /// <summary>
        /// 35_Уведомление о принятии решения о выдаче патента на ПМ (УВ-Кпм)
        /// </summary>
        public const string UV_KPM = "UV-KPM";

        /// <summary>
        /// Ходатайство о досрочной публикации
        /// </summary>
        public const string ApplicationForEarlyPublication = "001.004G.1";

        /// <summary>
        /// Запрос по заявке на селекционное достижение
        /// </summary>
        public const string SelectiveAchievementRequest = "S1";

        /// <summary>
        /// Реестр на отправку ОД
        /// удаленно (старый док)
        /// </summary>
        //public const string Reestr_006_014_3 = "006.014.3";

        /// <summary>
        /// Ходатайство о внесении изменений
        /// </summary>
        public const string PetitionForChanging = "001.004F";

        /// <summary>
        /// Справка об отсутствии секретности
        /// </summary>
        public const string СertificateOfConfidentiality = "Сertificate_Of_Confidentiality";

        /// <summary>
        /// Судебные документы
        /// </summary>
        public const string _006_02_01 = "006.02.01";

        /// <summary>
        /// Заявка на ИЗ_PCT
        /// </summary>
        public const string _001_001B_PCT = "001.001B_PCT";

        /// <summary>
        /// Заявка на ИЗ_ЕАПО
        /// </summary>
        public const string _001_001B_EAPO = "001.001B_EAPO";

        /// <summary>
        /// Заявка на инновационный ИЗ
        /// </summary>
        public const string _001_001C = "001.001C";

        /// <summary>
        /// Заявка на ИЗ
        /// </summary>
        public const string RequestForInvention = "001.001B";

        /// <summary>
        /// Заявка на НМПТ
        /// </summary>
        public const string RequestForNmpt = "001.001A_1";

        /// <summary>
        /// Заявка на СД
        /// </summary>
        public const string RequestForSelectiveAchievement = "001.001G";

        /// <summary>
        /// Заявка на ТЗ
        /// </summary>
        public const string RequestForTrademark = "001.001A";

        /// <summary>
        /// Заявка на МТЗ
        /// </summary>
        public const string RequestForInternationalTrademark = "001.001A_2";

        /// <summary>
        /// Заявка на ПМ
        /// </summary>
        public const string RequestForUsefulModel = "001.001D";

        /// <summary>
        /// Заявка на патент ПО
        /// </summary>
        public const string RequestForIndustrialSample = "001.001F";

        /// <summary>
        /// Изображение
        /// </summary>
        public const string Image = "001.043";

        /// <summary>
        /// Таблица признаков
        /// </summary>
        public const string AttributesTable = "001.035";

        /// <summary>
        /// Уведомление о непринятии к рассмотрению хадатайства 
        /// </summary>
        public const string UVED_NEPRINJATIE = "UVED_NEPRINJATIE";

        #endregion



        #region Internal codes

        /// <summary>
        /// Отчет о поиске ИЗ
        /// </summary>
        public const string InventionSearchReport = "IZ_OTCHET_POISK_IZ";

        /// <summary>
        /// Заявление на проведение поиска по  ИЗ
        /// </summary>
        public const string IZ_POISK = "IZ_POISK";

        /// <summary>
        /// Отчет о поиске (филиал) ИЗ
        /// </summary>
        public const string IZ_OTCHET_POISK_P = "IZ_OTCHET_POISK_P";

        /// <summary>
        /// Заявление  ТЗ
        /// </summary>
        public const string StatementTrademark = "001.001_1Z";

        /// <summary>
        /// Заявление ИЗ (РСТ и ЕАПВ)
        /// </summary>
        public const string _001_001_1B_IN = "001.001_1B_IN";

        /// <summary>
        /// Заявление  АП
        /// </summary>
        public const string AP_ZAYAVLENIE_SCAN = "AP_ZAYAVLENIE_SCAN";

        /// <summary>
        /// Заявление  ТИМ
        /// </summary>
        public const string TIM_ZAYAVLENIE_SCAN = "TIM_ZAYAVLENIE_SCAN";

        /// <summary>
        /// 4.26_Уведомление на оплату для физических лиц _ПМ
        /// </summary>
        public const string PaymentNotification = "RKS_YVED_PM";

        /// <summary>
        /// Уведомление об оплате за подачу ТЗ
        /// </summary>
        public const string TrademarkPaymentNotification = "TZREG_2.09";

        /// <summary>
        /// Заявление  АП
        /// </summary
        public const string StatementCopyrights = "AP_ZAYAVLENIE_SCAN";

        /// <summary>
        /// 4.21_Уведомление на полезную модель (неподан. в связи с отсутствием оплаты за прием заявки) _ПМ
        /// </summary>
        public const string NotPaymentRequest = "RKS_YVED_NEP_PM";

        /// <summary>
        /// Заявление  ПО
        /// </summary>
        public const string StatementIndustrialDesigns = "001.001_1PO";

        /// <summary>
        /// Заключение о выдаче патента ПМ на каз.я
        /// </summary
        public const string GrantingPatentConclusion = "IZ-3PM-KZ";

        /// <summary>
        /// Заявление  ИЗ
        /// </summary>
        public const string StatementInventions = "001.001_1B";

        /// <summary>
        /// Заявление  НМПТ
        /// </summary>
        public const string StatementNamePlaces = "001.001_NMPT";

		/// <summary>
		/// Заявление  НМПТ
		/// </summary>
		public const string StatementNameOfOrigin = "001_001_NMPT";

		/// <summary>
		/// Заявление СД
		/// </summary>
		public const string StatementSelectiveAchievs = "001.001_SA";

        /// <summary>
        /// Заявка на пред.патент ИЗ
        /// </summary>
		public const string A = "A";

        /// <summary>
        /// Заявка на предварительный ПО
        /// </summary>
		public const string _001_001E = "001.001E";

        /// <summary>
        /// Заявка на признание ТЗ общеизвестным
        /// </summary>
		public const string _001_001A_1 = "001.001A.1";

        /// <summary>
        /// Заявление  ПМ
        /// </summary>        
        public const string StatementUsefulModels = "001.001_1PM";

        /// <summary>
        /// Заявление  ТЗ
        /// </summary>
        public const string Statement = "001.001";

        /// <summary>
        /// Реестр для передачи сведений по договорам
        /// </summary>
        public const string DK_Registry_Transfer = "DK_Registry_Transfer";

        /// <summary>
        /// Перевод копии ранее поданной заявки
        /// </summary>
        public const string _001_072_2_TZ = "001.072_2_TZ";

        /// <summary>
        /// Результат распределения заявок
        /// </summary>
        public const string ResultDistributionRequests = "Result_Distribution_Requests";

        #endregion

        #region Outgoing codes

        /// <summary>
        /// Прочие документы
        /// </summary>
        public const string Others = "006.014";

        /// <summary>
        /// 4.11_Уведомление о продлении срока представления ответа 
        /// </summary>
        public const string FE6 = "FE6";

        /// <summary>
        /// Уведомление об отзыве  заявки
        /// </summary>
        public const string S5 = "S5";                      

        /// <summary>
        /// Заключение о регистрации
        /// </summary>
        public const string ConclusionAboutRegistrationOfContract = "DK_ZAKLUCHENIE";

        /// <summary>
        /// Заключение об отказе в регистрации
        /// </summary>
        public const string ConclusionAboutRegistrationRefusalOfContract = "DK_ZAKLUCHENIE_OTKAZ";

        /// <summary>
        /// 38_Уведомление об  отзыве заявки на патент из-за непредоставления ответа на запрос (УВ-2п(З))
        /// </summary>
        public const string NotificationOfRevocationOfPatentApplication = "UV-2P(Z)";

        /// <summary>
        /// 33_Уведомление о принятии решения о выдаче патента (УВ-Кб)
        /// </summary>
        public const string NotificationOfDecisionPatentGrant = "UV-KB";

        /// <summary>
        /// 41_Уведомление об отзыве в связи с неоплатой экспертизы по существу (УВ-2 эс)
        /// </summary>
        public const string NotificationOfRevocationOfPaymentlessSubstantiveExamination = "UV-2ES";

        /// <summary>
        /// 44_Уведомление об окончательном отзыве  заявки на патент из-за непредоставления ответа на запрос (УВ-2(о))
        /// </summary>
        public const string NotificationOfAnswerlessPatentRequestFinalRecall = "UV-2(O)";

        /// <summary>
        /// Уведомление об окончательном отзыве в связи с неоплатой экспертизы по существу (УВ-2 эс(о))
        /// </summary>
        public const string NotificationOfPaymentlessExaminationFinalRecall = "UV-2ES(O)";

        /// <summary>
        /// 4.7_1_Уведомление о положительном результате ФЭ (без оплаты за экспертизу по сущ.)
        /// </summary>
        public const string NotificationForPaymentlessPozitiveFormalExamination = "FE01.0";

        /// <summary>
        /// Уведомление ПО(экспертиза по существу)
        /// </summary>
        public const string NotificationForSubstantiveExamination = "UV_PO3";

        /// <summary>
        /// 4.7_Уведомление о положительном результате ФЭ
        /// </summary>
        public const string NotificationForPozitiveFormalExamination = "FE01";

        /// <summary>
        /// 4.17_Запрос формальной экспертизы на изобретение
        /// </summary>
        public const string RequestForFormalExamForInvention = "FE11";

        /// <summary>
        /// Уведомление о принятии решения
        /// </summary>
        public const string DecisionNotification = "NOT1_NEW";

        /// <summary>
        /// 4.18_Уведомление на оплату для физических лиц _ИЗ
        /// </summary>
        public const string NotificationForInventionPaymentForIndividuals = "RKS_YVED";

        /// <summary>
        /// 4.19_Уведомление на оплату для физических лиц, имеющих льготу_ИЗ
        /// </summary>
        public const string NotificationForInventionPaymentForBeneficiaries = "RKS_YVED_IZ_ILG";

        /// <summary>
        /// 4.20_Уведомление на изобретение (неподан. в связи с отсутствием оплаты за прием заявки)_ИЗ
        /// </summary>
        public const string NotificationForPaymentlessInventionRequest = "RKS_YVED_NEP_IZ";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (по просьбе)
        /// </summary>
        public const string NotificationOfTrademarkRequestRegistationByRequest = "TZPRED2";

        /// <summary>
        /// 92_Уведомление о прекращении делопроизводства (по просьбе)
        /// </summary>
        public const string NotificationOfOfficeWorkTerminationByRequest = "TZPOL8";

        /// <summary>
        /// Запрос предварительной экспертизы
        /// </summary>
        public const string RequestForPreExamination = "TZPRED4";

        /// <summary>
        /// 94_Запрос полной экспертизы
        /// </summary>
        public const string RequestForFullExamination = "TZPOL10";

        /// <summary>
        /// Запрос экспертизы по существу (ИЗ-2а_kz)
        /// </summary>
        public const string RequestForSubstantiveExamination = "IZ-2A-KZ";

        /// <summary>
        /// 30_Запрос экспертизы заявки на выдачу патента на изобретение (Форма ИЗ-2б)
        /// </summary>
        public const string RequestForExaminationOfInventionPatentRequest = "IZ-2B";

        /// <summary>
        /// Запрос экспертизы заявки на выдачу патента на изобретение (Форма ИЗ-2б) Филиал
        /// </summary>
        public const string RequestForExaminationOfInventionPatentFilialRequest = "IZ-2B_FILIAL";

        /// <summary>
        /// Экспертное заключение об отказе (окончательное) НМПТ
        /// </summary>
        public const string ExpertRefusalNmptFinalConclusion = "POL01_NMPT";

        /// <summary>
        /// ЗАКЛЮЧЕНИЕ О ПРЕДВАРИТЕЛЬНОМ ОТКАЗЕ В РЕГИСТРАЦИИ ТЗ
        /// </summary>
        public const string ExpertTmRegisterRefusalOpinion = "TZPOL2";

        /// <summary>
        /// Запрос о доверенности
        /// </summary>
        public const string TZ_ZAP_O_DOV = "TZ_ZAP_O_DOV";

        /// <summary>
        /// Запрос по приоритету
        /// </summary>
        public const string PriorityRequest = "REQ3_NEW";

        /// <summary>
        /// Запрос по переводу
        /// </summary>
        public const string TranslationRequest = "REQ4_NEW";

        /// <summary>
        /// Запрос по несоответствию товаров и услуг МКТУ
        /// </summary>
        public const string IcgsMismatchRequest = "REQ5_NEW";

        /// <summary>
        /// Запрос по несоответствию адреса заявителя
        /// </summary>
        public const string DeclarantAddressMismatchRequest = "REQ6_NEW";

        /// <summary>
        /// Запрос по обозначениям ECO, BIO, Organic
        /// </summary>
        public const string EcoBioOrganicDesignationRequest = "REQ7_NEW";

        /// <summary>
        /// Запрос по отсутствию МКТУ или изображения
        /// </summary>
        public const string IcgsOrImageMissingRequest = "REQ8_NEW";

        /// <summary>
        /// Запрос по переводу обозначения
        /// </summary>
        public const string DesignationTranslationRequest = "REQ9_NEW";

        /// <summary>
        /// Запрос об изображениях ТЗ
        /// </summary>
        public const string TZ_ZAP_O_IZOBR = "TZ_ZAP_O_IZOBR";

        /// <summary>
        /// Запрос по знакам соответствия
        /// </summary>
        public const string MatchingIconsRequest = "REQ11_NEW";

        /// <summary>
        /// Запрос по оплате (за предварительную экспертизу)
        /// </summary>
        public const string FormalExpertizePaymentRequest = "REQ12_NEW";

        /// <summary>
        /// Запрос по гос. символике и другие
        /// </summary>
        public const string StateSymbolsAndOtherRequest = "REQ13_NEW";

        /// <summary>
        /// Запрос по авторским правам
        /// </summary>
        public const string CopyrightRequest = "REQ14_NEW";

        /// <summary>
        /// Запрос по личным неимущественным правам
        /// </summary>
        public const string PersonalNonPropertyRequest = "REQ15_NEW";

        /// <summary>
        /// Запрос на объективную связь
        /// </summary>
        public const string ObjectiveLinkRequest = "REQ16_NEW";

        /// <summary>
        /// Запрос по письму- согласию
        /// </summary>
        public const string ConsentLetterRequest = "REQ17_NEW";

        /// <summary>
        /// Экспертное заключение об отказе в регистрации НМПТ
        /// </summary>
        public const string ExpertNmptRegisterRefusalOpinion = "POL2_NMPT";

        /// <summary>
        /// Заключение эксперта по заявке на ТЗ
        /// </summary>
        public const string ExpertOpinionTm = "TZ-LIST_SOGLAS_ZAKL";

        /// <summary>
        /// Экспертное заключение о регистрации НМПТ
        /// </summary>
        public const string RegisterNmptExpertConclusion = "POL3_NVPT";

        /// <summary>
        /// РЕШЕНИЕ и ЗАКЛЮЧЕНИЕ о выдаче патента на изобретение(Форма ИЗ-3б)
        /// </summary>
        public const string ConclusionOfInventionPatentGrant = "IZ_3B";

        /// <summary>
        /// Отрицательное заключение на патент на изобретение(Форма ИЗ-4п)
        /// </summary>
        public const string ConclusionOfInventionPatentGrantRefuse = "IZ-4P";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (не предоставление материалов или ответа на запрос)
        /// </summary>
        public const string NotificationOfPaymentlessOfficeWorkTerminationFormal = "NOT2_NEW";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (не предоставление материалов или ответа на запрос)
        /// </summary>
        public const string TZPRED3 = "TZPRED3";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (в связи с отстутствием ответа на запрос) (полная экспертиза)
        /// </summary>
        public const string NotificationOfAnswerlessOfficeWorkTermination = "TZPOL9";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о регистрации товарного знака
        /// </summary>
        public const string NotificationOfTmRegistration = "TZPOL32_UVED";

        /// <summary>
        /// Уведомление об отказе
        /// </summary>
        public const string NotificationOfRefusal = "AP_REJECT";

        /// <summary>
        /// ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ  с согласием заявителя
        /// </summary>
        public const string ExpertTmRegistrationFinalOpinionWithApplicantConsent = "TZPOL32_S_SOG";

        /// <summary>
        /// ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ  без согласия заявителя
        /// </summary>
        public const string ExpertTmRegistrationFinalOpinionWithoutApplicantConsent = "TZPOL32_S_BEZSOG";

        /// <summary>
        /// Уведомления о продлении срока ответа на запрос предваритаельной экспертизы ТЗ
        /// </summary>
        public const string ResponseTermProlongationNotification = "TZPRED7";

        /// <summary>
        /// Уведомление о выделении заявки на регистрацию ТЗ_знака обслуживания
        /// </summary>
        public const string ResponseReleaseRequestRegistrationNotification = "TZPRED6";

        /// <summary>
        /// 87_Уведомление о восст. делопроизв. форм. эксп.
        /// </summary>
        public const string ResponseDelapFormsExp = "TZPRED8";

        /// <summary>
        /// 88_Уведомление (промежуточное, об истечении сроков предоставления оплаты за проведение экспертизы на регистрацию товарного знака)
        /// </summary>
        public const string NotificationOfRegistrationExaminationTimeExpiration = "TZPRED9";

        /// <summary>
        /// 89_Уведомление (промежуточное, об истечении сроков предоставления ответа на запрос экспертизы)
        /// </summary>
        public const string NotificationOfAnswerTimeExpiration = "TZPRED10";

        /// <summary>
        /// 86_Уведомление о выделении заявки на регистрацию ТЗ_знака обслуживания
        /// </summary>
        public const string NotificationReleaseRequestRegistrationNotification = "TZPOL99";

        /// <summary>
        /// 89_1_Уведомление (промежуточное, об истечении сроков предоставления оплаты за прием и проведение форм.экспертизы) ТЗ
        /// </summary>
        public const string NotificationOfFormalExaminationTimeExpiration = "PRED_89_1";

        /// <summary>
        /// Реферат
        /// </summary>
        public const string Essay = "001.024";

        /// <summary>
        /// Реферат на иностранном языке
        /// </summary>
        public const string EssayForeign = "001.024.1";

        /// <summary>
        /// Измененный реферат
        /// </summary>
        public const string ChangedEssay = "001.024.0";

        /// <summary>
        /// Описание изобретения
        /// </summary>
        public const string InventionDescription = "001.072";

        /// <summary>
        /// Измененное описание изобретения
        /// </summary>
        public const string ChangedInventionDescription = "001.072.0";

        /// <summary>
        /// Описание ПМ
        /// </summary>        
        public const string UsefulModelDescription = "001.071";

        /// <summary>
        /// Полное описание патента ИЗ
        /// </summary>
        public const string InventionFullDescription = "OP_PAT";

        /// <summary>
        /// Полное описание патента ПМ
        /// </summary>
        public const string UsefulModelFullDescription = "OP_PAT_PM";

        /// <summary>
        /// Полное описание СД
        /// </summary>
        public const string SelectiveAchievementFullDescription = "OP_PAT_SA";

        /// <summary>
        /// Полное описание патента ИЗ (kz)
        /// </summary>
        public const string InventionFullDescriptionKz = "OP_PAT_KZ";

        /// <summary>
        /// Полное описание патента ПМ (kz)
        /// </summary>
        public const string UsefulModelFullDescriptionKz = "OP_PAT_PM_KZ";

        /// <summary>
        /// Описание промышленного образца
        /// </summary>
        public const string IndustrialModelDescription = "001.073";

        /// <summary>
        /// Полное описание СД (kz)
        /// </summary>
        public const string SelectiveAchievementFullDescriptionKz = "OP_PAT_SA_KZ";

        /// <summary>
        /// Полное описание патента ПО
        /// </summary>
        public const string IndustrialDesignFullDescription = "OP_PAT_ID";

        /// <summary>
        /// Полное описание патента ПО (kz)
        /// </summary>
        public const string IndustrialDesignFullDescriptionKz = "OP_PAT_ID_KZ";

        /// <summary>
        /// Формула ИЗ
        /// </summary>
        public const string FormulaInvention = "001.064";

        /// <summary>
        /// Измененная формула ИЗ
        /// </summary>
        public const string ChangedFormulaInvention = "001.064.0";

        /// <summary>
        /// Формула ПМ
        /// </summary>        
        public const string FormulaUsefulModel = "001.062";

        /// <summary>
        /// Уведомление об оплате гос.пошлины (УВП-У5)
        /// </summary>
        public const string NotificationOfPaymentOfStateDuty = "UVP-U5";

        /// <summary>
        /// Экспертное заключение об отказе в выдаче патента на промышленный образец
        /// </summary>
        public const string PO4 = "PO4";

        /// <summary>
        /// Экспертное заключение о выдаче патента на промышленный образец по заявке
        /// </summary>
        public const string PO4_1 = "PO4_1";

        /// <summary>
        /// Экспертное заключение о выдаче патента на промышленный образец по заявке
        /// </summary>
        public const string PO5 = "PO5";

        /// <summary>
        /// Экспертное заключение о выдаче патента на промышленный образец(каз.яз)
        /// </summary>
        public const string PO5_KZ = "PO5_KZ";

        /// <summary>
        /// Решение и заключение о выдаче патента на ПО
        /// </summary>
        public const string PO5_1111 = "PO5-1111";

        /// <summary>
        /// Решение и заключение о выдаче патента на ПО (каз)
        /// </summary>
        public const string PO5_10 = "PO5-10";

        /// <summary>
        /// Запрос экспертизы(по существу)
        /// </summary>
        public const string Z_PO7 = "Z_PO7";

        /// <summary>
        /// Экспертное заключение об отказе в выдаче патента на промышленный образец(оригинальность)
        /// </summary>
        public const string PO7_1 = "PO7_1";

        /// <summary>
        /// Экспертное заключение об отказе в выдаче патента на промышленный образец(новизна)
        /// </summary>
        public const string PO7_2 = "PO7_2";

        /// <summary>
        /// Уведомление о прекращении делопроизводства ПО(отозванные)
        /// </summary>
        public const string PO8 = "PO8";

        /// <summary>
        /// Уведомление о прекращении делопроизводства ПО(неподанные)
        /// </summary>
        public const string PO8_1 = "PO8_1";

        /// <summary>
        /// Уведомление об отсутствии оплаты за подачу заявки на ПО
        /// </summary>
        public const string POL2_0 = "POL2.0";

        /// <summary>
        /// Уведомление о проведении формальной экспертизы по заявке на ПО
        /// </summary>
        public const string PO1 = "PO1";

        /// <summary>
        /// Уведомление о проведении формальной экспертизы по заявке на ПО (без оплаты за экспертизу)
        /// </summary>
        public const string PO1_1 = "PO1.1";

        /// <summary>
        /// Запрос экспертизы (формальная)
        /// </summary>
        public const string PO7 = "PO7";

        /// <summary>
        /// Уведомление о принятии решения о выдаче патента на ПО
        /// </summary>
        public const string UV_P_PO = "UV_P_PO";

        /// <summary>
        /// Изображения заявляемого обозначения: цветное (PNG)
        /// </summary>
        public const string _001_001_1A = "001.001_1A";

        /// <summary>
        /// Уведомление о прекращении делопроизводства
        /// </summary>
        public const string _006_008E = "006.008E";

        /// <summary>
        /// Уведомление о возможности восстановлении пропущенного срока предоставления ответа на запрос экспертизы
        /// </summary>
        public const string _006_0088 = "006.0088";

        /// <summary>
        /// 90_Уведомление о внесении изменений в заявленное обозначение
        /// </summary>
        public const string UV_TZ_VN_IZM = "TZ_VN_IZM";

        /// <summary>
        /// 96_Уведомление о внесении изменения в адрес заявителя
        /// </summary>
        public const string UV_TZ_VN_IZM_ADR = "TZ_VN_IZM_ADR";

        /// <summary>
        /// Уведомление о внесении изменения в юр.адрес и адрес для переписки
        /// </summary>
        public const string UV_TZ_VN_IZM_YUR_ADR_PER = "TZ_VN_IZM_YUR_ADR_PER";

        /// <summary>
        /// Уведомление о внесении изменений в представителя заявителя
        /// </summary>
        public const string UV_TZ_VN_IZM_PRED_ZAYAV = "TZ_VN_IZM_PRED_ZAYAV";

        /// <summary>
        /// 88_Уведомление о внесении изменений в перечень товаров
        /// </summary>
        public const string UV_TZ_VN_IZM_PERECH_TOV = "TZ_VN_IZM_PERECH_TOV";

        /// <summary>
        /// 89_Уведомление о внесении изменений в наименование заявителя
        /// </summary>
        public const string UV_TZ_VN_IZM_NAIMEN_ZAYAV = "TZ_VN_IZM_NAIMEN_ZAYAV";

        /// <summary>
        /// 4.8_Уведомление об отзыве (ФЭ)
        /// </summary>
        public const string FE13 = "FE13";

        /// <summary>
        /// Уведомление о положительном результате ФЭ (каз)
        /// </summary>
        public const string NotificationForPozitiveFormalExaminationKz = "FE01_KZ";

        /// <summary>
        /// 4.8.1_Уведомление (Неподанная в связи с отсутствием перевода)
        /// </summary>
        public const string FE133 = "FE133";

        /// <summary>
        /// Удостоверение автора ПО
        /// </summary>
        public const string IndustrialDesignAuthorCertificate = "PAT_AVT_PO";

        /// <summary>
        /// Удостоверение автора ИЗ
        /// </summary>
        public const string InventionAuthorCertificate = "PAT_AVT_IZ";

        /// <summary>
        /// Удостоверение автора ПМ
        /// </summary>
        public const string UsefulModelAuthorCertificate = "PAT_AVT_PM";

        /// <summary>
        /// Удостоверение автора СД (растеневодство)
        /// </summary>
        public const string AgriculturalSelectiveAchievementAuthorCertificate = "PAT_AVT_CD_RASTENIE";

        /// <summary>
        /// Удостоверение автора СД (животноводство)
        /// </summary>
        public const string AnimalHusbandrySelectiveAchievementAuthorCertificate = "PAT_AVT_CD_ZHIVOD";

        /// <summary>
        /// Уведомление об аннулировании ОД ТЗ
        /// </summary>
        public const string ProtectionDocAnullmentNotification = "GR_TZ_POL_UV";

        /// <summary>
        /// Реестры эксп.заключений в МЮ РК (СД после проверки на наименование)
        /// </summary>
        public const string SA_006_014_0 = "006.014.0.SA";

        /// <summary>
        /// Реестры эксп.заключений в МЮ РК (СД после проверки на патентоспособность)
        /// </summary>
        public const string SA_006_014_1 = "006.014.1.SA";

        /// <summary>
        /// Сопроводительное в Госкомиссию на проверку наименования (растениеводство)
        /// </summary>
        public const string SA_1_SOPR = "SA_1_SOPR";

        /// <summary>
        /// Сопроводительное в Госкомиссию на патентоспособность (животноводство)
        /// </summary>
        public const string SA_2_SOPR = "SA_2_SOPR";

        /// <summary>
        /// Сопроводительное в Госкомиссию на патентоспособность (растениеводство)
        /// </summary>
        public const string SA_SOPR_2 = "SA_SOPR_2";

        /// <summary>
        /// Сопроводительное в Госкомиссию на проверку наименования (животноводство)
        /// </summary>
        public const string SA_SOPR_1 = "SA_SOPR_1";

        #endregion

        #region Public codes

        /// <summary>
        /// TZM-3 Экспертное заключение с положительным решением
        /// </summary>
        public const string PreliminaryPositiveExpertConclusion = "MTZ3";

        /// <summary>
        /// TZM-4 Окончательное экспертное заключение с положительным решением
        /// </summary>
        public const string FinalPositiveExpertConclusion = "MTZ4";

        /// <summary>
        /// TZM_5 Окончателный отказ
        /// </summary>
        public const string FinalNegativeExpertConclusion = "MTZ5";

        /// <summary>
        /// TZM_7 Окончательный отказ по патенту
        /// </summary>
        public const string FinalNegativeExpertConclusionPatent = "TZM_7";

        /// <summary>
        /// MTZ_3 Запрос экспертизЫ МТЗ
        /// </summary>
        public const string ItmExpertizeRequest = "MTZ_3";

        /// <summary>
        /// TZM_6 Сопроводительное письмо в ВОИС по МТЗ (отказы)
        /// </summary>
        public const string ItmAccompanyingNote = "TZM_6";

        /// <summary>
        /// TZM_6.1 Сопроводительное письмо в ВОИС по МТЗ (охрана)
        /// </summary>
        public const string ItmAccompanyingNoteProtection = "TZM_6.1";

        /// <summary>
        /// TZM_1 Экспертное заключение об отказе в регистрации МТЗ
        /// </summary>
        public const string NegativeRegistrationExpertConclusion = "TZM_1";

        /// <summary>
        /// TZM_3 REFUS PROVISOIRE DE PROTECTION
        /// </summary>
        public const string PreliminaryRejection = "MTZ2";

        /// <summary>
        /// MTZ2.1 REFUS Disclamation
        /// </summary>
        public const string PreliminaryRejectionDisclamated = "MTZ2.1";

        /// <summary>
        /// TZM_2.2 REFUS Partiel
        /// </summary>
        public const string PreliminaryPartialRejection = "MTZ2.2";

        /// <summary>
        /// TZM_2.3 REFUS
        /// </summary>
        public const string Rejection = "MTZ2.3";

        /// <summary>
        /// TZM_2.4 Refus provisoire partiel de protection
        /// </summary>
        public const string PartialRejection = "MTZ2.4";

        /// <summary>
        /// Реестры эксп.заключений в МЮ РК
        /// </summary>
        public const string RegistersOfExpertOpinionsInMjOfRk = "006.014.0";

        /// <summary>
        /// сопроводительное письмо ДК
        /// </summary>
        public const string CoveringLetterOfDk = "DK_SEND_CONTRACT";

        /// <summary>
        /// 36_Уведомление об отзыве заявки по просьбе заявителя (УВ-2(Х))
        /// </summary>
        public const string CoveringLetterOfDkAboutRecall = "UV-2(X)";

        /// <summary>
        /// 87_Уведомление о приостановлении делопроизводства
        /// </summary>
        public const string CoveringLetterOfDkAboutDelay = "TZPOL61";

        /// <summary>
        /// Уведомление о продолжении делопроизводства
        /// </summary>
        public const string CoveringLetterOfDkAboutDelay_1 = "TZPOL61_1";

        /// <summary>
        /// Выписка из заявки договора
        /// </summary>
        public const string StatementOfDk = "DK-VIPISKA";

        /// <summary>
        /// Уведомление по гос.пошлине
        /// </summary>
        public const string NotificationOfGovernmentDuty = "DK-U5";

        /// <summary>
        /// Уведомление (Свободная форма)
        /// </summary>
        public const string Notification = "DK_UV_CV";

        /// <summary>
        /// 99_УВЕДОМЛЕНИЕ о приостановлении делопроизводства
        /// </summary>
        public const string NotificationOfPausedWork = "DK_UVED_PRIOST_DEL";

        /// <summary>
        /// 99_УВЕДОМЛЕНИЕ о приостановлении делопроизводства
        /// </summary>
        public const string NotificationOfTerminationWork = "DK_UVED_PREK_DEL";

        /// <summary>
        /// 77_(71)_Уведомление о приостановлении делопроизводства МТЗ
        /// </summary>
        public const string NotificationOfTerminationWorkMTZ_1 = "MTZ_1";

        /// <summary>
        /// ДК- Счет на оплату
        /// </summary>
        public const string PaymentInvoiceOfDk = "DK_ORDER";

        /// <summary>
        /// Счет на оплату
        /// </summary>
        public const string PaymentInvoice = "P001";

        /// <summary>
        /// 101_Запрос заявителю
        /// </summary>
        public const string RequestForCustomerOfDk = "DK_ZAPROS";

        /// <summary>
        /// Ответ на запрос по ДК (Форма 1)
        /// </summary>
        public const string AnswerForRequestForCustomerOfDk = "DK-OTVET_DK";

        /// <summary>
        /// 95_Уведомление о прекращении делопроизводства (в связи с не уплатой)
        /// </summary>
        public const string NotificationOfAbsencePaymentOfDK = "TZPOL7";

        /// <summary>
        /// Уведомление  по заявке (Свободная форма)
        /// </summary>
        public const string FreeFormNotification = "UV-1";

        /// <summary>
        /// Экспертное заключение о положительном результате предварительной экспертизы заявки на выдачу патента на селекционное достижение
        /// </summary>
        public const string SelectiveAchievementExpertConclusionPositive = "S2";

        /// <summary>
        /// 4.2_Уведомление об отказе в выдаче патента на селекционное достижение
        /// </summary>
        public const string SelectiveAchievementPatentRefuseNotification = "S4";

        /// <summary>
        /// Экспертное заключение об отрицательном результате предварительной экспертизы заявки на выдачу патента на селекционное достижение
        /// </summary>
        public const string SelectiveAchievementExpertConclusionNegative = "S6";

        /// <summary>
        /// 4.1_Уведомление о выдаче патента на селекционное достижение
        /// </summary>
        public const string SelectiveAchievementPatentNotification = "S7";

        /// <summary>
        /// 4.3_Уведомление (положительное экспертное заключение предварительной экспертизы)
        /// </summary>
        public const string SelectiveAchievementExpertConclusionNotification = "S3";

        /// <summary>
        /// Патент на изобретение
        /// </summary>
        public const string InventionPatent = "OD_PAT_IZ_RU";

        /// <summary>
        /// Дубликат Патент на изобретение
        /// </summary>
        public const string InventionPatentDuplicate = "OD_PAT_IZ_RU_DUPLIKAT";

        /// <summary>
        /// Патент на Промышленный образец
        /// </summary>
        public const string IndustrialDesignsPatent = "PAT";

        /// <summary>
        /// Дубликат Патент на Промышленный образец
        /// </summary>
        public const string IndustrialDesignsPatentDuplicate = "PAT_DUPLIKAT";

        /// <summary>
        /// Патент на Полезную Модель
        /// </summary>
        public const string UsefulModelPatent = "PATPM";

        /// <summary>
        /// Дубликат Патент на Полезную Модель
        /// </summary>
        public const string UsefulModelPatentDuplicate = "PATPM_DUPLIKAT";

        /// <summary>
        /// Патент на СД  (растениеводство)
        /// </summary>
        public const string SelectiveAchievementsAgriculturalPatent = "SD_PAT";

        /// <summary>
        /// Дубликат Патент на СД  (растениеводство)
        /// </summary>
        public const string SelectiveAchievementsAgriculturalPatentDuplicate = "SD_PAT_DUPLIKAT";

        /// <summary>
        /// Патент на СД  (животноводство)
        /// </summary>
        public const string SelectiveAchievementsAnimalHusbandryPatent = "SD_PAT_2";

        /// <summary>
        /// Свидетельство на НМПТ
        /// </summary>
        public const string NmptCertificate = "SVID_NMPT";

        /// <summary>
        /// Свидетельство на ТЗ
        /// </summary>
        public const string TrademarkCertificate = "GR_TZ_SVID";

        /// <summary>
        /// Дубликат свидетельства на ТЗ
        /// </summary>
        public const string TrademarkCertificateDuplicate = "GR_TZ_SVID_DUPLIKAT";

        /// <summary>
        /// Регистрация НМПТ
        /// </summary>
        public const string NmptRegistry = "REG_NMPT";

        /// <summary>
        /// Регистрация ТЗ
        /// </summary>
        public const string TrademarkRegistry = "REG_TZ";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о продлении срока действия инновационного патента изобретение
        /// </summary>
        public const string NotificationOfInnovationExtension = "PRODLINP";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о продлении срока действия патента на изобретение
        /// </summary>
        public const string NotificationOfInventionExtension = "PRODLIZP";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о продлении срока действия патента на полезную модель
        /// </summary>
        public const string NotificationOfUsefulModelExtension = "PRODLPMP";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о продлении срока действия патента на промышленный образец
        /// </summary>
        public const string NotificationOfIndustrialExtension = "PRODLPOP";

        /// <summary>
        /// 8_УВЕДОМЛЕНИЕ о продлении срока действия регистрации товарного знака
        /// </summary>
        public const string NotificationOfTrademarkExtension = "GR_TZ_PRODL";

        /// <summary>
        /// 10_УВЕДОМЛЕНИЕ о продлении срока действия регистрации общеизвестного товарного знака
        /// </summary>
        public const string NotificationOfInternationalTrademarkExtension = "TZ_PRODL_OBSH";

        /// <summary>
        /// 5_УВЕДОМЛЕНИЕ о внесении  изменений в Государственный реестр и в свидетельство на товарный знак (знак обслуживания)
        /// </summary>
        public const string NotificationOfTrademarkChanging = "GR_TZ_IZMEN";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о внесении  изменений  в Государственный реестр и в патент на промышленный образец
        /// </summary>
        public const string NotificationOfIndustrialChanging = "PAT_PO-IZMEN";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о внесении изменений в Государственный реестр и  в патент на изобретение
        /// </summary>
        public const string NotificationOfInventionChanging = "PAT_IZ-IZMEN";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о внесении изменений в Государственный реестр и в инновационный патент на изобретение
        /// </summary>
        public const string NotificationOfInnovationChanging = "IN_P_IZ-IZMEN";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о внесении изменений в Государственный реестр и в патент на полезную модель
        /// </summary>
        public const string NotificationOfUsefulModelChanging = "PAT_PM-IZMEN";

        /// <summary>
        /// Приложение об уступке патента на ПО
        /// </summary>
        public const string IndustrialConcedingAttachment = "PRIL_PO_USTYP";

        /// <summary>
        /// Приложение об уступке патента на полезную модель
        /// </summary>
        public const string UsefulModelConcedingAttachment = "PRIL_USTUP_POL_MOD";

        /// <summary>
        /// Приложение об уступке патента на изобретение
        /// </summary>
        public const string InventionConcedingAttachment = "PRIL_USTUP_PAT_IZ";

        /// <summary>
        /// Приложение по уступке  ТЗ
        /// </summary>
        public const string TrademarkConcedingAttachment = "PRIL_TZ_UST";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о восстановлении действия инновационного патента на изобретение
        /// </summary>
        public const string NotificationOfInnovationRestoration = "VOSTIN";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о восстановлении действия патента на изобретение
        /// </summary>
        public const string NotificationOfInventionRestoration = "VOSTIZP";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о восстановлении действия патента на полезную модель
        /// </summary>
        public const string NotificationOfUsefulModelRestoration = "VOSTPMP";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о восстановлениии действия патента на промышленный образец
        /// </summary>
        public const string NotificationOfIndustrialRestoration = "VOSTPOP";

        /// <summary>
        /// Уведомление об окончательном отзыве заявки на ПМ из-за неоплаты за выдачу (УВ-2пм(о))
        /// </summary>
        public const string NotificationOfUsefulModelFinalRecall = "UV-2PM(O)";

        /// <summary>
        /// Счет на оплату_ЮР.Л_ИЗ,ПМ
        /// </summary>
        public const string P001_4 = "P001_4";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на промышленный образец
        /// </summary>
        public const string PoPrematureCessationNotification = "PREKR_PO";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на изобретение
        /// </summary>
        public const string IzPrematureCessationNotification = "PREKRPAT";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия  патента на полезную модель
        /// </summary>
        public const string PmPrematureCessationNotification = "PREKRPM";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на селекционное достижение
        /// </summary>
        public const string SdPrematureCessationNotification = "PREKRPAT_SA";

        /// <summary>
        /// Приложение о продлении срока действия ИЗ_ПАТ
        /// </summary>
        public const string InventionExtensionAppendix = "PRIL_IZ_P_PROD";

        /// <summary>
        /// Приложение о продлении срока действия ИЗ_ПАТ_ПОЛ_МОД
        /// </summary>
        public const string UsefulModelExtensionAppendix = "PRIL_IZ_POL_MOD_PROD";

        /// <summary>
        /// Приложение о продлении срока действия ТЗ
        /// </summary>
        public const string TrademarkExtensionAppendix = "PRIL_TZ_PROD";

        /// <summary>
        /// Приложение о продлении срока действия патента на ПО
        /// </summary>
        public const string IndustrialDesignExtensionAppendix = "PRIL_PO_PROD";

        /// <summary>
        /// Приложение о продлении срока действия НМПТ
        /// </summary>
         public const string NmptExtensionAppendix = "PRIL_NMPT_PROD";

        #endregion

        #endregion

        #region Incoming codes

        /// <summary>
        /// Решение уполномоченного органа
        /// </summary>
        public const string DecisionOfAuthorizedBody = "004.1";

        /// <summary>
        /// Ответ на запрос
        /// </summary>
        public const string AnswerToRequest = "001.003";

        /// <summary>
        /// Дополнительные материалы
        /// </summary>
        public const string _001_005 = "001.005";

        /// <summary>
        /// Ходатайтсво о преобразовании ИЗ в ПМ или ПМ в ИЗ
        /// </summary>
        public const string _001_004G_1 = "001.004G_1";

        /// <summary>
        /// Возражения (Поступило возражение на предварительное решение)
        /// </summary>
        public const string Objection = "006.021";

        /// <summary>
        /// Прочие документы
        /// </summary>
        public const string OthersIncoming = "006.02";
        
        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на промышленный образец
        /// </summary>
        public const string PREKR_PO = "PREKR_PO";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на изобретение
        /// </summary>
        public const string PREKRPAT = "PREKRPAT";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия  патента на полезную модель
        /// </summary>
        public const string PREKRPM = "PREKRPM";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о досрочном прекращении действия патента на селекционное достижение
        /// </summary>
        public const string PREKRPAT_SA = "PREKRPAT_SA";

        /// <summary>
        /// Решение Апелляционного совета
        /// </summary>
        public const string DecisionOfAppealsBoard = "004.1.1";

        /// <summary>
        /// Заключение Госкомиссии на наименование
        /// </summary>
        public const string NamingConclusion = "004.1.3";

        /// <summary>
        /// Заключение Госкомиссии на патентоспособность
        /// </summary>
        public const string PatentableConclusion = "004.1.2";

        /// <summary>
        /// Ходатайство о продлении срока ответа на запрос
        /// </summary>
        public const string PetitionForExtendTimeRorResponse = "001.004A_4";

        /// <summary>
        /// Ходатайство о продлении срока
        /// </summary>
        public const string PetitionForExtendTime = "001.004A";

        /// <summary>
        /// Ходатайство о продлении срока подачи возражения 
        /// </summary>
        public const string PetitionForExtendTimeRorObjections = "001.004A_5";

        /// <summary>
        /// Ходатайство о восстановлении пропущенного срока
        /// </summary>
        public const string Petition_001_004G_3 = "001.004G_3";
        
        /// <summary>
        /// Уведомление о восстановлении делопроизводства
        /// </summary>
        public const string OfficeWorkRestartNotification = "POL5";

        /// <summary>
        /// Ходатайство о восстановлении пропущенного срока
        /// </summary>
        public const string PetitionForRestoreTime = "001.004G_3";

        /// <summary>
        /// Ходатайство о восстановлении действия охранного документа
        /// </summary>
        public const string PetitionForPdRestore = "001.004G_3.1";

        /// <summary>
        /// Ходатайство на изготовление заверенной копии
        /// </summary>
        public const string PetitionForCopy = "001.004C1";

        /// <summary>
        /// Ходатайство о согласии заявителя с экспертным заключением
        /// </summary>
        public const string PetitionOfApplicantConsent = "001.004G_4";

        /// <summary>
        /// Ходатайство об отзыве по просьбе заявителя
        /// </summary>
        public const string PetitionOfApplicationRevocation = "001.004G_5";

        /// <summary>
        /// Ходатайство о приостановлении делопроизводства
        /// </summary>
        public const string PetitionForSuspensionOfOfficeWork = "001.004G_6";

        /// <summary>
        /// Выписка из государственного реестра ТЗ РК
        /// </summary>
        public const string ExtractFromStateRegisterOfTrademark = "TZ_GR_VYP";

        /// <summary>
        /// Выписка из государственного реестра СД
        /// </summary>
        public const string ExtractFromStateRegisterOfSelectiveAchievement = "SD_GR_VYP";

        /// <summary>
        /// Выписка из государственного реестра ПО РК
        /// </summary>
        public const string ExtractFromStateRegisterOfIndustrialDesign = "PO_GR_VYP";

        /// <summary>
        /// Выписка из государственного реестра ИЗ РК
        /// </summary>
        public const string ExtractFromStateRegisterOfInvention = "IZ_GR_VYP";

        /// <summary>
        /// Выписка из государственного реестра ПМ РК
        /// </summary>
        public const string ExtractFromStateRegisterOfUsefulModel = "PM_GR_VYP";

        /// <summary>
        /// Выписка из государственного реестра  Общеизвестных ТЗ РК
        /// </summary>
        public const string ExtractFromStateRegisterOfWellKnownTrademark = "TZ_GR_VYP_OBSH";

        /// <summary>
        /// Выписка из государственного реестра НМПТ РК
        /// </summary>
        public const string ExtractFromStateRegisterOfCOO = "TZ_GR_VYP_NMPT";

        /// <summary>
        ///Уведомление о прекращении делопроизводства ПО(отозванные, экспертизы по существу)
        /// </summary>
        public const string UV_PO8 = "UV_PO8";

        /// <summary>
        /// Форма УВО-4
        /// </summary>
        public const string UVO_4 = "UVO-4";

        /// <summary>
        /// Форма УВО-5
        /// </summary>
        public const string UVO_5 = "UVO-5";

        ///// <summary>
        ///// Платежный документ
        ///// </summary>
        //public const string _001_002 = "003.002";

        /// <summary>
        /// Платежный документ
        /// </summary>
        public const string _001_002 = "001.002";

        /// <summary>001.002
        /// Распоряжение по оплатам
        /// </summary>
        public const string _001_002_1 = "001.002.1";

        /// <summary>
        /// Доверенность
        /// </summary>
        public const string IN_001_063 = "001.063";

        /// <summary>
        /// Договор копия
        /// </summary>
        public const string DOG_KOPPI = "DOG_KOPPI";

        /// <summary>
        /// Копия Устава (ТОО, АО)
        /// </summary>
        public const string _001_11 = "001.11";

        /// <summary>
        /// Свидетельство или справка о регистрации юридического лица 
        /// </summary>
        public const string _001_094 = "001.094";

        /// <summary>
        /// Свидетельство о регистрации ИП
        /// </summary>
        public const string _001_094_2 = "001.094.2";

        /// <summary>
        /// --Заявление на договор коммерциализации
        /// </summary>
        public const string _001_006 = "001.006";

        /// <summary>
        /// Заявление на оказание гос.услуги
        /// </summary>
        public const string StateServicesRequest = "006.02.GU";

        /// <summary>
        /// Заявление на оказание гос.услуги
        /// </summary>
        public const string StateServiceRequest = "006.02.GU";

        /// <summary>
        /// Заверенная копия ранее поданной заявки(-ок)
        /// </summary>
        public const string _001_072_1 = "001.072_1";

        /// <summary>
        /// Др. ходатайства
        /// </summary>
        public const string OtherPetitions = "001.004G";

        /// <summary>
        /// Ходатайство о разделении заявки
        /// </summary>
        public const string RequestSplitPetition = "001.004G_2";

        /// <summary>
        /// Форма ММ1
        /// </summary>
        public const string POL_1 = "POL_1";

        /// <summary>
        /// Извещение об оформлении на межд.регистрацию
        /// </summary>
        public const string POL_2 = "POL_2";

        /// <summary>
        /// Ходатайство о прекращении делопроизводства по просьбе заявителя
        /// </summary>
        public const string PetitionForInvalidation = "PetitionForInvalidation";

        /// <summary>
        /// Ходатайство о преобразовании заявки ТЗ в коллективный (и  наоборот)
        /// </summary>
        public const string TzConvertPetition = "001.004G.2";

        #endregion

        #region TZ

        #region OUT

        /// <summary>
        /// Запрос на письмо-согласие соответствующие требованиям
        /// </summary>
        public const string OUT_Zap_Pol_pismo_sogl_v1_19 = "OUT_Zap_Pol_pismo_sogl_v1_19";

        /// <summary>
        /// Запрос по п.2 ст.6 Закона (предварительная)
        /// </summary>
        public const string OUT_Zap_Pred_gos_sim_v1_19 = "OUT_Zap_Pred_gos_sim_v1_19";

        /// <summary>
        /// Запрос по п.2 ст.6 Закона (полная)
        /// </summary>
        public const string OUT_Zap_Pol_gos_sim_v1_19 = "OUT_Zap_Pol_gos_sim_v1_19";

        /// <summary>
        /// Запрос на доверенность (предварительная)
        /// </summary>
        public const string OUT_Zap_Pred_dover_v1_19 = "OUT_Zap_Pred_dover_v1_19";

        /// <summary>
        /// Запрос по заявке на ТЗ
        /// </summary>
        public const string OUT_Zap_Pol_dover_v1_19 = "OUT_Zap_Pol_dover_v1_19";

        /// <summary>
        /// Запрос по пп.3 п.2 ст.7 Закона
        /// </summary>
        public const string OUT_Zap_Pol_avtor_v1_19 = "OUT_Zap_Pol_avtor_v1_19";

        /// <summary>
        /// Запрос по пп.4 п.2 ст.7 Закона
        /// </summary>
        public const string OUT_Zap_Pol_lich_neimush_v1_19 = "OUT_Zap_Pol_lich_neimush_v1_19";

        /// <summary>
        /// Запрос на объективную связь
        /// </summary>
        public const string OUT_Zap_Pol_obyect_sv_v1_19 = "OUT_Zap_Pol_obyect_sv_v1_19";

        /// <summary>
        /// Запрос по п.3 ст.7 Закона
        /// </summary>
        public const string OUT_Zap_Pol_kult_dost_v1_19 = "OUT_Zap_Pol_kult_dost_v1_19";

        /// <summary>
        /// Уведомление о принятии к рассмотрению заявки на регистрацию ТЗ_знака обслуживания
        /// </summary>
        public const string TZPRED1 = "TZPRED1";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (в связи с отсутствием ответа на запрос) (полная)
        /// </summary>
        public const string OUT_Uv_pol_prekr_del_otv_zap_v1_19 = "OUT_Uv_pol_prekr_del_otv_zap_v1_19";

        /// <summary>
        /// Уведомление о регистрации ТЗ (после Аппеляционного совета)
        /// </summary>
        public const string OUT_UV_Pol_Reg_TZ_AP_sov_v1_19 = "OUT_UV_Pol_Reg_TZ_AP_sov_v1_19";

        /// <summary>
        /// Уведомление о прекращении делопроизводства в связи с отсутствием оплаты
        /// </summary>
        public const string OUT_UV_Pred_Prekr_del_bez_opl_v1_19 = "OUT_UV_Pred_Prekr_del_bez_opl_v1_19";

        /// <summary>
        /// Уведомление о продлении срока предоставления ответа на запрос
        /// </summary>
        public const string MTZ_NEW_7 = "MTZ_NEW_7";

        /// <summary>
        /// Уведомление о продлении срока подачи возражения
        /// </summary>
        public const string MTZ_NEW_6 = "MTZ_NEW_6";

        /// <summary>
        /// Уведомление о приостановлении делопроизводства
        /// </summary>
        public const string TZPOL61 = "TZPOL61";

        /// <summary>
        /// Уведомление о преобразовании заявки на ТЗ в заявку на КТЗ (предварительная)
        /// </summary>
        public const string OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19 = "OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в адрес для переписки (предварительная)
        /// </summary>
        public const string OUT_UV_Pred_izmen_adres_v1_19 = "OUT_UV_Pred_izmen_adres_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в наименование и адрес (предварительная)
        /// </summary>
        public const string OUT_UV_Pred_izmen_naim_adr_v1_19 = "OUT_UV_Pred_izmen_naim_adr_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в юридический адрес (предварительная)
        /// </summary>
        public const string OUT_UV_Pred_izmen_yur_adr_v1_19 = "OUT_UV_Pred_izmen_yur_adr_v1_19";

        /// <summary>
        /// Уведомление о не принятии ходатайства во внимание
        /// </summary>
        public const string OUT_UV_Pol_neprin_hod_v1_19 = "OUT_UV_Pol_neprin_hod_v1_19";

        /// <summary>
        /// Уведомление об уступке права на получение свидетельства на ТЗ
        /// </summary>
        public const string TZ_USTUP_PRAVA_ZN = "TZ_USTUP_PRAVA_ZN";

        /// <summary>
        /// Уведомление о непринятии заявления
        /// </summary>
        public const string OUT_UV_GR_neprin_zayav_v1_19 = "OUT_UV_GR_neprin_zayav_v1_19";

        /// <summary>
        /// Решение и окончательное заключение о частичной регистрации (по истечению сроков)
        /// </summary>
        public const string OUT_Resh_pol_Chast_Reg_TZ_istech_srok_v1_19 = "OUT_Resh_pol_Chast_Reg_TZ_istech_srok_v1_19";

        /// <summary>
        /// Уведомление об отказе принятии завления
        /// </summary>
        public const string OUT_UV_GR_Otkaz_zayav = "OUT_UV_GR_Otkaz_zayav";

        #endregion

        #region INT

        /// <summary>
        /// Справка по заявке на ТЗ
        /// </summary>
        public const string TZ_VIPISKA_1 = "TZ-VIPISKA.1";

        /// <summary>
        /// Выписка по заявке на ТЗ
        /// </summary>
        public const string TZ_VIPISKA = "TZ-VIPISKA";

        /// <summary>
        /// Выписка из государственного реестра ТЗ РК
        /// </summary>
        public const string GR_TZ_VYP = "GR_TZ_VYP";

        /// <summary>
        /// Форма отчета эксперта ТЗ и НМПТ(Частичная регистрация)
        /// </summary>
        public const string TZPOL555PR = "TZPOL555PR";

        /// <summary>
        /// Форма отчета эксперта ТЗ и НМПТ(Частичная регистрация с дискламацией)
        /// </summary>
        public const string TZPOL555PRWD = "TZPOL555PRWD";

        /// <summary>
        /// Форма отчета эксперта ТЗ и НМПТ(Предварительный отказ)
        /// </summary>
        public const string TZPOL555PF = "TZPOL555PF";

        /// <summary>
        /// Форма отчета эксперта ТЗ и НМПТ
        /// </summary>
        public const string TZPOL555 = "TZPOL555";

        /// <summary>
        /// Приложение к заявлению
        /// </summary>
        public const string IN001_024_1_1 = "001.024.1.1";

        /// <summary>
        /// Служебная записка
        /// </summary>
        public const string IN001_001_SL = "001.001_SL";

        /// <summary>
        /// Документ об оплате
        /// </summary>
        public const string IN001_032 = "001.032";

        /// <summary>
        /// Реестр оплат
        /// </summary>
        public const string RE_PAYMENT = "RE_PAYMENT";

        /// <summary>
        /// Другие документы
        /// </summary>
        public const string IN001_082 = "001.082";

        /// <summary>
        /// Сопроводительное письмо
        /// </summary>
        public const string IN001_041 = "001.041";

        #endregion

        /// <summary>
        /// Устав
        /// </summary>
        public const string _001_095_1 = "001.095.1";

        #endregion

        /// <summary>
        /// УВЕДОМЛЕНИЕ о преобразовании заявки на товарный знак в заявку на коллективный товарный знак
        /// </summary>
        public const string TmToCtmConversionNotification = "NOT6_NEW";

        /// <summary>
        /// Уведомление об отказе в принятии заявления
        /// </summary>
        public const string RequestRejectionNotification = "NOT5_NEW";

        /// <summary>
        /// Уведомление о внесении изменений в соответствии с объемом внесенных изменений
        /// </summary>
        public const string FullChangeNotification = "NOT9_NEW";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о преобразовании заявки на коллективный товарный знак в заявку на товарный знак
        /// </summary>
        public const string CtmToTmConversionNotification = "NOT7_NEW";

        /// <summary>
        /// Уведомление о том, что ходатайство не принято во внимание в связи с неуплатой
        /// </summary>
        public const string PetitionRejectedDueToLackOfPaymentNotification = "NOT8_NEW";

        /// <summary>
        /// РЕШЕНИЕ о регистрации товарного знака (знаков обслуживания)
        /// </summary>
        public const string TrademarkRegistrationDecision = "TZPOL3-11";

        /// <summary>
        /// Решение об отказе в регистрации ТЗ
        /// </summary>
        public const string TrademarkRegistrationRejectionDecision = "TZPOL0112";

        /// <summary>
        /// Решение о частичной регистрации ТЗ
        /// </summary>
        public const string TrademarkPartialRegistrationDecision = "TZPOL3111";

        /// <summary>
        /// Внутренняя опись входящей корреспонденции
        /// </summary>
        public const string IN01_vn_opis_V1_19 = "IN01_vn_opis_V1_19";

        /// <summary>
        /// Заявление о выдаче охранного документа
        /// </summary>
        public const string ApplicationIssuingSecurityDocument = "AppIssuingSecDoc";

        /// <summary>
        /// Уведомление о положительном результате ФЭ (без оплаты за экспертизы по существу) (каз) 
        /// </summary>
        public const string NotificationPositiveResultFEWithoutPayingKz = "NotifPositeResultFENoPayKz";

        /// <summary>
        /// Уведомление о регистрации ДК (ТЗ)
        /// </summary>
        public const string DK_UVED_POL_TZ = "DK_UVED_POL_TZ";

        /// <summary>
        /// Уведомление об отказе в регистрации ДК (ТЗ)
        /// </summary>
        public const string NoticeRefusalRegisterContract = "DK_UVED_OTKAZ_TZ";

        /// <summary>
        /// УВЕДОМЛЕНИЕ об аннулировании договора
        /// </summary>
        public const string CancellationNotice = "DK_UVED_CANCELLATION";

        /// <summary>
        /// УВЕДОМЛЕНИЕ о расторжении договора
        /// </summary>
        public const string TerminationNotice = "DK_UVED_RASTORZHENIE";

        /// <summary>
        /// 101_Запрос заявителю ДК
        /// </summary>
        public const string DK_ZAPROS = "DK_ZAPROS";

        /// <summary>
        /// Реестр для передачи сведений по договорам в Управление государственных реестров
        /// </summary>
        public const string RegisterTransferInformationContracts = "Register_Transfer_Information_Contracts";

        /// <summary>
        /// Чертежи, фото, рисунки
        /// </summary>
        public const string SetOfImagesOfTheProductOrDrawingAndOtherMaterials = "001.052";

        /// <summary>
        /// Анкета СД
        /// </summary>
        public const string SAQuestionnaire = "001.021";

        /// <summary>
        /// Негативы или цветные слайды
        /// </summary>
        public const string SANegativesOrColorSlides = "001.043.1";

        /// <summary>
        /// Информация о раннее произведенной продаже
        /// </summary>
        public const string SAInformationAboutPreviouslyMadeSale = "001.043.2";

        /// <summary>
        /// Выписка из банка.
        /// </summary>
        public const string StatementFromBank = "STATEMENT_FROM_BANK";

        /// <summary>
        /// Счет на оплату.
        /// </summary>
        public const string OUT_Bill_V1_19 = "OUT_Bill_V1_19";

        /// <summary>
        /// Ответ на входящее письмо.
        /// </summary>
        public const string OUT_Ovt_na_Pismo_V1_19 = "OUT_Ovt_na_Pismo_V1_19";

        /// <summary>
        /// Решение о частичной регистрации ТЗ (после согласия заявителя).
        /// </summary>
        public const string OUT_Resh_Pol_chast_REG_TZ_Sogl_zayav_V1_19 = "OUT_Resh_Pol_chast_REG_TZ_Sogl_zayav_V1_19";

        /// <summary>
        /// Решение и окончательное заключение о частичной регистрации (после возражения).
        /// </summary>
        public const string OUT_Resh_pol_Chast_Reg_TZ_vozr_v1_19 = "OUT_Resh_pol_Chast_Reg_TZ_vozr_v1_19";

        /// <summary>
        /// Решение и окончательное заключение об отказе регистрации ТЗ (по истечению сроков).
        /// </summary>
        public const string OUT_Resh_pol_Otkaz_Reg_TZ_istech_srok_v1_19 = "OUT_Resh_pol_Otkaz_Reg_TZ_istech_srok_v1_19";

        /// <summary>
        /// Решение и окончательное заключение об отказе регистрации ТЗ (после возражения).
        /// </summary>
        public const string OUT_Resh_pol_Otkaz_Reg_TZ_vozr_v1_19 = "OUT_Resh_pol_Otkaz_Reg_TZ_vozr_v1_19";

        /// <summary>
        /// Решение и окончательное заключение о регистрации ТЗ (после возражения).
        /// </summary>
        public const string OUT_Resh_pol_Reg_TZ_vozr_v1_19 = "OUT_Resh_pol_Reg_TZ_vozr_v1_19";

        /// <summary>
        /// Уведомление о продлении срока подачи возражения.
        /// </summary>
        public const string OUT_UV_POL_prodl_srok_vozr_v1_19 = "OUT_UV_POL_prodl_srok_vozr_v1_19";
        
        /// <summary>
        /// Уведомление о внесении изменений в наименование и адрес (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_naim_adr_v1_19 = "OUT_UV_Pol_izmen_naim_adr_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в наименование (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_naim_v1_19 = "OUT_UV_Pol_izmen_naim_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в обозначение (полная)
        /// </summary>
        public const string OUT_UV_Pol_izmen_obozn_v1_19 = "OUT_UV_Pol_izmen_obozn_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в перечень (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_perech_v1_19 = "OUT_UV_Pol_izmen_perech_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в представителя (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_predstav_v1_19 = "OUT_UV_Pol_izmen_predstav_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в юридический адрес (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_yur_adr_v1_19 = "OUT_UV_Pol_izmen_yur_adr_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в юридический адрес и адрес для переписки (полная).
        /// </summary>
        public const string OUT_UV_Pol_izmen_yur_perep_adr_v1_19 = "OUT_UV_Pol_izmen_yur_perep_adr_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в наименование (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_izmen_naim_v1_19 = "OUT_UV_Pred_izmen_naim_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в обозначение (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_izmen_obozn_v1_19 = "OUT_UV_Pred_izmen_obozn_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в перечень (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_izmen_perech_v1_19 = "OUT_UV_Pred_izmen_perech_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в представителя (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_izmen_predstav_v1_19 = "OUT_UV_Pred_izmen_predstav_v1_19";

        /// <summary>
        /// Уведомление о внесении изменений в юридический адрес и адрес для переписки (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_izmen_yur_perep_adr_v1_19 = "OUT_UV_Pred_izmen_yur_perep_adr_v1_19";

        /// <summary>
        /// Уведомление о прекращении делопроизводства  (в связи с неуплатой за полную экспертизу).
        /// </summary>
        public const string OUT_Uv_pred_prekr_del_net_opl_exp_v1_19 = "OUT_Uv_pred_prekr_del_net_opl_exp_v1_19";


        /// <summary>
        /// Уведомление о прекращении делопроизводства (в связи с отсутствием ответа на запрос) (предварительная).
        /// </summary>
        public const string OUT_Uv_pred_prekr_del_otv_zap_v1_19 = "OUT_Uv_pred_prekr_del_otv_zap_v1_19";

        /// <summary>
        /// Уведомление о прекращении делопроизводства (по просьбе заявителя) (предварительная).
        /// </summary>
        public const string OUT_Uv_pred_prekr_del_prosb_zayav_v1_19 = "OUT_Uv_pred_prekr_del_prosb_zayav_v1_19";

        /// <summary>
        /// Уведомление о преобразовании заявки на КТЗ в заявку на ТЗ (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_preobr_zayav_KTZ_na_TZ_v1_19 = "OUT_UV_Pred_preobr_zayav_KTZ_na_TZ_v1_19";

        /// <summary>
        /// Уведомление о продлении срока предоставления ответа на запрос (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_prodl_srok_na_otv_v1_19 = "OUT_UV_Pred_prodl_srok_na_otv_v1_19";
        
        /// <summary>
        /// Уведомление об уступке права на получение охранного документа на товарный знак (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_ustup_v1_19 = "OUT_UV_Pred_ustup_v1_19";

        /// <summary>
        /// Уведомление о востановлении делопроизводства (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_vost_del_v1_19 = "OUT_UV_Pred_vost_del_v1_19";

        /// <summary>
        /// Уведомление о выделении заявки на ТЗ (предварительная).
        /// </summary>
        public const string OUT_UV_Pred_vyd_zayavTZ_v1_19 = "OUT_UV_Pred_vyd_zayavTZ_v1_19";

        /// <summary>
        /// Заключение о предварительной частичной регистрации ТЗ.
        /// </summary>
        public const string OUT_Zakl_Pol_Predv_otkaz_Chast_Reg_TZ_V1_19 = "OUT_Zakl_Pol_Predv_otkaz_Chast_Reg_TZ_V1_19";

        /// <summary>
        /// Заключение о предварительном отказе в регистрации ТЗ.
        /// </summary>
        public const string OUT_Zakl_Pol_Predv_otkaz_Reg_TZ_V1_19 = "OUT_Zakl_Pol_Predv_otkaz_Reg_TZ_V1_19";

        /// <summary>
        /// Решение и Заключение о выдаче патента на изобретение.
        /// </summary>
        public const string DecisionConclusionGrantPatent = "IZ-3B";

        /// <summary>
        /// Отчет о поиске ПО
        /// </summary>
        public const string PO10 = "PO10";

        /// <summary>
        /// Решение о дальнейшем рассмотрении заявки на селекционное достижение
        /// </summary>
        public const string S12 = "S12";
    }
}
