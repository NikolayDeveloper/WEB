using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.DocumentWorkflows
{
    public class GetDocumentWorkflowByIdQuery : BaseQuery
    {
        public DocumentWorkflow Execute(int documentWorkflowId)
        {
            var repo = Uow.GetRepository<DocumentWorkflow>();

            return repo.GetById(documentWorkflowId);
        }
    }
}
