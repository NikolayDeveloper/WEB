using System;
using System.Collections.Generic;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Notifications.Logic;
using Newtonsoft.Json;
using Iserv.Niis.Common.Codes;
using System.Linq;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class RequestDocumentSendStage : INotificationMessageRequirement<Document>, IEmailNotificationRequirement
    {
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

        public RequestDocumentSendStage(IDocumentGeneratorFactory templateGeneratorFactory)
        {
            _templateGeneratorFactory = templateGeneratorFactory;
        }
        public Document CurrentRequestObject { get; set; }

        public string Message => string.Format(MessageTemplates.Email, $"{CurrentRequestObject.Type.NameRu} №{CurrentRequestObject.OutgoingNumber}");

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsEmail { get { return true; } set { } }
        public string Subject
        {
            get
            {
                return $"{CurrentRequestObject.Type.NameRu} №{CurrentRequestObject.OutgoingNumber}";
            }
            set { }
        }

        public byte[] Attachment
        {
            get
            {
                var userInput = NiisAmbientContext.Current.Executor.GetQuery<GetDoumentUserInputByDocumentIdQuery>().Process(r => r.ExecuteAsync(CurrentRequestObject.Id));
                var userInputDto = JsonConvert.DeserializeObject<UserInputDto>(userInput.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(CurrentRequestObject.Type.Code);
                var generatedDocument = documentGenerator.Process(new Dictionary<string, object>
                {
                    {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                    {"RequestId", userInputDto.OwnerId},
                    {"DocumentId", CurrentRequestObject.Id},
                    {"UserInputFields", userInputDto.Fields},
                    {"SelectedRequestIds", userInputDto.SelectedRequestIds},
                    {"PageCount", userInputDto.PageCount},
                    {"OwnerType", userInputDto.OwnerType},
                    {"Index", userInputDto.Index }
                });

                return generatedDocument.File;
            }
        }

        public bool IsPassesRequirements()
        {
            if (CurrentRequestObject.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == RouteStageCodes.DocumentOutgoing_03_1)
                && !string.IsNullOrEmpty(CurrentRequestObject.OutgoingNumber))
            {
                return true;
            }

            return false;
        }
    }
}
