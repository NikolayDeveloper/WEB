using AutoMapper;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class CreateMaterialWorkFlowHandler : BaseHandler
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);

        private readonly IList<string> LastStageCodes = new List<string>() {
            DicRouteStageCodes.IN01_1_3
        }; 

        public async Task<MaterialWorkflowDto> ExecuteAsync(MaterialWorkflowDto workflowDto, bool isPrevious = false)
        {
            var workflow = Mapper.Map<DocumentWorkflow>(workflowDto);
            int workflowId = await Executor.GetCommand<ApplyDocumentWorkflowCommand>()
                    .Process(c => c.ExecuteAsync(workflow, isPrevious));

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(workflowDto.OwnerId ?? default(int)));

            var workflowWithIncludes = await Executor.GetQuery<GetWorkflowsByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(workflowId));
            var result = Mapper.Map<MaterialWorkflowDto>(workflowWithIncludes);

            if (LastStageCodes.Contains(workflowDto.CurrentStageCode))
            {
                var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed));
                var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(workflow.OwnerId));
                document.StatusId = workStatus.Id;
                await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
            }

            return result;
        }
    }
}
