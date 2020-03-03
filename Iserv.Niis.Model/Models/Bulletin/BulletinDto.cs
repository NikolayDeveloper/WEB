using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Bulletin
{
    public class BulletinDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
    }
}
