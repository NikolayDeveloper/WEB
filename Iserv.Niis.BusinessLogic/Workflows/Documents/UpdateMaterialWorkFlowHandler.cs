using AutoMapper;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class UpdateMaterialWorkFlowCommand : BaseCommand
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);

        public async Task ExecuteAsync(MaterialWorkflowDto workflowDto)
        {
            //var workflow = Mapper.Map<DocumentWorkflow>(workflowDto);
            var workflowRepo = Uow.GetRepository<DocumentWorkflow>();
            var workflow = await workflowRepo.GetByIdAsync(workflowDto.Id);
            workflow.CurrentUserId = workflowDto.CurrentUserId;
            workflowRepo.Update(workflow);
            await Uow.SaveChangesAsync();

            ////var workflowWithIncludes = await Executor.GetQuery<GetWorkflowsByDocumentIdQuery>()
            ////    .Process(q => q.ExecuteAsync(workflowId));

            ////var result = Mapper.Map<MaterialWorkflowDto>(workflow);
        }
    }
}
