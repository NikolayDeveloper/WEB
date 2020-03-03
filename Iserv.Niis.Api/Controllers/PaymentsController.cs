using AutoMapper;
using Iserv.Niis.BusinessLogic._1CIntegration;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.PaymentExecutors;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.PaymentUses;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.WorkflowBusinessLogic.Customers;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Excel;
using Iserv.Niis.Exceptions;
using CreateRequestCommand = Iserv.Niis.WorkflowBusinessLogic.Requests.CreateRequestCommand;
using GenerateRequestNumberHandler = Iserv.Niis.WorkflowBusinessLogic.Requests.GenerateRequestNumberHandler;
using UpdateRequestCommand = Iserv.Niis.WorkflowBusinessLogic.Requests.UpdateRequestCommand;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Payments")]
    public class PaymentsController : BaseNiisApiController
    {
        private readonly IExecutor _executor;

        private readonly IMapper _mapper;

        public PaymentsController(IExecutor executor, IMapper mapper)
        {
            _executor = executor;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var paymentDtos = _executor.GetQuery<GetPaymentsQuery>().Process(q => q.Execute(Request));

            return paymentDtos.AsOkObjectResult(Response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _executor.GetQuery<GetPaymentByIdQuery>().Process(q => q.ExecuteAsync(id));
            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);
            return Ok(paymentDto);
        }

        [HttpGet("{paymentId}/returnamount")]
        public async Task<IActionResult> GetPaymentForReturnAmount(int paymentId)
        {
            var responseDto = await _executor.GetQuery<GetPaymentForReturnAmountQuery>()
                .Process(q => q.ExecuteAsync(paymentId));

            return Ok(responseDto);
        }

        [HttpPost("{paymentId}/returnamount")]
        public async Task<IActionResult> PaymentReturnAmount(int paymentId, [FromBody] PaymentReturnAmountRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            var responseDto = await _executor.GetCommand<PaymentReturnAmountCommand>()
                .Process(q => q.ExecuteAsync(paymentId, requestDto));

            return Ok(responseDto);
        }

        [HttpGet("{paymentId}/blockamount")]
        public async Task<IActionResult> GetPaymentForBlockAmount(int paymentId)
        {
            var responseDto = await _executor.GetQuery<GetPaymentForBlockAmountQuery>()
                .Process(q => q.ExecuteAsync(paymentId));

            return Ok(responseDto);
        }

        [HttpPost("{paymentId}/blockamount")]
        public async Task<IActionResult> PaymentBlockAmount(int paymentId, [FromBody] PaymentBlockAmountRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            var responseDto = await _executor.GetCommand<PaymentBlockAmountCommand>()
                .Process(q => q.ExecuteAsync(paymentId, requestDto));

            return Ok(responseDto);
        }

		[HttpGet("getpaymentsbyinvoice/{paymentInvoiceId}")]
		public async Task<IActionResult> GetPaymentsByInvoice(int paymentInvoiceId)
		{
			var responseDto = await _executor.GetQuery<GetPaymentsByInvoiceId>()
				.Process(q => q.ExecuteAsync(paymentInvoiceId));

			return Ok(responseDto);
		}

		#region Uses

		[HttpPost("uses/{ownerType}/{force}")]
        public async Task<IActionResult> Post(Owner.Type ownerType, bool force, [FromBody] PaymentUseDto useDto)
        {
            var paymentUse = _mapper.Map<PaymentUseDto, PaymentUse>(useDto);

            _executor.GetCommand<CreatePaymentUseHandler>().Process(c => c.Execute(ownerType, paymentUse, force));
            PaymentInvoice paymentInvoice = null;

            if (paymentUse.PaymentInvoiceId.HasValue)
            {
                paymentInvoice = await _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentUse.PaymentInvoiceId.Value));
            }

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestId = paymentInvoice?.RequestId;
                    NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(new RequestWorkFlowRequest
                    {
                        RequestId = requestId ?? 0,
                        PaymentInvoiceId = paymentInvoice?.Id,
                        IsAuto = true
                    });

                    await Executor.GetHandler<UnsetRequestFormalExamNotPaidHandler>().Process(h => h.ExecuteAsync(requestId ?? 0, paymentInvoice));
                    //await Executor.GetHandler<GenerateAutoNotificationHandler>().Process(h => h.Execute(requestId ?? 0));
                    break;
                case Owner.Type.ProtectionDoc:
                    if (DicTariff.Codes.GetProtectionDocSupportCodes().Contains(paymentInvoice?.Tariff?.Code) 
                            || paymentInvoice?.Tariff?.Code == DicTariff.Codes.ProtectionDocRestore)
                    {
                        var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(paymentInvoice?.ProtectionDocId ?? 0));
                        if (protectionDoc != null)
                        {
                            protectionDoc.MaintainDate = protectionDoc.MaintainDate?.AddYears(paymentInvoice?.TariffCount ?? 1) 
                                                            ?? DateTimeOffset.Now.AddYears(paymentInvoice?.TariffCount ?? 1);
                            await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));
                        }
                    }
                    NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(new ProtectionDocumentWorkFlowRequest
                    {
                        ProtectionDocId = paymentInvoice?.ProtectionDocId ?? 0,
                        IsAuto = true
                    });
                    break;
                case Owner.Type.Contract:
                    NiisWorkflowAmbientContext.Current.ContractWorkflowService.Process(new ContractWorkFlowRequest
                    {
                        ContractId = paymentInvoice?.ContractId ?? 0,
                        IsAuto = true
                    });
                    break;
            }

            return Ok(_mapper.Map<PaymentUse, PaymentUseDto>(paymentUse));
        }

        [Obsolete]
        private void ConvertRequest(int requestId)
        {
            #region Создание заявки

            var request = Executor.GetQuery<WorkflowBusinessLogic.Requests.GetRequestByIdQuery>().Process(q => q.Execute(requestId));
            var protectionDocTypeCode = request.ProtectionDocType.Code;
            DicProtectionDocType newProtectionDocType = null;
            DicProtectionDocSubType newProtectionDocSubType = null;
            string initialStageCode = null;
            switch (protectionDocTypeCode)
            {
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    newProtectionDocType = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(q =>
                        q.Execute(DicProtectionDocTypeCodes.RequestTypeInventionCode));
                    initialStageCode = RouteStageCodes.I_02_1;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    newProtectionDocType = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(q =>
                        q.Execute(DicProtectionDocTypeCodes.RequestTypeUsefulModelCode));
                    initialStageCode = RouteStageCodes.UM_02_1;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    initialStageCode = request.CurrentWorkflow.FromStage.Code;
                    newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                        .Process(q => q.Execute(DicProtectionDocSubtypeCodes.CollectiveTrademark));
                    break;
            }
            if (initialStageCode == null)
            {
                return;
            }
            var newRequest = _mapper.Map<Request>(request);
            newRequest.ProtectionDocTypeId = newProtectionDocType?.Id ?? request.ProtectionDocTypeId;
            newRequest.RequestTypeId = newProtectionDocSubType?.Id ?? request.RequestTypeId;
            newRequest.DateCreate = DateTimeOffset.Now;
            Executor.GetHandler<GenerateRequestNumberHandler>().Process(h => h.Execute(newRequest));
            var newRequestId = Executor.GetCommand<CreateRequestCommand>().Process(c => c.Execute(newRequest));

            var initialStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>()
                .Process(q => q.Execute(initialStageCode));

            var initialWorkflow = new RequestWorkflow
            {
                CurrentUserId = request.CurrentWorkflow.FromUserId,
                OwnerId = newRequestId,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };
            Executor.GetCommand<CreateRequestWorkflowCommand>()
                .Process(c => c.Execute(initialWorkflow));

            newRequest.CurrentWorkflowId = initialWorkflow.Id;
            Executor.GetCommand<UpdateRequestCommand>().Process(c => c.Execute(newRequest));

            #endregion

            #region Биб. данные

            var newConventionInfos = _mapper.Map<RequestConventionInfo[]>(request.RequestConventionInfos);
            foreach (var conventionInfo in newConventionInfos)
            {
                conventionInfo.RequestId = newRequestId;
                Executor.GetCommand<CreateConventionInfoCommand>().Process(c => c.Execute(conventionInfo));
            }

            var newEarlyRegs = _mapper.Map<RequestEarlyReg[]>(request.EarlyRegs);
            foreach (var earlyReg in newEarlyRegs)
            {
                earlyReg.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestEarlyRegCommand>().Process(c => c.Execute(earlyReg));
            }

            var newIcgsRequests = _mapper.Map<ICGSRequest[]>(request.ICGSRequests);
            foreach (var icgsRequest in newIcgsRequests)
            {
                icgsRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestIcgsCommand>().Process(c => c.Execute(icgsRequest));
            }

            var newIpcRequests = _mapper.Map<IPCRequest[]>(request.IPCRequests);
            foreach (var ipcRequest in newIpcRequests)
            {
                ipcRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateIpcRequestCommand>().Process(c => c.Execute(ipcRequest));
            }

            var newIcisRequests = _mapper.Map<ICISRequest[]>(request.ICISRequests);
            foreach (var icisRequest in newIcisRequests)
            {
                icisRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestIcisCommand>().Process(c => c.Execute(icisRequest));
            }

            foreach (var colorTz in request.ColorTzs)
            {
                var newColorTz = new DicColorTZRequestRelation
                {
                    ColorTzId = colorTz.ColorTzId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestColorTzCommand>().Process(c => c.Execute(newColorTz));
            }

            foreach (var icfem in request.Icfems)
            {
                var newIcfem = new DicIcfemRequestRelation
                {
                    DicIcfemId = icfem.DicIcfemId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestIcfemCommand>().Process(c => c.Execute(newIcfem));
            }

            #endregion

            #region Контрагенты

            var newCustomers = _mapper.Map<RequestCustomer[]>(request.RequestCustomers);
            foreach (var customer in newCustomers)
            {
                customer.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(c => c.Execute(customer));
            }

            #endregion
        }

        #endregion

        #region Invoices

        /// <summary>
        /// Возвращает ответ со списком выставленных счетов для родительской сущности по его идентификатору и типу.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <returns>Ответ со списком выставленных счетов.</returns>
        [HttpGet("invoices/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetInvoices(int ownerId, Owner.Type ownerType)
        {
            var paymentInvoices = await GetPaymentInvoicesRequest(ownerId, ownerType);

            return Ok(paymentInvoices);
        }

        [HttpGet("GetInvoicesExcel/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetInvoicesExcel(int ownerId, Owner.Type ownerType)
        {
            var paymentInvoices = await GetPaymentInvoicesRequest(ownerId, ownerType);

            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(paymentInvoices, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        [HttpPost("invoices/{ownerType}")]
        public async Task<IActionResult> Post(Owner.Type ownerType, [FromBody] PaymentInvoiceDto invoiceDto)
        {
            var newPaymentInvoice =
                _mapper.Map<PaymentInvoiceDto, PaymentInvoice>(invoiceDto, opt => opt.Items["OwnerType"] = ownerType);
            newPaymentInvoice.CreateUserId = NiisAmbientContext.Current.User.Identity.UserId;
            var paymentInvoiceId = await _executor.GetCommand<CreatePaymentInvoiceCommand>()
                .Process(c => c.ExecuteAsync(newPaymentInvoice));
            var paymentInvoice = await _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentInvoiceId));
            PaymentInvoiceDto paymentInvoiceDto;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    if (!paymentInvoice.RequestId.HasValue)
                    {
                        throw new ArgumentNullException(nameof(paymentInvoice.RequestId));
                    }

                    var request = await _executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.RequestId.Value));
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
                    break;
                case Owner.Type.Contract:
                    if (!paymentInvoice.ContractId.HasValue)
                    {
                        throw new ArgumentNullException(nameof(paymentInvoice.ContractId));
                    }

                    var contract = await _executor.GetQuery<GetContractByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.ContractId.Value));
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
                    break;
                case Owner.Type.ProtectionDoc:
                    if (!paymentInvoice.ProtectionDocId.HasValue)
                    {
                        throw new ArgumentNullException(nameof(paymentInvoice.ProtectionDocId));
                    }

                    var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.ProtectionDocId.Value));
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
                    break;
                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }

            return Ok(paymentInvoiceDto);
        }

        [HttpPost("addRangePaymentInvoice/{ownerType}")]
        public async Task<IActionResult> AddRangePaymentInvoice(Owner.Type ownerType,
            [FromBody] PaymentInvoiceDto[] invoiceDtos)
        {
            var newPaymentInvoices = _mapper
                .Map<IEnumerable<PaymentInvoiceDto>, IEnumerable<PaymentInvoice>>(invoiceDtos,
                    opt => opt.Items["OwnerType"] = ownerType)
                .ToList();

            await _executor.GetCommand<CreateRangePaymentInvoiceCommand>().Process(c => c.ExecuteAsync(newPaymentInvoices));
            var paymentInvoices = _executor.GetQuery<GetPaymentInvoicesByIdsQuery>()
                .Process(q => q.Execute(newPaymentInvoices.Select(x => x.Id).ToArray()));

            List<PaymentInvoiceDto> paymentInvoiceDtos;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestId = paymentInvoices.FirstOrDefault(c => c.RequestId.HasValue)?.RequestId;
                    if (requestId.HasValue == false)
                    {
                        throw new ArgumentNullException(nameof(requestId));
                    }

                    var request = await _executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(requestId.Value));
                    paymentInvoiceDtos = _mapper.Map<List<PaymentInvoice>, List<PaymentInvoiceDto>>(paymentInvoices,
                        opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
                    break;
                case Owner.Type.Contract:
                    var contractId = paymentInvoices.FirstOrDefault(c => c.ContractId.HasValue)?.ContractId;
                    if (contractId.HasValue == false)
                    {
                        throw new ArgumentNullException(nameof(contractId));
                    }

                    var contract = await _executor.GetQuery<GetContractByIdQuery>()
                        .Process(q => q.ExecuteAsync(contractId.Value));

                    paymentInvoiceDtos = _mapper.Map<List<PaymentInvoice>, List<PaymentInvoiceDto>>(paymentInvoices,
                        opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDocId = paymentInvoices.FirstOrDefault(c => c.ProtectionDocId.HasValue)
                        ?.ProtectionDocId;
                    if (protectionDocId.HasValue == false)
                    {
                        throw new ArgumentNullException(nameof(protectionDocId));
                    }

                    var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(protectionDocId.Value));
                    paymentInvoiceDtos = _mapper.Map<List<PaymentInvoice>, List<PaymentInvoiceDto>>(paymentInvoices,
                        opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
                    break;
                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }

            return Ok(paymentInvoiceDtos);
        }



        [HttpPost("invoices/chargePaymentInvoice")]
        public async Task<IActionResult> ChargePaymentInvoice([FromBody] ChargePaymentInvoiceDto chargePaymentInvoice)
        {

            var paymentInvoiceItem = await Executor.GetHandler<GetPaymentInvoiceByIdQuery>().Process(h =>
                    h.ExecuteAsync(chargePaymentInvoice.PaymentInvoiceId));


            var ownerType = (Owner.Type)Enum.ToObject(typeof(Owner.Type), chargePaymentInvoice.OwnerType);
            var paymentInvoiceDto = new PaymentInvoiceDto();
            switch (ownerType)
            {
                case Owner.Type.Request:
                    paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(new PaymentInvoice[] { paymentInvoiceItem },
                            opt => opt.Items["RequestCustomers"] = paymentInvoiceItem.Request.RequestCustomers).FirstOrDefault();

                    break;
                case Owner.Type.ProtectionDoc:
                    paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(new PaymentInvoice[] { paymentInvoiceItem },
                                            opt => opt.Items["ProtectionDocCustomers"] = paymentInvoiceItem.ProtectionDoc.ProtectionDocCustomers).First();
                    break;
                case Owner.Type.Contract:
                    paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(new PaymentInvoice[] { paymentInvoiceItem },
                                            opt => opt.Items["ContractCustomers"] = paymentInvoiceItem.Contract.ContractCustomers).First();

                    break;
                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));


            }

            var notPaidStatus = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>()
                .Process(q => q.Execute(DicPaymentStatusCodes.Notpaid));

            if (paymentInvoiceItem.StatusId == notPaidStatus.Id)
            {
                if (paymentInvoiceDto.Remainder > 100)
                {
                    return Ok("Невозможно списать оплату. Сумма фактической оплаты меньше Суммы с НДС, выставленной оплаты, более чем на 100 тенге. Оплатите счёт и повторите попытку");
                }
            }

            DateTimeOffset date;
            if (!DateTimeOffset.TryParse(chargePaymentInvoice.ChargeDate, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out date))
            {
                return Ok("Невозможно преобразовать дату:" + chargePaymentInvoice.ChargeDate);
            };


            await Executor.GetHandler<ChargePaymentInvoiceByIdOutgoingDateHandler>().Process(h =>
                    h.ExecuteAsync(ownerType, chargePaymentInvoice.PaymentInvoiceId, date));

            return Ok("Оплата успешно списана");
        }


        [HttpPost("invoices/{paymentInvoiceId}/deleteChargedPaymentInvoice")]
        public async Task<IActionResult> DeleteChargedPaymentInvoice(int paymentInvoiceId, [FromBody] DeleteChargedPaymentInvoiceDto request)
        {
            await Executor.GetHandler<DeleteChargedPaymentInvoiceCommand>().Process(h =>
                    h.ExecuteAsync(paymentInvoiceId, request));

            return Ok();
        }
        [HttpPost("invoices/{paymentInvoiceId}/deletePaymentInvoice")]
        public async Task<IActionResult> DeletePaymentInvoice(int paymentInvoiceId, DeletePaymentInvoiceDto request)
        {
            var response = await Executor.GetHandler<DeletePaymentInvoiceCommand>().Process(h =>
                    h.ExecuteAsync(paymentInvoiceId, request));

            return Ok(response);
        }

        [HttpPost("invoices/{paymentInvoiceId}/editChargedPaymentInvoice")]
        public async Task<IActionResult> EditChargedPaymentInvoice(int paymentInvoiceId, [FromBody] EditChargedPaymentInvoiceDto request)
        {
            var paymentInvoiceItem = await Executor.GetHandler<GetPaymentInvoiceByIdQuery>().Process(h =>
                    h.ExecuteAsync(paymentInvoiceId));

            var ownerType = (Owner.Type)Enum.ToObject(typeof(Owner.Type), request.OwnerType);
            var user = Executor.GetCommand<GetUserByIdQuery>().Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            DateTimeOffset editDate;
            if (!DateTimeOffset.TryParse(request.EditDate, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out editDate))
            {
                return Ok("Невозможно преобразовать дату:" + request.EditDate);
            };

            paymentInvoiceItem.ReasonOfChangingChargedPaymentInvoice = request.EditReason;
            paymentInvoiceItem.DateOfChangingChargedPaymentInvoice = editDate;
            paymentInvoiceItem.DateComplete = editDate;
            paymentInvoiceItem.EmployeeAndPositonWhoChangedChargedPaymentInvoice = $"{user.NameRu} {user.Position.NameRu}";


            Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoiceItem));

            var isExportedTo1C = await Executor.GetHandler<ExportPaymentTo1CHandler>().Process(r => r.ExecuteAsync(ownerType, paymentInvoiceId,
                    PaymentInvoiveChangeFlag.PaymentInvoiceChargedDateIsChanged));

            return Ok("Дата списания изменена.");
        }

        #endregion

        #region Executors

        [HttpPost("executor")]
        public async Task<IActionResult> PostExecutor([FromBody] PaymentExecutorDto executorDto)
        {
            foreach (var tariffId in executorDto.TariffIds)
            {
                var executor = new PaymentExecutor
                {
                    RequestId = executorDto.OwnerId,
                    UserId = executorDto.UserId,
                    TariffId = tariffId
                };
                await Executor.GetCommand<CreatePaymentExecutorCommand>().Process(c => c.ExecuteAsync(executor));
            }
            return NoContent();
        }

        [HttpGet("requiredTariffs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetRequiredTariffs(Owner.Type ownerType, int ownerId)
        {
            int stageId;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    stageId = request?.CurrentWorkflow?.CurrentStageId ?? 0;
                    break;
                default:
                    throw new NotImplementedException();
            }
            var requiredTariffs = await Executor.GetQuery<GetRequiredTariffsByStageIdQuery>()
                .Process(q => q.ExecuteAsync(stageId));
            var result = Mapper.Map<List<SelectOptionDto>>(requiredTariffs.ToList());

            return Ok(result);
        }

        #endregion

        /// <summary>
        /// Возвращает ответ со списком выставленных счетов для родительской сущности по его идентификатору и типу.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <returns>Список выставленных счетов.</returns>
        private async Task<IEnumerable<PaymentInvoiceDto>> GetPaymentInvoicesRequest(int ownerId, Owner.Type ownerType)
        {
            IEnumerable<PaymentInvoiceDto> paymentInvoices;

            switch (ownerType)
            {
                case Owner.Type.Request:
                    paymentInvoices = await GetPaymentInvoicesRequest(ownerId);
                    break;

                case Owner.Type.Contract:
                    paymentInvoices = await GetPaymentInvoicesContract(ownerId);
                    break;

                case Owner.Type.ProtectionDoc:
                    paymentInvoices = await GetPaymentInvoicesProtectionDoc(ownerId);
                    break;

                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }

            return paymentInvoices;
        }

        /// <summary>
        /// Возвращает ответ со списком выставленных счетов для родительской сущности по его идентификатору и типу для заявок.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <returns>Список выставленных счетов.</returns>
        private async Task<IEnumerable<PaymentInvoiceDto>> GetPaymentInvoicesRequest(int ownerId)
        {
            var request = await _executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

            if (request == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Read, ownerId);

            var requestInvoices = await _executor.GetQuery<GetPaymentInvoicesByRequestIdQuery>()
                .Process(q => q.ExecuteAsync(ownerId));

            var paymentInvoices = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(requestInvoices,
                opt => opt.Items["RequestCustomers"] = request.RequestCustomers);

            return paymentInvoices;
        }


        /// <summary>
        /// Возвращает ответ со списком выставленных счетов для родительской сущности по его идентификатору и типу для контрактов.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <returns>Список выставленных счетов.</returns>
        private async Task<IEnumerable<PaymentInvoiceDto>> GetPaymentInvoicesContract(int ownerId)
        {
            var contract = await _executor.GetQuery<GetContractByIdQuery>()
                .Process(q => q.ExecuteAsync(ownerId));

            if (contract == null)
                throw new DataNotFoundException(nameof(Contract),
                    DataNotFoundException.OperationType.Read, ownerId);

            var contractInvoices = await _executor.GetQuery<GetPaymentInvoicesByContractIdQuery>()
                .Process(q => q.ExecuteAsync(ownerId));

            var paymentInvoices = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(contractInvoices,
                opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);

            return paymentInvoices;
        }

        /// <summary>
        /// Возвращает ответ со списком выставленных счетов для родительской сущности по его идентификатору и типу для охранного документа.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <returns>Список выставленных счетов.</returns>
        private async Task<IEnumerable<PaymentInvoiceDto>> GetPaymentInvoicesProtectionDoc(int ownerId)
        {
            var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.ExecuteAsync(ownerId));

            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc),
                    DataNotFoundException.OperationType.Read, ownerId);

            var protectionDocInvoices = _executor.GetQuery<GetPaymentInvoicesByProtectionDocIdQuery>()
                .Process(q => q.Execute(ownerId));

            var paymentInvoices = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(protectionDocInvoices,
                opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);

            return paymentInvoices;
        }
    }
}