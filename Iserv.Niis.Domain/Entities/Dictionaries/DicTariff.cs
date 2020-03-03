using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.EntitiesHistory.References;
using Iserv.Niis.Domain.Enums;
using Newtonsoft.Json;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Справочник Тариф (Коды оплат)
    /// </summary>
    [JsonObject(IsReference = true)] 
    public class DicTariff :  DictionaryEntity<int>, IReference, IHistorySupport, IHaveConcurrencyToken, IHaveLongName
    {
        public decimal? Price { get; set; }
        public string Limit { get; set; }

        /// <summary>
        /// Группа (Бумажный/Эллектронный)
        /// </summary>
        public ReceiveTypeGroupEnum ReceiveTypeGroup { get; set; }
        
        /// <summary>
        /// Является ли для количества классо МКТУ более поргового значения
        /// </summary>
        public bool? IsMoreThanIcgsThreshold { get; set; }
        /// <summary>
        /// Является ли для Коллективного товарного знака
        /// </summary>
        public bool? IsCtm { get; set; }

        public int? MaintenanceYears { get; set; }
        public int? ProtectionDocSupportYearsFrom { get; set; }
        public int? ProtectionDocSupportYearsUntil { get; set; }
        public bool? IsProtectionDocSupportDateExpired { get; set; }

        #region Relationships

        /// <summary>
        /// Свзят старифа и типа объекта, один ко многим
        /// </summary>
        [JsonIgnore]
        public ICollection<DicTariffProtectionDocType> TariffProtectionDocTypes { get; set; }

        public int? NiisTariffId { get; set; }
        public IntegrationNiisRefTariff NiisTariff { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(RefTariffHistory);
        }

        public static class Codes
        {
            public enum CodesEnum
            {
                None,
            }

            public static string GetCode(CodesEnum code)
            {
                switch (code)
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(code), code, null);
                }
            }

            #region Public codes

            /// <summary>
            /// Прием заявок и проведение формальной экспертизы на изобретение (при приеме заявки на бумажном носителе)
            /// </summary>
            public const string InventionFormalExaminationOnPurpose = "100";

            /// <summary>
            /// Прием заявок и проведение формальной экспертизы на изобретение (при электронном приеме заявки)
            /// </summary>
            public const string InventionFormalExaminationEmail = "101";

            /// <summary>
            /// Прием заявок и ускоренное проведение формальной экспертизы на изобретение по перечню установленного уполномоченным органом 
            /// </summary>
            public const string InventionAcceleratedFormalExaminationOnPurpose = "111";

            /// <summary>
            /// Прием заявок и ускоренное проведение формальной экспертизы на изобретение по перечню установленного уполномоченным органом 
            /// </summary>
            public const string InventionAcceleratedFormalExaminationEmail = "112";

            /// <summary>
            /// Экспертиза по существу заявки на изобретение
            /// </summary>
            public const string ExaminationOfApplicationForInventionMerits  = "300";

            /// <summary>
            /// Экспертиза по существу дополнительно за каждый независимый пункт формулы свыше одного 
            /// </summary>
            public const string ExaminationEssentiallyAdditionallyIndependentClaimOverOne = "301";

            /// <summary>
            /// Проведение ускоренной экспертизы по существу заявки на изобретение
            /// </summary>
            public const string AcceleratedExaminationOfApplicationForInventionMerits = "302";

            /// <summary>
            /// Экспертиза по существу заявки на изобретение, при наличии отчета о международном поиске или заключения предварительной экспертизы, подготовленных одним из международных органов в соответствии с международными соглашениями, участником которых является Республика Казахстан(дополнительно за каждый независимый пункт формулы свыше одного оплата уменьшается на 20%)
            /// </summary>
            public const string ExaminationOfApplicationForInventionMeritsWithInternationalReport = "NEW_009";

            /// <summary>
            /// Экспертиза по существу заявки на изобретение, при наличии в заявке отчета об информационном поиске(дополнительно за каждый независимый пункт формулы свыше одного оплата уменьшается на 20%)
            /// </summary>
            public const string ExaminationOfApplicationForInventionMeritsWithSearchReport = "12";

			/// <summary>
			/// Прием заявок и проведение экспертизы на полезную модель (при приеме заявки на бумажном носителе)
			/// </summary>
			public const string InventionExaminationOnUsefulModelOnPaper = "NEW_011";

			/// <summary>
			/// Прием заявок и проведение экспертизы на полезную модель(при электронном приеме заявки)
			/// </summary>
			public const string InventionExaminationOnUsefulModelOnOnline = "NEW_012";

            /// <summary>
            /// Восстановление срока действия охранного документа и публикация сведений о восстановлении срока действия охранного документа
            /// </summary>
            public const string ProtectionDocRestore = "NEW_027";

            /// <summary>
            /// Восстановление срока действия охранного документа и публикация сведений
            /// </summary>
            public const string ProtectionDocRestoreSelectiveAchievement = "NEW_059";

            /// <summary>
            /// Проведение экспертизы по существу заявки на промышленный образец
            /// </summary>
            public const string NEW_015 = "NEW_015";

            /// <summary>
            /// Проведение экспертизы по существу дополнительно за каждый промышленный образец свыше одного
            /// </summary>
            public const string NEW_016 = "NEW_016";

            /// <summary>
            /// Внесение изменений в материалы заявки
            /// </summary>
            public const string NEW_056 = "NEW_056";

            /// <summary>
            /// Внесение однотипных изменений в материалы заявки по инициативе заявителя
            /// </summary>
            public const string NEW_018 = "NEW_018";

            /// <summary>
            /// Преобразование заявки на изобретение и/или на полезную модель
            /// </summary>
            public const string NEW_019 = "NEW_019";

            /// <summary>
            /// Преобразование заявки на товарный знак в коллективный товарный знак и наоборот
            /// </summary>
            public const string NEW_080 = "NEW_080";

            /// <summary>
            /// Внесение изменений в материалы заявки по инициативе заявителя
            /// </summary>
            public const string NEW_017 = "NEW_017";

            /// <summary>
            /// Подготовка перечня товаров и услуг в соответствии с международной классификацией товаров и услуг
            /// </summary>
            public const string InternationalServiceListPreparation = "NEW_100";

            /// <summary>
            /// Подготовка документов к выдаче охранного документа и удостоверения автора,  публикация сведений о выдаче охранного документа
            /// </summary>
            public const string PreparationOfDocumentsForIssuanceOfPatent= "9907";

            /// <summary>
            /// Прием и проведение формальной экспертизы заявок по товарным знакам, знакам обслуживания (нарочно)
            /// </summary>
            public const string TmFormalExaminationOnPurpose = "NEW_072";

            /// <summary>
            /// Прием и проведение формальной экспертизы заявок по товарным знакам, знакам обслуживания (e-mail)
            /// </summary>
            public const string TmFormalExaminationEmail = "NEW_073";

            /// <summary>
            /// Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания
            /// </summary>
            public const string TmRequestExamination = "NEW_076";

            /// <summary>
            /// проведение экспертизы заявки  на регистрацию товарных знаков, знаков обслуживания дополнительно за каждый класс свыше трех 
            /// </summary>
            public const string TmRequestExaminationWithThreeOrMoreClass = "NEW_077";

            /// <summary>
            /// регистрация товарных знаков, знаков обслуживания и наименования мест происхождения товаров и публикация сведений о регистрации
            /// </summary>
            public const string TmNmptRegistration = "NEW_081";

            /// <summary>
            /// Госпошлина 2018
            /// </summary>
            public const string StateFee2018 = "NEW_GOS_18";

            /// <summary>
            /// Продление срока ответа на запрос за каждый месяц
            /// </summary>
            public const string RequestAnswerTimeExtensionForMonth = "NEW_087";

            /// <summary>
            /// Восстановление пропущенного срока ответа на запрос, оплаты, подачи возражения заявителем (п.3 ст.15 Закона)
            /// </summary>
            public const string RequestAnswerMissedTimeRestoration = "NEW_091";

            /// <summary>
            /// Продление и восстановление сроков представления ответа на запрос экспертизы и оплаты
            /// </summary>
            public const string ExtensionAndRestorationTimesForAnswerToExaminationRequest = "NEW_166";

            /// <summary>
            /// За восстановление пропущенного срока оплаты за выдачу охранного документа заявителем до шести месяцев с даты истечения установленного срока
            /// </summary>
            public const string NEW_058 = "NEW_058";

            /// <summary>
            /// Продление срока подачи возражения на решение экспертизы за каждый месяц
            /// </summary>
            public const string ExpertiseConclusionObjectionTermExtensionMonthly = "NEW_089";

            /// <summary>
            /// Продление сроков представления запрашиваемых документов за каждый месяц до двенадцати  месяцев с даты истечения установленного срока
            /// </summary>
            public const string NEW_032 = "NEW_032";

            /// <summary>
            /// Восстановление сроков представления ответа на запрос экспертизы, перевода и оплаты, в том  числе  к экспертизе договоров
            /// </summary>
            public const string NEW_033 = "NEW_033";

            /// <summary>
            /// Проведение экспертизы договора об уступке охранных документов и публикация сведений о регистрации договора (с учетом пункта 34)
            /// </summary>
            public const string NEW_034 = "NEW_034";

            /// <summary>
            /// Проведение экспертизы лицензионного (сублицензионного) договора, договора залога, прием заявки на регистрацию экспертизы договора  о предоставлении комплексной предпринимательской лицензии в отношении одного объекта промышленной собственности, публикация сведений о регистрации договора (с учетом пункта 34)
            /// </summary>
            public const string NEW_035 = "NEW_035";

            /// <summary>
            /// Проведение экспертизы договора об уступке патента и публикация сведений о его регистрации
            /// </summary>
            public const string NEW_064 = "NEW_064";

            /// <summary>
            /// Проведение экспертизы лицензионного (сублицензионного) договора, договора залога и публикация сведений о регистрации договора
            /// </summary>
            public const string NEW_065 = "NEW_065";

            /// <summary>
            /// Прием заявки на предоставление открытой лицензии
            /// </summary>
            public const string NEW_037 = "NEW_037";

            /// <summary>
            /// Прием заявки на предоставление открытой лицензии (селекционные достижения)
            /// </summary>
            public const string NEW_066 = "NEW_066";

            /// <summary>
            /// Проведение экспертизы дополнительного соглашения к договору и публикация сведений о его регистрации (селекционные достижения)
            /// </summary>
            public const string NEW_067 = "NEW_067";

            /// <summary>
            /// Проведение экспертизы материалов заявки на регистрацию наименования мест происхождения товаров  и/или предоставления права пользования наименования мест происхождения товаров
            /// </summary>
            public const string NEW_078 = "NEW_078";

            /// <summary>
            /// Разделение заявки на товарный знак по классам по инициативе заявителя
            /// </summary>
            public const string SplitRequest = "NEW_079";
            
            /// <summary>
            /// Проведение экспертизы дополнительного соглашения к договору и публикация сведений о его регистрации
            /// </summary>
            public const string NEW_104 = "NEW_104";

            /// <summary>
            /// проведение экспертизы договоров о передаче прав на товарный знак,  знак обслуживания ,лицензионных (сублицензионных) договоров, договора залога, договора о предоставлении комплексной предпринимательской лицензии в отношении одного объекта промышленной собственности
            /// </summary>
            public const string NEW_102 = "NEW_102";

            /// <summary>
            /// в отношении группы объектов промышленной собственности, дополнительно за каждый объект свыше одного объекта промышленной собственности
            /// </summary>
            public const string NEW_103 = "NEW_103";

            /// <summary>
            /// В отношении группы объектов промышленной собственности, дополнительно за каждый объект свыше одного объекта промышленной собственности (применяется к пунктам 31, 33)
            /// </summary>
            public const string NEW_036 = "NEW_036";

            /// <summary>
            /// Прием заявок и проведение формальной экспертизы на промышленный образец (при приеме заявки на бумажном носителе)
            /// </summary>
            public const string IndustrialDesignExaminationOnPurpose = "NEW_013";

            /// <summary>
            /// Прием заявок и проведение формальной экспертизы на промышленный образец (при электронном приеме заявки)
            /// </summary>
            public const string IndustrialDesignExaminationEmail = "NEW_014";

            /// <summary>
            /// Прием заявок и проведение предварительной экспертизы заявок на селекционное достижение (при приеме заявки на бумажном носителе)
            /// </summary>
            public const string SelectiveAchievementsExaminationPaper = "NEW_054";

            /// <summary>
            /// Прием заявок и проведение предварительной экспертизы заявок на селекционное достижение (при электронном приеме заявки)
            /// </summary>
            public const string SelectiveAchievementsExaminationDigital = "NEW_055";

            /// <summary>
            /// Подготовка документов к выдаче охранного документа и удостоверения автора, публикация сведений о выдаче охранного документа(для юридических лиц)
            /// </summary>
            public const string IndustrialDesign_9907 = "9907";
            /// <summary>
            /// Подготовка документов к выдаче охранного документа и удостоверения автора, публикация сведений о выдаче охранного документа(для субъектов малого и среднего бизнеса - резидентов)
            /// </summary>
            public const string IndustrialDesign_9907_1 = "9907.1";
            /// <summary>
            /// Подготовка документов к выдаче охранного документа и удостоверения автора, публикация сведений о выдаче охранного документа(для физических лиц)
            /// </summary>
            public const string IndustrialDesign_9907_2 = "9907.2";
            /// <summary>
            /// Подготовка документов к выдаче охранного документа и удостоверения автора, публикация сведений о выдаче охранного документа(для участников Великой отечественной войны, инвалидов, учащихся общеобразовательных школ и колледжей, студентов высших учебных заведений)
            /// </summary>
            public const string IndustrialDesign_9907_3 = "9907.3";
            /// <summary>
            /// За поддержание патента на изобретение (Первый-Третий)
            /// </summary>
            public const string FirstToThirdSupportYearInvention = "NEW_108";
            /// <summary>
            /// За поддержание патента на полезную модель (Первый-Третий)
            /// </summary>
            public const string FirstToThirdSupportYearUsefulModel = "NEW_124";
            /// <summary>
            /// За поддержание патента на промышленный образец (Первый-Третий)
            /// </summary>
            public const string FirstToThirdSupportYearIndustrialDesign = "NEW_128";
            /// <summary>
            /// За поддержание патента на селекционное достижение (Первый-Третий)
            /// </summary>
            public const string FirstToThirdSupportYearSelectiveAchievement = "NEW_142";
            /// <summary>
            /// За поддержание патента на изобретение (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)
            /// </summary>
            public const string FirstToThirdSupportYearExpiredInvention = "NEW_116";
            /// <summary>
            /// За поддержание патента на полезную модель (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)
            /// </summary>
            public const string FirstToThirdSupportYearExpiredUsefulModel = "NEW_126";
            /// <summary>
            /// За поддержание патента на промышленный образец (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)
            /// </summary>
            public const string FirstToThirdSupportYearExpiredIndustrialDesign = "NEW_135";
            /// <summary>
            /// За поддержание патента на селекционное достижение (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)
            /// </summary>
            public const string FirstToThirdSupportYearExpiredSelectiveAchievement = "NEW_154";
            /// <summary>
            /// Внесение изменений в охранный документ, государственные реестры изобретений, полезных моделей, промышленных образцов
            /// </summary>
            public const string UmImIzProtectionDocChange = "NEW_022";
            /// <summary>
            /// Внесение однотипных изменений в охранный документ, государственные реестры изобретений, полезных моделей, промышленных образцов
            /// </summary>
            public const string UmImIzProtectionDocSameTypeChange = "NEW_022";
            /// <summary>
            /// Внесение изменений в охранный документ и государственные реестры по охраняемым сортам растений и пород животных
            /// </summary>
            public const string SelectiveAchevementsProtectionDocChange = "NEW_061";
            /// <summary>
            /// Внесение изменений в Государственные реестры охраняемых товарных знаков, знаков обслуживания и наименований мест происхождения товаров
            /// </summary>
            public const string TmNmptProtectionDocChange = "NEW_082";
            /// <summary>
            /// Внесение однотипных изменений в Государственный реестр охраняемых товарных знаков, знаков обслуживания и наименований мест происхождения товаров
            /// </summary>
            public const string TmNmptProtectionDocSameTypeChange = "NEW_083";
            /// <summary>
            /// Продление срока действия регистрации права пользования наименованием места происхождения товара, регистрации товарного знака, знака обслуживания и  публикация сведений о продлении (п.2 ст.15 Закона)
            /// </summary>
            public const string NmptTzExtension = "NEW_085";
            /// <summary>
            /// Продление срока действия охранного документа и публикация сведений о продлении охранного документа
            /// </summary>
            public const string SaExtension = "NEW_063";
            /// <summary>
            /// Продление срока действия охранного документа и публикация сведений о продлении
            /// </summary>
            public const string InvExtension = "NEW_026";
            /// <summary>
            /// Продление срока действия охранного документа и публикация сведений о продлении
            /// </summary>
            public const string ImExtension = "NEW_025";
            /// <summary>
            /// госпошлина за выдачу охранного документа
            /// </summary>
            public const string _2_2018 = "2_2018";
            /// <summary>
            /// госпошлина по договорам коммерциализации
            /// </summary>
            public const string _3_2018 = "3_2018";

            /// <summary>
            /// Прием заявок с испрашиванием конвенционного приоритета после установленного срока (для субъектов малого и среднего бизнеса - резидентов)
            /// </summary>
            public const string AcceptanceApplicationsConventionalPriorityAafterDeadline = "130";

            /// <summary>
            /// Проведение экспертизы лицензионного (сублицензионного) договора, договора залога, прием заявки на регистрацию экспертизы договора о предоставлении комплексной предпринимательской лицензии в отношении одного и группы объектов промышленной собственности, публикация сведений о регистрации договоров
            /// </summary>
            public const string _1030 = "1030";

            /// <summary>
            /// В отношении группы объектов промышленной собственности, дополнительно за каждый объект свыше одного объекта промышленной собственности
            /// </summary>
            public const string _34 = "34";

            #endregion

            //todo вынести по константам
            public static List<string> GetProtectionDocSupportCodes()
            {
                var result = new List<string>();
                for (int i = 108; i < 166; i++)
                {
                    result.Add("NEW_" + i);
                }
                return result;
            }
        }
    }
}