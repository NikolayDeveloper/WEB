using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Calendar
{
    public class Event : Entity<int>
    {
        public DateTimeOffset Date { get; set; }

        public int EventTypeId { get; set; }

        public DicEventType EventType { get; set; }
    }
}