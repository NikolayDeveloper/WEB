using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowBusinessLogic.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreWorkflow;
using System;
using System.Linq;

namespace Iserv.Niis.WorkflowServices
{
    public class WorkflowServiceRequest : IWorkflowServiceRequest
    {
        private readonly IExecutor _executor;

        public WorkflowServiceRequest(IExecutor executor)
        {
            _executor = executor;
        }

        public void Process(RequestWorkFlowRequest requestWorkFlowRequest)
        {
            if (requestWorkFlowRequest.RequestId != 0)
            {
                //requestWorkFlowRequest.CurrentWorkflowObject = _executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(requestWorkFlowRequest.RequestId));
                requestWorkFlowRequest.CurrentWorkflowObject = _executor.GetQuery<GetRequestByIdForWorkflowServiceQuery>().Process(r => r.Execute(requestWorkFlowRequest.RequestId));

                if(requestWorkFlowRequest.IsAuto)
                {
					if (requestWorkFlowRequest.CurrentWorkflowObject.Department.Code == DicDepartmentCodes.D_3_4)
					{
						var users = _executor.GetQuery<GetUsers>().Process(r => r.Execute());
						var chief = users.FirstOrDefault(u => u.DepartmentId == requestWorkFlowRequest.CurrentWorkflowObject.Department.Id
                                                              && u.Position.Code == "030a"
                                                              && u.Position.PositionType.Code == "34");

                        if (chief != null) requestWorkFlowRequest.NextStageUserId = chief.Id;
                    }
					else
					{
						requestWorkFlowRequest.NextStageUserId = requestWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentUserId ?? 0;
					}
                }

                RequestWorkflows(requestWorkFlowRequest);
            }
        }

        private void RequestWorkflows(RequestWorkFlowRequest requestWorkFlowRequest)
        {
            NetCoreBaseWorkflow<RequestWorkFlowRequest, Request> requestWorkFlow;
            switch (requestWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflow.Route.Code)
            {
                case RouteCodes.TradeMark:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestTradeMarkWorkflow;
                    break;

                case RouteCodes.InternationalTrademarks:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestInternationalTradeMarkWorkflow;
                    break;

                case RouteCodes.Inventions:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestInventionsWorkflow;
                    break;

                case RouteCodes.SelectiveAchievements:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestSelectiveAchievementsWorkFlow;
                    break;

                case RouteCodes.AppellationOfOrigin:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestAppellationOfOriginWorkflow;
                    break;

                case RouteCodes.IndustrialDesigns:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestIndustrialDesignsWorkflow;
                    break;

                case RouteCodes.UsefulModel:
                    requestWorkFlow = NiisWorkflowAmbientContext.Current.RequestUsefulModelWorkflow;
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (requestWorkFlow == null)
            {
                throw new NotImplementedException();
            }

            requestWorkFlow.SetWorkflowRequest(requestWorkFlowRequest);

            requestWorkFlow.Process();
        }
    }
}
