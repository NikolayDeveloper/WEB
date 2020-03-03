namespace Iserv.Niis.Domain.Enums
{
    public enum TaskPriority
    {
        /// <summary>
        /// Непросроченные задачи (не подсвечивать)
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Просрочено менее чем на 5 календарных дней (подсвечивать жёлтым)
        /// </summary>
        Yellow = 1,

        /// <summary>
        /// Просрочено на 5 или более календарных дней (подсвечивать красным)
        /// </summary>
        Red = 2,
        
        /// <summary>
        /// Экспертиза не оплачена
        /// </summary>
        Orange = 3
    }
}
