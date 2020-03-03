using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Implementations.Request
{
	public class TaskRegister : ITaskRegister<Domain.Entities.Request.Request>
	{
		private readonly ICalendarProvider _calendarProvider;
		private readonly NiisWebContext _context;
		private readonly Dictionary<string, string> _resultStageMap;

		public TaskRegister(NiisWebContext context, ICalendarProvider calendarProvider)
		{
			_context = context;
			_calendarProvider = calendarProvider;
			_resultStageMap = new Dictionary<string, string>
			{
				{"TM01.1", "X01"},
				{"TM02.1", "X01"},
				{"TM02.2.2", "TM02.2.3"},
				{"TM03.2.2", "X01"},
				{"TM03.2.2.1", "X01"},
                // результирующий этап неопределен, определяется динамически при истечении сроков
                {"TM03.3.7.1", string.Empty},
				{"TM03.3.7.3", string.Empty},
				{"TM03.3.7.0", string.Empty},
				{"TM03.2.2.0", string.Empty},
				{"TM03.3.2", "X01"},
				{"TM03.3.5", "X01"},
				{"TM03.3.4.4", "X01"},
				{"TM03.3.2.0", "X01"},
				{"TM03.3.2.0.0", "X01"},
				{"TM03.3.4.4.0", "X01"},
				{"TM03.3.7", "TM03.3.7.0"},
				{"TM03.3.9.1", "TM03.3.9"},
				{"TM03.3.9.2", "X01"},
				{"TM03.3.8", "X01"},

				{"B01.1", "X01"},
				{"B02.1", "X01"},
				{"B02.2.1.0.0", "X01"},
				{"B03.3.1.1.0", string.Empty},
				{"B03.3.1.1.1", string.Empty},
				{"B03.3.4.1", "B03.3.9"},
				{"B03.3.8", "X01"},
				{"B03.3.7.4", "B04.0"}, 
                //{"B03.2.1", "X01"}, формальнаая экспертиза
                {"B02.2.1", "X01"},
				{"B03.2.1.1", "X01"},
				{"B03.3.1.1", "X01"},
				{"B02.2.0", "X01"},
				{"B03.2.1_0", "X01"},
				{"B03.2.4", "X01"},
				{"B03.3.4", "X01"},
				{"B03.3.4.1.0", "X01"},
				{"B03.3.7.0", "B03.3.7.4"},

				{"TMI01.1", "X01"},
				{"TMI02.1", "X01"},
				{"TMI03.3.1", "X01"},
				{"TMI03.3.2", "X01"},
				{"TMI03.3.3", "X01"},
				{"TMI03.3.3.1", "X01"},
				{"TMI03.3.5", "X01"},
				{"TMI03.3.4.1.1", "X01"},
				{"TMI03.3.8", "X01"},
				{"TMI03.3.9", "X01"},
				{"TMI03.3.4", "X01"},
				{"TMI03.3.4.2", "X01"},
				{"TMI03.3.4.3", "X01"},
                //Автоматические этапы МТЗ, не просроченные
                {"TMI03.1", "TMI03.3.1"},
				{"TMI03.3.4.1.0", "TMI03.3.4.1.1"},
				{"TMI03.3.4.5.1", "TMI03.3.4.1.1"},
	            {"TMI03.3.2.0", "TMI03.3.5"},
             
			    //Полезные модели
			    {"U02.1", "X01" },
			    {"U02.2.7", "X01" },
			    {"U03.2.1", "X01" },
			    {"U02.2.6", "U03.3.1" },
			    {"U03.3.1", "U03.2.2" },
			    {"U03.5", "X01" },
			    {"U03.7.1", "U03.3.8" },
			    {"U03.4.0", "U02.2.0" },
				{"U03.3.8", "U02.2.0" },
				{"U03.8", string.Empty },
				

			};
		}

		public async Task RegisterAsync(int requestId)
		{
			try
			{
				var workflowTaskQueue = _context.Requests
					.Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
					.Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
					.Where(r => r.Id == requestId && _resultStageMap.ContainsKey(r.CurrentWorkflow.CurrentStage.Code))
					.ToList()
					.Select(r => new WorkflowTaskQueue
					{
						Request = r,
						ResolveDate = GetResolveDate(r),
						ConditionStage = r.CurrentWorkflow.CurrentStage,
						ResultStage = GetResultStage(r)
					})
					.SingleOrDefault();

				if (workflowTaskQueue != null) await _context.WorkflowTaskQueues.AddAsync(workflowTaskQueue);
			}
			catch (Exception e)
			{
				Log.Warning($"Workflow task registration failed for request id: {requestId}! Details: {e.Message}");
			}
		}

		private DicRouteStage GetResultStage(Domain.Entities.Request.Request request)
		{
			var resultStageCode = _resultStageMap[request.CurrentWorkflow.CurrentStage.Code];

			return _context.DicRouteStages
				.SingleOrDefault(s => !string.IsNullOrWhiteSpace(resultStageCode) && s.Code.Equals(resultStageCode));
		}

		private DateTimeOffset GetResolveDate(Domain.Entities.Request.Request request)
		{
			var startDate = GetStartDate(request);
			var executionDate = GetExecutionDate(request, startDate);

			return executionDate;
		}

		private DateTimeOffset GetExecutionDate(Domain.Entities.Request.Request request, DateTimeOffset startDate)
		{
			var currentStage = request.CurrentWorkflow.CurrentStage;
			if (currentStage.ExpirationValue == null) throw new Exception("");

			var executionDate = _calendarProvider.GetExecutionDate(startDate, currentStage.ExpirationType, currentStage.ExpirationValue.Value);

			// Этапы, которые имеют накопительные даты (5, 15, 25)
			if (new[] { "TM03.2.2.1", "TM03.3.2" }.Contains(currentStage.Code))
			{
				executionDate = _calendarProvider.GetFullExaminationDate(startDate);
			}

			// Этапы, которые имеют накопительные даты (1, 10, 20)
			if (new[] { "B02.2.1", "B03.2.1.1" }.Contains(currentStage.Code))
			{
				executionDate = _calendarProvider.GetFormalExaminationDate(startDate);
			}

			// Этапы, которые имеют накопительные даты (15, 30)
			if (new[] { "TM03.3.8" }.Contains(currentStage.Code))
			{
				executionDate = _calendarProvider.GetTransferToGosreestrDate(startDate);
			}

			// Продление срока специфичное (3 месяца)
			if (currentStage.Code.Equals("B03.3.1.1.0") && request.CurrentWorkflow.FromStage.Code.Equals("B03.3.4.1"))
			{
				executionDate = _calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, 3);
			}

			if (currentStage.Code.Equals("TMI03.3.4.5.1"))
			{
				var cnt = request.PaymentInvoices.Where(pi => pi.Tariff.Code.Equals("NEW_089")).Sum(pi => pi.TariffCount);
				cnt = cnt > 6 ? 6 : cnt;
				executionDate = _calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, (short)(cnt ?? 1));
			}

			if (currentStage.Code.Equals("U02.2.6"))
			{
				var queue = _context.WorkflowTaskQueues.LastOrDefault(a => a.RequestId == request.Id && a.ConditionStage.Code == "U03.2.1");
				if (queue == null) queue = _context.WorkflowTaskQueues.LastOrDefault(a => a.RequestId == request.Id);
				var invoice = request.PaymentInvoices.LastOrDefault(pi => pi.Tariff.Code.Equals("NEW_166"));
				var cnt = invoice != null ? invoice.TariffCount.HasValue ? invoice.TariffCount.Value : 0 : 0;
				cnt = cnt > 3 ? 3 : cnt;
				executionDate = queue.ResolveDate.AddMonths(cnt);
			}

			if (currentStage.Code.Equals("U02.2.7"))
			{
				var doc = request.Documents.Any(a => a.Document.Type.Code == "001.004A") && request.PaymentInvoices.Any(pi => pi.Tariff.Code.Equals("NEW_011") /*&& pi.Status.Code != "notpaid"*/);
				executionDate = _calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, 2);
			}

			if (currentStage.Code.Equals("U03.8"))
			{
				var doc = request.Documents.Any(a => a.Document.Type.Code == "001.004A") && request.PaymentInvoices.Any(pi => pi.Tariff.Code.Equals("NEW_011") /*&& pi.Status.Code != "notpaid"*/);
				executionDate = _calendarProvider.GetExecutionDate(request.DateCreate,ExpirationType.CalendarMonth, 12);
			}

			return executionDate;
		}

		private DateTimeOffset GetStartDate(Domain.Entities.Request.Request request)
		{
			var startDate = DateTimeOffset.Now;
			var currentStageCode = request.CurrentWorkflow.CurrentStage.Code;
			var fromStageCode = request.CurrentWorkflow.FromStage?.Code;

			// Этапы, которые начинают отсчет с даты подачи заявки
			if (new[] { "TM03.2.2", "TM03.3.2", "U02.2.7" }.Contains(currentStageCode))
			{
				startDate = request.DateCreate;
			}

			// Этапы, которые начинают отсчет с даты направления запроса
			if (new[] { "TM03.3.7.1", "B03.3.1.1" }.Contains(currentStageCode))
			{
				if (fromStageCode != null && fromStageCode.Equals("TM03.2.2"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.RequestForPreExamination).DateCreate;
				}

				if (fromStageCode != null && fromStageCode.Equals("TM03.3.2"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.RequestForFullExamination).DateCreate;
				}

				if (fromStageCode != null && fromStageCode.Equals("B03.2.1"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.RequestForFormalExamForInvention).DateCreate;
				}

				if (fromStageCode != null && fromStageCode.Equals("B03.2.4"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.RequestForExaminationOfInventionPatentRequest).DateCreate;
				}
			}

			// Этапы, которые начинают отсчет с даты направления уведомления
			if (new[] { "TM03.3.7.0", "B02.2.0", "B03.2.1_0" }.Contains(currentStageCode))
			{
				if (fromStageCode != null && fromStageCode.Equals("TM03.3.7.1"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationOfAnswerTimeExpiration).DateCreate;
				}

				if (fromStageCode != null && fromStageCode.Equals("TM03.2.2.0"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationOfRegistrationExaminationTimeExpiration).DateCreate;
				}

				if (fromStageCode != null && (fromStageCode.Equals("B03.2.1") || fromStageCode.Equals("B03.2.1.1")))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationForPozitiveFormalExamination).DateCreate;
				}

				if (fromStageCode != null && (fromStageCode.Equals("B03.2.1") || fromStageCode.Equals("B03.2.1.1")))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationForPozitiveFormalExamination).DateCreate;
				}
			}

			if (new[] { "TM03.2.2.0" }.Contains(currentStageCode))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationOfTmRequestReviewingAcceptance).DateCreate;
			}

			if (new[] { "TM03.3.7" }.Contains(currentStageCode))
			{
				if (fromStageCode != null && fromStageCode.Equals("TM03.3.6"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationOfRegistrationDecision).DateCreate;
				}

				if (fromStageCode != null && fromStageCode.Equals("TM03.3.9.2.0"))
				{
					startDate = GetLastDocument(request.Id, DicDocumentType.Codes.NotificationOfTmRegistration).DateCreate;
				}
			}

			if (new[] { "TM03.3.9.1" }.Contains(currentStageCode))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.ExpertRefusalOpinionFinal).DateCreate;
			}

			// Этапы, которые начинают отсчет с даты направления заключения
			if (new[] { "TM03.3.4.4" }.Contains(currentStageCode))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.ExpertTmRegistrationOpinionWithDisclaimer).DateCreate;
			}

			if (new[] { "TM03.3.4.4.0" }.Contains(currentStageCode))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.ExpertTmRegisterRefusalOpinion).DateCreate;
			}

			if (currentStageCode.Equals("B03.3.4.1") && fromStageCode != null && fromStageCode.Equals("B03.3.3"))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse).DateCreate;
			}

			// Этапы, которые начинают отсчет с даты получения возражения
			if (new[] { "TM03.3.2.0", "TM03.3.9.2", "B03.3.4.1.0" }.Contains(currentStageCode))
			{
				startDate = GetLastDocument(request.Id, DicDocumentType.Codes.Objection).DateCreate;
			}

			return startDate;
		}

		private Domain.Entities.Document.Document GetLastDocument(int requestId, string typeCode)
		{
			var document = _context.RequestsDocuments
				.LastOrDefault(rd => rd.RequestId == requestId && rd.Document.Type.Code.Equals(typeCode))?.Document;

			if (document == null)
			{
				throw new Exception($"Document with type \"{typeCode}\" not found in request {requestId}!");
			}

			return document;
		}
	}
}