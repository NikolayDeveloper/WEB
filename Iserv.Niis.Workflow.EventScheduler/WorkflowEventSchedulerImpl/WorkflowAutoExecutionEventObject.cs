using System;
using System.Threading;
using System.Threading.Tasks;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using Iserv.Niis.WorkflowServices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Workflow.EventScheduler.WorkflowEventSchedulerImpl
{
    /// <summary>
    /// Выполняемая задача, которая запускает определенный рабочий процесс с пометкой "автоматический".
    /// </summary>
    public class WorkflowAutoExecutionEventObject
    {
        private readonly IExecutor _executor;
        private readonly CancellationTokenSource _workflowEventCancellationToken;

        public WorkflowAutoExecutionEventObject(IExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Название задачи. Представляет из себя строку в которой хранится, тип, который относится к задаче (заявка, охранный документ, договор), и его Id.
        /// <para></para>
        /// Пример:
        /// Задача на заявку. Id заявки = 5. -> "Request_5"
        /// </summary>
        public string EventName { get; private set; }
        /// <summary>
        /// Делегат, который запускает рабочий процесс.
        /// </summary>
        public Action<WorkflowTaskQueue> EventAction { get; private set; }
        /// <summary>
        /// Запланированная задача.
        /// </summary>
        public WorkflowTaskQueue WorkflowTaskEvent { get; private set; }

        public WorkflowAutoExecutionEventObject()
        {
            _executor = NiisAmbientContext.Current.Executor;
            _workflowEventCancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Запускает выполняемую задачу. Задача ждет своего времени исполнения и запускает определенный рабочий процесс. После выполнения, задача помечается как "выполненная".
        /// <para></para>
        /// Если задача просрочена, она выполняется сразу же после запуска.
        /// </summary>
        public void StartEventExecution()
        {
            Task.Run(async delegate
            {
                WriteLogMessage($"Init {EventName}");

                var delayTime = WorkflowTaskEvent.ResolveDate - NiisAmbientContext.Current.DateTimeProvider.Now;
                if (delayTime > TimeSpan.Zero)
                {
                    await Task.Delay(delayTime, _workflowEventCancellationToken.Token);
                }

                EventAction?.Invoke(WorkflowTaskEvent);

                WorkflowAutoEvents.RemoveEvent(EventName);

                _executor.GetCommand<UpdateMarkAsExecutedWorkflowTaskEvenstByIdCommand>().Process(r => r.Execute(WorkflowTaskEvent.Id));

                WriteLogMessage($"Test {EventName}");
            }, _workflowEventCancellationToken.Token);

            WriteLogMessage($"New event start {EventName}");
        }

        /// <summary>
        /// Останавливает выполняемую задачу.
        /// </summary>
        public void StopEventExecution()
        {
            WriteLogMessage($"New event stop {EventName}");
            _workflowEventCancellationToken.Cancel();
        }

        /// <summary>
        /// Создает выполняемую задачу из запланированной задачи из бд.
        /// </summary>
        /// <param name="workflowTaskEvent">Запланированная задача из бл.</param>
        /// <returns>Выполняемая задача.</returns>
        public static WorkflowAutoExecutionEventObject ConstructFrom(WorkflowTaskQueue workflowTaskEvent)
        {
            var workflowEventObject = new WorkflowAutoExecutionEventObject
            {
                EventName = workflowTaskEvent.WorkflowEventKey,
                WorkflowTaskEvent = workflowTaskEvent
            };

            if (workflowTaskEvent.IsRequestEvent)
            {
                workflowEventObject.EventAction = ProcessRequestWorkflowEvent;
            }
            else if (workflowTaskEvent.IsContractEvent)
            {
                workflowEventObject.EventAction = ProcessContractWorkflowEvent;
            }
            else if (workflowTaskEvent.IsProtectionDocEvent)
            {
                workflowEventObject.EventAction = ProcessProtectionDocumentWorkflowEvent;
            }

            return workflowEventObject;
        }

        /// <summary>
        /// Запускает рабочий процесс, связанный с заявкой, с пометкой "автоматический".
        /// </summary>
        /// <param name="workflowTaskEvent">Запланированная задача.</param>
        private static void ProcessRequestWorkflowEvent(WorkflowTaskQueue workflowTaskEvent)
        {
            var requestWorkFlowRequest = new RequestWorkFlowRequest
            {
                RequestId = workflowTaskEvent.RequestId ?? default(int),
                IsAuto = true
            };

            NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
        }
    
        /// <summary>
        /// Запускает рабочий процесс, связанный с договором, с пометкой "автоматический".
        /// </summary>
        /// <param name="workflowTaskEvent"></param>
        private static void ProcessContractWorkflowEvent(WorkflowTaskQueue workflowTaskEvent)
        {

            var сontractWorkFlowRequest = new ContractWorkFlowRequest
            {
                ContractId = workflowTaskEvent.ContractId ?? default(int),
                IsAuto = true
            };

            NiisWorkflowAmbientContext.Current.ContractWorkflowService.Process(сontractWorkFlowRequest);
        }

        /// <summary>
        /// Запускает рабочий процесс связанный, с охранным документом(ОД), с пометкой "автоматический".
        /// </summary>
        /// <param name="workflowTaskEvent">Запланированная задача.</param>
        private static void ProcessProtectionDocumentWorkflowEvent(WorkflowTaskQueue workflowTaskEvent)
        {

            var protectionDocumentWorkFlowRequest = new ProtectionDocumentWorkFlowRequest
            {
                ProtectionDocId = workflowTaskEvent.ProtectionDocId ?? default(int),
                IsAuto = true
            };

            NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(protectionDocumentWorkFlowRequest);
        }

        /// <summary>
        /// Логгирует сообщение. 
        /// <para></para>      
        /// Внимание, прошлый разработчик оставил этот метод таким, но хотел реализовать нормальное логгирование.
        /// </summary>
        /// <param name="logMessage">Текст сообщения.</param>
        private static void WriteLogMessage(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
