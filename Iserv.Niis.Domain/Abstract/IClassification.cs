using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iserv.Niis.Domain.Abstract
{
    /// <summary>
    /// Указывает на то, что сущность содержит ссылку на родительскую сущность. Учавствует в иерархие
    /// </summary>
    public interface IClassification<T> : IReference
    {
        int? ParentId { get; set; }
        [JsonIgnore]
        T Parent { get; set; }
        [JsonIgnore]
        ICollection<T> Childs { get; set; }
    }
}