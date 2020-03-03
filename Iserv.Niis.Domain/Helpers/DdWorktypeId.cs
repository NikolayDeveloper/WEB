namespace Iserv.Niis.Domain.Helpers
{
    /// <summary>
    /// Маршруты
    /// </summary>
    public static class DdWorktypeId
    {
        #region Requests

        /// <summary>
        /// Обработка Заявок ИЗ
        /// </summary>
        public const int B = 4;

        /// <summary>
        /// Обработка Заявок ТЗ
        /// </summary>
        public const int TM = 6;

        /// <summary>
        /// Обработка Заявок СД
        /// </summary>
        public const int SA = 7;

        /// <summary>
        /// Обработка Заявок ПО
        /// </summary>
        public const int S2 = 8;

        /// <summary>
        /// Обработка Заявок ПМ
        /// </summary>
        public const int U = 21;

        /// <summary>
        /// Обработка Заявок НМПТ
        /// </summary>
        public const int NMPT = 101;

        /// <summary>
        /// Обработка Заявок МТЗ
        /// </summary>
        public const int TMI = 141;

        /// <summary>
        /// Обработка заявок ДК
        /// </summary>
        public const int DK = 162;

        /// <summary>
        /// Обработка Заявок по АП
        /// </summary>
        public const int AP = 163;

        /// <summary>
        /// Обработка Заявок ИЗ_IN
        /// </summary>
        public const int B_1 = 164;

        /// <summary>
        /// Обработка Заявок ТИМ
        /// </summary>
        public const int TIM = 166;

        #endregion

        #region Materials

        /// <summary>
        /// Обработка входящей корреспонденции 
        /// </summary>
        public const int IN = 1;

        /// <summary>
        /// Обработка исходящей корреспонденции
        /// </summary>
        public const int OUT = 2;

        /// <summary>
        /// Внутренняя переписка
        /// </summary>
        public const int W = 3;

        /// <summary>
        /// Обработка входящей корреспонденции 
        /// </summary>
        public const int INТТТ = 167;

        #endregion

        /// <summary>
        /// Обработка ОД
        /// </summary>
        public const int GR = 22;

        /// <summary>
        /// Обработка договоров
        /// </summary>
        public const int DK_GR = 165;
    }
}