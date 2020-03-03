using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Info.RequestInfos
{
    public class UpdateRequestInfoCommand : BaseCommand
    {
        public async Task ExecuteAsync(RequestInfo requestInfo)
        {
            var requestInfoRepo = Uow.GetRepository<RequestInfo>();
            var originRequestInfo = await requestInfoRepo
                .AsQueryable()
                .FirstOrDefaultAsync(ri => ri.RequestId == requestInfo.RequestId);

            if (originRequestInfo == null)
            {
                return;
            }

            originRequestInfo.RequestId = requestInfo.RequestId;
            originRequestInfo.AcceptAgreement = requestInfo.AcceptAgreement;
            originRequestInfo.Breed = requestInfo.Breed;
            originRequestInfo.BreedCountryId = requestInfo.BreedCountryId;
            originRequestInfo.BreedingNumber = requestInfo.BreedingNumber;
            originRequestInfo.FlagHeirship = requestInfo.FlagHeirship;
            originRequestInfo.FlagNine = requestInfo.FlagNine;
            originRequestInfo.FlagTat = requestInfo.FlagTat;
            originRequestInfo.FlagTpt = requestInfo.FlagTpt;
            originRequestInfo.FlagTth = requestInfo.FlagTth;
            originRequestInfo.FlagTtw = requestInfo.FlagTtw;
            originRequestInfo.Genus = requestInfo.Genus;
            originRequestInfo.IsColorPerformance = requestInfo.IsColorPerformance;
            originRequestInfo.IzCollectiveTZ = requestInfo.IzCollectiveTZ;
            originRequestInfo.IsConventionPriority = requestInfo.IsConventionPriority;
            originRequestInfo.Priority = requestInfo.Priority;
            originRequestInfo.IsExhibitPriority = requestInfo.IsExhibitPriority;
            originRequestInfo.IsStandardFont = requestInfo.IsStandardFont;
            originRequestInfo.IsTransliteration = requestInfo.IsTransliteration;
            originRequestInfo.Transliteration = requestInfo.Transliteration;
            originRequestInfo.IsTranslation = requestInfo.IsTranslation;
            originRequestInfo.Translation = requestInfo.Translation;
            originRequestInfo.IsVolumeTZ = requestInfo.IsVolumeTZ;
            originRequestInfo.ProductType = requestInfo.ProductType;
            originRequestInfo.ProductSpecialProp = requestInfo.ProductSpecialProp;
            originRequestInfo.ProductPlace = requestInfo.ProductPlace;

            requestInfoRepo.Update(originRequestInfo);
            await Uow.SaveChangesAsync();
        }
    }
}