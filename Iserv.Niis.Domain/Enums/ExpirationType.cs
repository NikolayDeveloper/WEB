namespace Iserv.Niis.Domain.Enums
{
    /// <summary>
    /// Тип истечения
    /// </summary>
    public enum ExpirationType
    {
        None = 0,
        /// <summary>
        /// Календарный день
        /// </summary>
        CalendarDay = 1,
        /// <summary>
        /// Рабочий день
        /// </summary>
        WorkDay = 2,
        /// <summary>
        /// Календарный месяц
        /// </summary>
        CalendarMonth = 3,
    }
}