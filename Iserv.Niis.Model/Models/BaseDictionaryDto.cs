using System;

namespace Iserv.Niis.Model.Models
{
    public class BaseDictionaryDto
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
    }
}