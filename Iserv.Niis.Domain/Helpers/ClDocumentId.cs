namespace Iserv.Niis.Domain.Helpers
{
    /// <summary>
    /// Типы заявок
    /// </summary>
    public static class ClDocumentId
    {
        #region Request

        /// <summary>
        /// Заявка на ТЗ
        /// </summary>
        public const int Trademark = 411;

        /// <summary>
        /// Заявка на признание ТЗ общеизвестным
        /// </summary>
        public const int TrademarkGenerallyKnown = 4131;

        /// <summary>
        /// Заявка на НМПТ
        /// </summary>
        public const int NameOfOrigin = 1611;

        /// <summary>
        /// Заявка на МТЗ
        /// </summary>
        public const int InternationalTrademark = 2631;

        /// <summary>
        /// Заявка на ИЗ
        /// </summary>
        public const int Invention = 452;

        /// <summary>
        /// Заявка на ПМ
        /// </summary>
        public const int UsefulModel = 437;

        /// <summary>
        /// Заявка на патент ПО
        /// </summary>
        public const int IndustrialSample = 269;

        /// <summary>
        /// Заявка на СД
        /// </summary>
        public const int SelectionAchieve = 462;

        /// <summary>
        /// Заявки по АП
        /// </summary>
        public const int Copyright = 4101;

        /// <summary>
        /// Заявка на ИЗ_ЕАПО
        /// </summary>
        public const int Eapo = 4137;

        /// <summary>
        /// Заявка на ИЗ_PCT
        /// </summary>
        public const int Rst = 4134;

        /// <summary>
        /// Заявка на инновационный ИЗ
        /// </summary>
        public const int InnovativeInvention = 291;

        /// <summary>
        /// Заявка на предварительный ПО
        /// </summary>
        public const int PreliminaryIndustrialSample = 451;

        #endregion

        #region ProtectionDoc

        /// <summary>
        /// Патент на Промышленный образец
        /// </summary>
        public const int Pat = 391;

        /// <summary>
        /// Патент на Полезную Модель
        /// </summary>
        public const int Patpm = 1292;

        /// <summary>
        /// Свидетельство на ТЗ
        /// </summary>
        public const int GrTzSvid = 394;

        /// <summary>
        /// Инновационный патент
        /// </summary>
        public const int InPat = 294;

        /// <summary>
        /// Патент на изобретение
        /// </summary>
        public const int OdPatIzRu = 529;

        /// <summary>
        /// Предварительный патент на Промышленный образец
        /// </summary>
        public const int Ppat = 1291;

        /// <summary>
        /// Патент на СД (растениеводство)
        /// </summary>
        public const int SdPat = 1431;

        /// <summary>
        /// Патент на СД (животноводство)
        /// </summary>
        public const int SdPat2 = 2551;

        /// <summary>
        /// Предварительный патент на изобретение
        /// </summary>
        public const int PatPred = 392;

        /// <summary>
        /// Сертификат на общеизвест. ТЗ
        /// </summary>
        public const int SvidTzOb = 4186;

        /// <summary>
        /// Регистрация ТЗ
        /// </summary>
        public const int RegTz = 4204;

        /// <summary>
        /// Регистрация НМПТ
        /// </summary>
        public const int RegNmpt = 4208;

        /// <summary>
        /// Регистрация ТЗ (общеизвестный)
        /// </summary>
        public const int RegTzOb = 4293;

        /// <summary>
        /// Свидетельство на НМПТ
        /// </summary>
        public const int SvidNmpt = 3272;

        /// <summary>
        /// Свидетельство прав на объект авторского права
        /// </summary>
        public const int ApSvidetelstvo = 4115;

        /// <summary>
        /// Свидетельство на топологию интегральных микросхем
        /// </summary>
        public const int TimSvidetelstvo = 4444;

        #endregion
    }
}