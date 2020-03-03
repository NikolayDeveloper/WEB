using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models
{
    public class DocumentsCompareDto
    {
        public string Description { get; set; }
        public int ChangedDescriptionDocId { get; set; }
        public string ChangedDescription { get; set; }
        public string Essay { get; set; }
        public int ChangedEssayDocId { get; set; }
        public string ChangedEssay { get; set; }
        public string Formula { get; set; }
        public int ChangedFormulaDocId { get; set; }
        public string ChangedFormula { get; set; }

    }
}
