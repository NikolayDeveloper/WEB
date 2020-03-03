using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Request
{
    public class RequestProtectionDocSimilarDto
    {
	    public DateTimeOffset DateCreate { get; set; }
	    public int RequestId { get; set; }
	    public int ProtectionDocId { get; set; }
	}
}
