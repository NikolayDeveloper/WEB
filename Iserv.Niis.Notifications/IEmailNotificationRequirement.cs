namespace Iserv.Niis.Notifications
{
    internal interface IEmailNotificationRequirement
    {
        bool IsEmail { get; set; }
        string Subject { get; set; }
        byte[] Attachment { get; }
    }
}
