using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.System
{
    public class GeneratedInvoiceNumber : Entity<int>
    {
        public ApplicationUser User { get; set; }

        public DicDepartment Department
        {
            get { return Department; }
            set
            {
                Department = value;
                DepartmentCode = value?.Code;
            }
        }

        public string DepartmentCode { get; private set; }
        public int Year { get; set; }
        public Document.Document Document { get; set; }
        public int Index { get; set; }
        public string InvoiceNumber { get; set; }
    }
}