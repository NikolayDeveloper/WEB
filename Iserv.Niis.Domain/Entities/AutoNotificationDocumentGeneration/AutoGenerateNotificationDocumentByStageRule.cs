using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoNotificationDocumentGeneration
{
    /// <summary>
    /// Правила автогенерации документов-уведомлений по триггеру этапов
    /// </summary>
    public class AutoGenerateNotificationDocumentByStageRule: Entity<int>
    {
        /// <summary>
        /// id этапа, на котором должно генерироваться уведомление
        /// </summary>
        public int StageId { get; set; }
        /// <summary>
        /// объект этапа, на котором должно генерироваться уведомление
        /// </summary>
        public DicRouteStage Stage { get; set; }
        /// <summary>
        /// id типа генерируемого уведомления
        /// </summary>
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// объект типа генерируемого уведомления
        /// </summary>
        public DicDocumentType NotificationType { get; set; }
    }
}
