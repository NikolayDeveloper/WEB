using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.Domain.EntitiesHistory.References;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicDocumentType : DictionaryEntity<int>, IHistorySupport, IReference, IHaveConcurrencyToken
    {
        public DicDocumentType()
        {
            DicProtectionDocTypes = new HashSet<DicDocumentTypeDicProtectionDocTypeRelation>();
        }

        public int? TemplateFileId { get; set; }
        public DocumentTemplateFile TemplateFile { get; set; }
        public bool? IsUnique { get; set; }
        public int? Order { get; set; }
        public bool? IsRequireSigning { get; set; }
        public int? Interval { get; set; }
        public string ConServiceTypeCode { get; set; }
        public bool? IsSendByEmail { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(RefDocumentTypeHistory);
        }

        #region Relationships

        public int? RouteId { get; set; }
        public DicRoute Route { get; set; }
        public int? ClassificationId { get; set; }
        public DicDocumentClassification Classification { get; set; }
        public ICollection<DicDocumentTypeDicProtectionDocTypeRelation> DicProtectionDocTypes { get; set; }

        #endregion
    }
}