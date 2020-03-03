using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class FilterNextStagesByProtectionDocIdHandler: BaseHandler
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int protectionDocId)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(protectionDocId));
            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);
            List<DicRouteStage> nextStages;
            bool parallel = protectionDoc.Workflows != null
                 && protectionDoc.Workflows.Any(x => x.CurrentStage != null && x.CurrentStage.Code == RouteStageCodes.ODParallel);

            if (parallel)
            {
                nextStages = await Executor.GetQuery<GetNextStagesByCurrentUserIdQuery>()
                    .Process(q => q.ExecuteAsync(protectionDoc.Id, NiisAmbientContext.Current.User.Identity.UserId));
                if (nextStages.Count == 0)
                    nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                        .Process(q => q.ExecuteAsync(protectionDoc.CurrentWorkflow.RouteId ?? default(int)));
                //nextStages = await Executor.GetQuery<GetNextStagesByCurrentStageIdQuery>()
                //.Process(q => q.ExecuteAsync(protectionDoc.CurrentWorkflow.CurrentStageId ?? default(int)));
            }
            else
                nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                    .Process(q => q.ExecuteAsync(protectionDoc.CurrentWorkflow.RouteId ?? default(int)));
            //nextStages = await Executor.GetQuery<GetNextStagesByCurrentStageIdQuery>()
            //    .Process(q => q.ExecuteAsync(protectionDoc.CurrentWorkflow.CurrentStageId ?? default(int)));

            //if (protectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD05_01)
            //{
            //    if (protectionDoc.IcgsProtectionDocs.Count < 2)
            //    {
            //        nextStages = nextStages.Where(ns => ns.Code != RouteStageCodes.OD04_4).ToList();
            //    }
            //}
            //if (new[] {RouteStageCodes.OD05, RouteStageCodes.OD05_01}.Contains(protectionDoc.CurrentWorkflow
            //    .CurrentStage.Code))
            //{
            //    var currentStageCreateDate = protectionDoc.CurrentWorkflow.DateCreate;
            //    if (!protectionDoc.Documents.Any(pdd =>
            //        pdd.Document.Type.Code == DicDocumentTypeCodes.PetitionForCopy &&
            //        pdd.Document.DateCreate > currentStageCreateDate))
            //    {
            //        nextStages = nextStages.Where(ns => ns.Code != RouteStageCodes.OD04_5).ToList();
            //    }
            //    //if (!protectionDoc.Documents.Any(pdd =>
            //    //    new[]
            //    //    {
            //    //        DicDocumentTypeCodes.DecisionOfAppealsBoard,
            //    //        DicDocumentTypeCodes._006_02_01,
            //    //        DicDocumentTypeCodes.PetitionForInvalidation
            //    //    }.Contains(pdd.Document.Type.Code) &&
            //    //    pdd.Document.DateCreate > currentStageCreateDate))
            //    //{
            //    //    nextStages = nextStages.Where(ns => ns.Code != RouteStageCodes.OD03_2).ToList();
            //    //}
            //}

            return nextStages;
        }
    }
}
