using System;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class UpdateProtectionDocCommand:BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, ProtectionDoc newProtectionDoc)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();
            var oldProtectionDoc = await repository.GetByIdAsync(protectionDocId);
            oldProtectionDoc.DateUpdate = DateTimeOffset.Now;
            oldProtectionDoc.GosDate = newProtectionDoc.GosDate;
            oldProtectionDoc.GosNumber = newProtectionDoc.GosNumber;
            oldProtectionDoc.BulletinUserId = newProtectionDoc.BulletinUserId;
            oldProtectionDoc.SupportUserId = newProtectionDoc.SupportUserId;
            oldProtectionDoc.BeneficiaryTypeId = newProtectionDoc.BeneficiaryTypeId;
            oldProtectionDoc.ConventionTypeId = newProtectionDoc.ConventionTypeId;
            oldProtectionDoc.CurrentWorkflowId = newProtectionDoc.CurrentWorkflowId;
            oldProtectionDoc.Description = newProtectionDoc.Description;
            oldProtectionDoc.DisclaimerKz = newProtectionDoc.DisclaimerKz;
            oldProtectionDoc.DisclaimerRu = newProtectionDoc.DisclaimerRu;
            oldProtectionDoc.Image = newProtectionDoc.Image;
            oldProtectionDoc.NameEn = newProtectionDoc.NameEn;
            oldProtectionDoc.NameRu = newProtectionDoc.NameRu;
            oldProtectionDoc.NameKz = newProtectionDoc.NameKz;
            oldProtectionDoc.OutgoingDate = newProtectionDoc.OutgoingDate;
            oldProtectionDoc.OutgoingNumber = newProtectionDoc.OutgoingNumber;
            oldProtectionDoc.PreviewImage = newProtectionDoc.PreviewImage;
            oldProtectionDoc.Referat = newProtectionDoc.Referat;
            oldProtectionDoc.SelectionAchieveTypeId = newProtectionDoc.SelectionAchieveTypeId;
            oldProtectionDoc.SelectionFamily = newProtectionDoc.SelectionFamily;
            oldProtectionDoc.SubTypeId = newProtectionDoc.SubTypeId;
            oldProtectionDoc.Transliteration = newProtectionDoc.Transliteration;
            oldProtectionDoc.TypeTrademarkId = newProtectionDoc.TypeTrademarkId;
            oldProtectionDoc.ValidDate = newProtectionDoc.ValidDate;
            oldProtectionDoc.MaintainDate = newProtectionDoc.MaintainDate;
            oldProtectionDoc.PageCount = newProtectionDoc.PageCount;
            oldProtectionDoc.SendTypeId = newProtectionDoc.SendTypeId;
            oldProtectionDoc.AddresseeId = newProtectionDoc.AddresseeId;
            repository.Update(oldProtectionDoc);
            await Uow.SaveChangesAsync();
        }
    }
}
