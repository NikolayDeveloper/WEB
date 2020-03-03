using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetMessageFileService : IGetMessageFileService
    {
        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly NiisWebContext _niisWebContext;
        private readonly IntegrationValidationHelper _validationHelper;

        public GetMessageFileService(NiisWebContext niisWebContext, IntegrationValidationHelper validationHelper,
            IntegrationAttachFileHelper attachFileHelper,
            DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisWebContext;
            _validationHelper = validationHelper;
            _attachFileHelper = attachFileHelper;
            _dictionaryHelper = dictionaryHelper;
        }

        public void GetMessageFile(GetMessageFileArgument argument, GetMessageFileResult result)
        {
            var dicReceiveTypeElectronicFeedId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed);
            var childClass =
                _dictionaryHelper.GetChildClass(new[]
                    {dicReceiveTypeElectronicFeedId});
            var docInfo = _niisWebContext.DocumentDocumentRelations
                .Include(x => x.Child)
                .Include(x => x.Parent)
                .Include(x => x.Child.Type)
                .Include(x => x.Child.Workflows)
                .Include(x => x.Child.MainAttachment)
                .Where(x => x.Parent.Barcode == argument.MainDocumentID &&
                            (childClass.Contains(x.Child.Type.ClassificationId ?? 0)
                             || x.Child.Type.ClassificationId ==
                             dicReceiveTypeElectronicFeedId) &&
                            x.Child.Workflows.Count(w => w.IsComplete == true) > 0)
                .OrderBy(x => x.Child.CurrentWorkflows.Select(d => d.DateCreate).Max())
                .Select(x => new
                {
                    Id = x.Child.Barcode,
                    x.Child.TypeId,
                    x.Child.OutgoingNumber,
                    x.Child.DateCreate,
                    DocTypeName = x.Child.Type.NameRu,
                    x.Child.MainAttachment.PageCount,
                    x.Child.MainAttachment.OriginalName,
                    x.Child.MainAttachment.BucketName
                })
                .FirstOrDefault();
            if (docInfo == null)
                return;
            result.DocDate = docInfo.DateCreate.Date;
            result.DocNumber = docInfo.OutgoingNumber;
            result.MainDocumentID = argument.MainDocumentID;
            result.PageCount = docInfo.PageCount ?? 0;
            result.CorrespondenceType = new RefKey {UID = docInfo.TypeId, Note = docInfo.DocTypeName};
            result.DocumentID = docInfo.Id;
            var file = _attachFileHelper.GetFile(docInfo.BucketName, docInfo.OriginalName);
            if (_validationHelper.SenderIsPep(argument.SystemInfo.Sender))
            {
                var shepFile = _attachFileHelper.ShepFileUpload(file, docInfo.OriginalName);
                result.File = new File {ShepFile = shepFile};
            }
            else
            {
                result.File = new File {Name = docInfo.OriginalName, Content = file};
            }
        }
    }
}