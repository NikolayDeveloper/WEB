using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class Statement
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
        public string Identifier { get; set; }
        public string GosNumber { get; set; }
        public DateTimeOffset? GosDate { get; set; }
        public string ReqNumber { get; set; }
        public DateTimeOffset? ReqDate { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string Xin { get; set; }
        public string Note { get; set; }

        #region Relashionship

        public int RequestId { get; set; }
        public Request.Request Request { get; set; }

        #endregion
    }
}
