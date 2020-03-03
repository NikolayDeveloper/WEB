using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoNotificationDocumentGeneration
{
    /// <summary>
    /// Правила автогенерации документов-уведомлений по триггеру создания ходатайств и зачтению платежек
    /// </summary>
    public class AutoGenerateNotificationDocumentByPetitionAndPaymentRule: Entity<int>
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
        /// id триггера-тарифа
        /// </summary>
        public int TariffId { get; set; }
        /// <summary>
        /// объект триггера-тарифа
        /// </summary>
        public DicTariff Tariff { get; set; }
        /// <summary>
        /// id типа триггера-ходатайства
        /// </summary>
        public int PetitionTypeId { get; set; }
        /// <summary>
        /// объект типа триггера-ходатайства
        /// </summary>
        public DicDocumentType PetitionType { get; set; }
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
