using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.References;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Международная Классификация Промышленных Образцов.
    /// </summary>
    public class DicICIS : DictionaryEntity<int>, IHistorySupport, IClassification<DicICIS>, IHaveConcurrencyToken
    {
        public int RevisionNumber { get; set; }

        #region Relationships

        public int? ParentId { get; set; }
        public DicICIS Parent { get; set; }
        public ICollection<DicICIS> Childs { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(RefICISHistory);
        }
    }
}