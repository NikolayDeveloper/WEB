using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class UpdateDocumentCommand:BaseCommand
    {
        public async Task Execute(Document document)
        {
            var repo = Uow.GetRepository<Document>();
            var oldDocument = repo.AsQueryable().Where(d => d.Id == document.Id).Include(d => d.MainAttachment).FirstOrDefault();

            oldDocument.StatusId = document.StatusId;
            oldDocument.Status = document.Status;
            oldDocument.TypeId = document.TypeId;
            oldDocument.AddresseeAddress = document.AddresseeAddress;
            oldDocument.AddresseeId = document.AddresseeId;
            oldDocument.Barcode = document.Barcode;
            oldDocument.DepartmentId = document.DepartmentId;
            oldDocument.Description = document.Description;
            oldDocument.DivisionId = document.DivisionId;
            oldDocument.DocumentNum = document.DocumentNum;
            oldDocument.IncomingNumber = document.IncomingNumber;
            oldDocument.IncomingNumberFilial = document.IncomingNumberFilial;
            oldDocument.NameEn = document.NameEn;
            oldDocument.NameRu = document.NameRu;
            oldDocument.NameKz = document.NameKz;
            oldDocument.MainAttachmentId = document.MainAttachmentId;
            oldDocument.OutgoingNumber = document.OutgoingNumber;
            oldDocument.ReceiveTypeId = document.ReceiveTypeId;
            oldDocument.SendTypeId = document.SendTypeId;
            oldDocument.SendingDate = document.SendingDate;
            oldDocument.PageCount = document.PageCount;
            oldDocument.CopyCount = document.CopyCount;
            oldDocument.ControlMark = document.ControlMark;
            oldDocument.ControlDate = document.ControlDate;
            oldDocument.ResolutionExtensionControlDate = document.ResolutionExtensionControlDate;
            oldDocument.OutOfControl = document.OutOfControl;
            oldDocument.DateOutOfControl = document.DateOutOfControl;
            oldDocument.IsHasPaymentDocument = document.IsHasPaymentDocument;
            oldDocument.NumberForPayment = document.NumberForPayment;
            oldDocument.PaymentDate = document.PaymentDate;
            oldDocument.PaymentInvoiceId = document.PaymentInvoiceId;
            oldDocument.IncomingDocumentNumber = document.IncomingDocumentNumber;
            oldDocument.Comments = document.Comments;
            oldDocument.TrackNumber = document.TrackNumber;
            oldDocument.IncomingAnswerId = document.IncomingAnswerId;
            oldDocument.AttachedPaymentsCount = document.AttachedPaymentsCount;

            if (oldDocument.MainAttachment != null)
            {
                oldDocument.MainAttachment.PageCount = document.PageCount;
                oldDocument.MainAttachment.CopyCount = document.CopyCount;
            }

            repo.Update(oldDocument);
            await Uow.SaveChangesAsync();
        }
    }
}
