using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Службы
    /// </summary>
    public class DicDepartment : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public int DepartmentTypeId { get; set; }
        public DicDepartmentType DepartmentType { get; set; }
        public bool IsMonitoring { get; set; }
        public int DivisionId { get; set; }
        public DicDivision Division { get; set; }
        public string TNameRu { get; set; }
        public string ShortNameRu { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(NameRu) ? NameRu : Code;
        }
    }
}