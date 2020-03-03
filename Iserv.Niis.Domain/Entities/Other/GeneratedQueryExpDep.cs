using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Other
{
    public class GeneratedQueryExpDep : Entity<int>
    {
        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }
        public string DepartmentCode { get; set; }
        public int? DocumentId { get; set; }
        public Document.Document Document { get; set; }
        public int Index { get; set; }
        public string Number { get; set; }
    }
}