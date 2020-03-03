using Iserv.Niis.Domain.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class InventionSearchDto
    {
        public int? Id { get; set; }
        public Owner.Type OwnerType { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Номер заявки")]
        public string RegNumber { get; set; }

        [Display(Name = "Дата подачи заявки")]
        public DateTimeOffset? RegDate { get; set; }

        [Display(Name = "Статус(заявки/ОД)")]
        public string Status { get; set; }

        [Display(Name = "Номер ОД")]
        public string GosNumber { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTimeOffset? PublishDate { get; set; }

        [Display(Name = "Патентообладатель/Заявитель")]
        public string Declarant { get; set; }

        [Display(Name = "МПК (индекс)")]
        public string Ipc { get; set; }

        [Display(Name = "Реферат")]
        public string Referat { get; set; }
    }
}