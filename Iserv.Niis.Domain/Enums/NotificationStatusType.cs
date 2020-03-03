namespace Iserv.Niis.Domain.Enums
{
    public enum NotificationStatusType
    {
        None,
        PhoneNotFound,
        EmailNotFound,
        SmsSendOk,
        SmsSendFail,
        EmailSendOk,
        EmailSendFail,
        CorrespondenceNotFound
    }

    public enum NotificationType
    {
        None,
        Sms,
        Email
    }
}
