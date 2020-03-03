using System;

namespace Iserv.Niis.Model.Models.Calendar
{
    public class PublicationRange
    {
        public PublicationRange(
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateTimeOffset StartDate{ get; set; }
        public DateTimeOffset EndDate{ get; set; }
    }
}