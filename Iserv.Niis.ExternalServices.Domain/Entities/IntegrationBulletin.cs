using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationBulletin
    {
        public int Id { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Sent { get; set; }
        public string Note { get; set; }
    }
}