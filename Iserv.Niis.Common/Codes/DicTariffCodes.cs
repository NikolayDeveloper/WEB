using System;
using System.Collections.Generic;
using System.Text;

namespace Iserv.Niis.Common.Codes
{
    public class DicTariffCodes
    {
        /// <summary>
        /// Прием и проведение формальной экспертизы заявок по товарным знакам, знакам обслуживания и мест происхождения товаров  на бумажном носителе
        /// </summary>
        public const string TmNmptFormalExpertizeDigital = "140";
        /// <summary>
        /// Прием и проведение формальной экспертизы заявок по товарным знакам, знакам обслуживания и мест происхождения товаров  при электронном приеме заявки
        /// </summary>
        public const string TmNmptFormalExpertizePaper = "140.1";
        /// <summary>
        /// Прием и проведение формальной экспертизы заявки на регистрацию коллективного товарного знака при приеме заявки на бумажном носителе
        /// </summary>
        public const string CollectiveTmFormalExpertizePaper = "141";
        /// <summary>
        /// Прием и проведение формальной экспертизы заявки на регистрацию коллективного товарного знака при электронном приеме заявки
        /// </summary>
        public const string CollectiveTmFormalExpertizeDigital = "141.1";
        /// <summary>
        /// Прием заявки и проведение предварительной экспертизы на регистрацию товарного знака, знака обслуживания дополнительно за каждый класс свыше 3-х классов МКТУ на бумажном носителе
        /// </summary>
        public const string TmNmptFormalExpertizeMoreThanThreeIcgsClassesPaper = "142";
        /// <summary>
        /// Прием заявки и проведение предварительной экспертизы на регистрацию товарного знака , знака обслуживания дополнительно за каждый класс свыше 3х классов МКТУ при электронной подаче
        /// </summary>
        public const string TmNmptFormalExpertizeMoreThanThreeIcgsClassesDigital = "142.1";
        /// <summary>
        /// Прием заявки на регистрацию и проведение предварительной экспертизы коллективного товарного знака дополнительно за каждый класс свыше 3-х классов МКТУ на бумажном носителе
        /// </summary>
        public const string CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesPaper = "143";
        /// <summary>
        /// Прием заявки на регистрацию и проведение предварительной экспертизы коллективного товарного знака дополнительно за каждый класс свыше 3-х классов МКТУ при электронной подаче
        /// </summary>
        public const string CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesDigital = "143.1";
        /// <summary>
        /// Проведение полной экспертизы заявки  на регистрацию коллективного  товарного знака  до 3-х классов МКТУ на бумажном носителе
        /// </summary>
        public const string CollectiveTmNmptFullExpertizePaper = "144";
        /// <summary>
        /// Проведение полной экспертизы заявки  на регистрацию коллективного  товарного знака  до 3-х классов МКТУ при электронной подаче
        /// </summary>
        public const string CollectiveTmNmptFullExpertizeDigital = "144.1";
        /// <summary>
        /// За проведение полной экспертизы на регистрацию товарного знака до 3-х классов МКТУ на бумажном носителе  
        /// </summary>
        public const string TmNmptFullExpertizePaper = "145";
        /// <summary>
        /// За проведение полной экспертизы на регистрацию товарного знака до 3-х классов МКТУ при электронной подаче
        /// </summary>
        public const string TmNmptFullExpertizeDigital = "145.1";
        /// <summary>
        /// За проведение полной экспертизы на регистрацию товарного знака дополнительно за каждый класс свыше трех на бумажном носителе
        /// </summary>
        public const string TmNmptFullExpertizeMoreThanThreeIcgsClassesPaper = "146";
        /// <summary>
        /// За проведение полной экспертизы на регистрацию товарного знака дополнительно за каждый класс свыше трех при электронной подаче
        /// </summary>
        public const string TmNmptFullExpertizeMoreThanThreeIcgsClassesDigital = "146.1";
        /// <summary>
        /// Проведение полной экспертизы заявки  на регистрацию коллективного  товарного знака дополнительно за каждый класс свыше трех при электронной подаче
        /// </summary>
        public const string CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesPaper = "146.2";
        /// <summary>
        /// Проведение полной экспертизы заявки  на регистрацию коллективного  товарного знака дополнительно за каждый класс свыше трех на бумажном носителе
        /// </summary>
        public const string CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesDigital = "146.3";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (первый месяц)
        /// </summary>
        public const string ResponseTimeProlongationFirstMonth = "551";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (второй месяц)
        /// </summary>
        public const string ResponseTimeProlongationSecondMonth = "552";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (третий месяц)
        /// </summary>
        public const string ResponseTimeProlongationThirdMonth = "553";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (четвертый месяц)
        /// </summary>
        public const string ResponseTimeProlongationFourthMonth = "554";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (пятый месяц)
        /// </summary>
        public const string ResponseTimeProlongationFifthMonth = "555";
        /// <summary>
        /// Продление срока ответа на запрос за каждый месяц (шестой месяц)
        /// </summary>
        public const string ResponseTimeProlongationSixthMonth = "556";
        /// <summary>
        /// Восстановление пропущенного срока ответа на запрос, оплаты, подачи возражении заявителем
        /// </summary>
        public const string TimeRestore = "651";
        /// <summary>
        /// Рассмотрение возражений на решение о предварительном отказе в регистрации
        /// </summary>
        public const string PreliminaryRejectionObjectionConsideration = "400";
        /// <summary>
        /// регистрация товарных знаков, знаков обслуживания и наименования мест происхождения товаров и публикация сведений о регистрации
        /// </summary>
        public const string TrademarNmptRegistrationAndPublishing = "731";
        /// <summary>
        /// Проведение работ по публикации в Государственном реестре сведений о регистрации и о выдаче охранного документа на изобретение, полезную модель, промышленный образец этот код зависит от категории заявителя - ЮЛ, ФЛ, МСБ, ДР
        /// </summary>
        public const string BS2URegistrationAndPublishing = "701";
        /// <summary>
        /// Преобразование заявки на товарный знак в коллективный товарный знак и наоборот
        /// </summary>
        public const string TmConvert = "430";
        /// <summary>
        /// Разделение заявки на товарный знак по классам по инициативе заявителя
        /// </summary>
        public const string TmSplit = "440";
        /// <summary>
        /// Внесение изменений и исправлений в материалы заявки
        /// </summary>
        public const string TmChange = "2222";
        /// <summary>
        /// за внесение однотипных изменений в материалы заявки
        /// </summary>
        public const string TmSameTypeChange = "2223";
        /// <summary>
        /// Продление и восстановление сроков представления ответа на запрос экспертизы и оплаты, в том числе к экспертизе договоров
        /// </summary>
        public const string BRestoreAndProlongationTerm = "30";

        /// <summary>
        /// Продление сроков представления запрашиваемых документов за каждый месяц до двенадцати  месяцев с даты истечения установленного срока
        /// </summary>
        public const string BProlongationRequestDocumentsTerm = "29";

        /// <summary>
        /// Прием заявок и проведение формальной экспертизы на изобретение (при приеме заявки на бумажном носителе)
        /// </summary>
        public const string UsefullModelPatentExaminationOnPurpose = "120";

        /// <summary>
        /// Прием заявок и проведение формальной экспертизы на изобретение (при электронном приеме заявки)
        /// </summary>
        public const string UsefullModelPatentExaminationEmail = "121";
    }
}
