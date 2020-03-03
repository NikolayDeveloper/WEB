using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Material
{
    public class MaterialItemDto
    {
        public int Id { get; set; }
        public string DocumentNum { get; set; }
        public string TypeNameRu { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public string Initiator { get; set; }

        /// <summary>
        /// Все испольнители в текущих этапах
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// Все статусы текущих исполнений
        /// </summary>
        public string Status { get; set; }

        public bool CanDownload { get; set; }
        public bool HasTemplate { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
