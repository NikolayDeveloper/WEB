using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.References;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Международная Патентная Классификация
    /// </summary>
    public class DicIPC : DictionaryEntity<int>, IHistorySupport, IClassification<DicIPC>, IHaveConcurrencyToken
    {
        public int RevisionNumber { get; set; }
        public string EntryType { get; set; }
        public string Kind { get; set; }
        public string EntryLevel { get; set; }

        #region Relationships

        public int? ParentId { get; set; }
        public DicIPC Parent { get; set; }
        public ICollection<DicIPC> Childs { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(RefIPCHistory);
        }
    }
}