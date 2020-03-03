
using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Entities.Request;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Контент документ
    /// </summary>
    public class DocumentContent : Entity<int>
    {
        public DocumentContent()
        {
            
        }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public string Content { get; set; }
    }
}