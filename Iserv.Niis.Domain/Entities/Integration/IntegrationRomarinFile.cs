using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationRomarinFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Status { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
        
    }
}