using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Info.ProtectionDocInfos
{
    public class UpdateProtectionDocInfoCommand: BaseCommand
    {
        public async Task ExecuteAsync(ProtectionDocInfo protectionDocInfo)
        {
            var requestInfoRepo = Uow.GetRepository<ProtectionDocInfo>();
            var originProtectionDocInfo = await requestInfoRepo
                .AsQueryable()
                .FirstOrDefaultAsync(ri => ri.ProtectionDocId == protectionDocInfo.ProtectionDocId);

            if (originProtectionDocInfo == null)
            {
                return;
            }

            originProtectionDocInfo.ProtectionDocId = protectionDocInfo.ProtectionDocId;
            originProtectionDocInfo.AcceptAgreement = protectionDocInfo.AcceptAgreement;
            originProtectionDocInfo.Breed = protectionDocInfo.Breed;
            originProtectionDocInfo.BreedCountryId = protectionDocInfo.BreedCountryId;
            originProtectionDocInfo.BreedingNumber = protectionDocInfo.BreedingNumber;
            originProtectionDocInfo.FlagHeirship = protectionDocInfo.FlagHeirship;
            originProtectionDocInfo.FlagNine = protectionDocInfo.FlagNine;
            originProtectionDocInfo.FlagTat = protectionDocInfo.FlagTat;
            originProtectionDocInfo.FlagTpt = protectionDocInfo.FlagTpt;
            originProtectionDocInfo.FlagTth = protectionDocInfo.FlagTth;
            originProtectionDocInfo.FlagTtw = protectionDocInfo.FlagTtw;
            originProtectionDocInfo.Genus = protectionDocInfo.Genus;
            originProtectionDocInfo.IsColorPerformance = protectionDocInfo.IsColorPerformance;
            originProtectionDocInfo.IzCollectiveTZ = protectionDocInfo.IzCollectiveTZ;
            originProtectionDocInfo.IsConventionPriority = protectionDocInfo.IsConventionPriority;
            originProtectionDocInfo.Priority = protectionDocInfo.Priority;
            originProtectionDocInfo.IsExhibitPriority = protectionDocInfo.IsExhibitPriority;
            originProtectionDocInfo.IsStandardFont = protectionDocInfo.IsStandardFont;
            originProtectionDocInfo.IsTransliteration = protectionDocInfo.IsTransliteration;
            originProtectionDocInfo.Transliteration = protectionDocInfo.Transliteration;
            originProtectionDocInfo.IsTranslation = protectionDocInfo.IsTranslation;
            originProtectionDocInfo.Translation = protectionDocInfo.Translation;
            originProtectionDocInfo.IsVolumeTZ = protectionDocInfo.IsVolumeTZ;
            originProtectionDocInfo.ProductType = protectionDocInfo.ProductType;
            originProtectionDocInfo.ProductSpecialProp = protectionDocInfo.ProductSpecialProp;
            originProtectionDocInfo.ProductPlace = protectionDocInfo.ProductPlace;

            requestInfoRepo.Update(originProtectionDocInfo);

            await Uow.SaveChangesAsync();
        }
    }
}
