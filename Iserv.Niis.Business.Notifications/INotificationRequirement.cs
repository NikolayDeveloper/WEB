using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Notification;
using System;

namespace Iserv.Niis.Notifications
{
    internal interface INotificationMessageRequirement<TRequestObject>
        where TRequestObject : Entity<int>, new()
    {
        TRequestObject CurrentRequestObject { get; set; }

        DateTimeOffset NotificationDate { get; }
        string Message { get; }


        bool IsPassesRequirements();
    }
}
