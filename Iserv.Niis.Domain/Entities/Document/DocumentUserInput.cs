using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Хранение пользовательского ввода документа
    /// </summary>
    public class DocumentUserInput: Entity<int>, IHaveConcurrencyToken
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public string UserInput { get; set; }
    }
}
