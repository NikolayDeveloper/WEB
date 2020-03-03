using System;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Model.Models.Calendar
{
    public class EventDto
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }

        public int EventTypeId { get; set; }
    }
}