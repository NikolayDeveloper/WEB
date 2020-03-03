using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Notifications.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Iserv.Niis.Notifications.Helpers
{
    internal class SendHelper
    {
        public string Send(SendModel model)
        {
            return model.IsSms ? SendSms(model) : SendEmail(model);
        }

        private string SendSms(SendModel model)
        {
            if (model.MobilePhones.Count <= 0)
                return DicNotificationStatus.Codes.PhoneNotFound;

            SmscApi api = new SmscApi();

            var responseStatuses = new ConcurrentBag<string>();
            Parallel.ForEach(model.MobilePhones, phone =>
            {
                var responseModel = api.SendSms(phone, model.Message, model.Credentials);
                var responseStatus = !responseModel.ErrorCode.Equals(0)
                                        ? DicNotificationStatus.Codes.SmsSendFail
                                        : DicNotificationStatus.Codes.SmsSend;
                responseStatuses.Add(responseStatus);
            });

            return responseStatuses.Contains(DicNotificationStatus.Codes.SmsSend)
                ? DicNotificationStatus.Codes.SmsSend
                : DicNotificationStatus.Codes.SmsSendFail;
        }

        private string SendEmail(SendModel model)
        {
            var attachment = new MimePart("document", "pdf")
            {
                Content = new MimeContent(new MemoryStream(model.Attachment)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "document.pdf"
            };

            MimeMessage message = new MimeMessage { Subject = model.Subject };
            var body = new TextPart(TextFormat.Plain)
            {
                Text = model.Message ?? string.Empty
            };
            var multipart = new Multipart("mixed") { body, attachment };

            message.Body = multipart;

            if (model.EmailAddresses.Count <= 0)
                return DicNotificationStatus.Codes.EmailNotFound;

            message.From.Add(new MailboxAddress("NIIS", model.Credentials.EmailFrom));

            foreach (var emailAddress in model.EmailAddresses)
            {
                message.To.Add(new MailboxAddress("", emailAddress));
            }


            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(model.Credentials.SmtpServer, model.Credentials.SmtpPort, true);
                    client.Authenticate(model.Credentials.EmailFrom, model.Credentials.EmailPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                return DicNotificationStatus.Codes.EmailSendFail;
            }
            
            return DicNotificationStatus.Codes.EmailSend;
        }
    }
}
