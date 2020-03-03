using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Models;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.Extensions.Configuration;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic._1CIntegration
{
	public class ExportPaymentTo1CHandler : BaseHandler
	{
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly IConnectTo1CService _connectTo1C;

		public ExportPaymentTo1CHandler(IMapper mapper, IConfiguration configuration, IConnectTo1CService connectTo1C)
		{
			_mapper = mapper;
			_configuration = configuration;
			_connectTo1C = connectTo1C;
		}

		public async Task<bool> ExecuteAsync(Owner.Type ownerType, int paymentInvoiceId,
											 PaymentInvoiveChangeFlag changeFlag = PaymentInvoiveChangeFlag.NewChargedPaymentInvoice,
											 string status = null,
											 DateTimeOffset? chargedDate = null)
		{
			bool returnValue = false;
			try
			{

				var paymentInvoice = await Executor.GetQuery<GetPaymentInvoiceByIdQuery>()
					 .Process(q => q.ExecuteAsync(paymentInvoiceId));

				var paymentInvoices = new PaymentInvoice[] { paymentInvoice };

				var RequestNums = string.Empty;
				var RequestTypes = string.Empty;
				var LinkedProtectionDocNums = string.Empty;
				var ProtectionDocTypes = string.Empty;
				var ContractNums = string.Empty;
				var ContractTypes = string.Empty;
				var paymentInvoiceDto = new PaymentInvoiceDto();
				var DepartmentName = string.Empty;
				var ProtectionDocTypeNumber = string.Empty;

				switch (ownerType)
				{
					case Owner.Type.Contract:
						var contractItem = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(paymentInvoice.ContractId.Value));
						var contractDetailDto = _mapper.Map<Contract, ContractDetailDto>(contractItem, opt => opt.Items[nameof(contractItem.ContractCustomers)] = contractItem.ContractCustomers);
						ContractNums = contractItem.GosNumber;
						ContractTypes = contractItem.Type.NameRu;
						DepartmentName = contractItem.Department.NameRu;

						var requestIds = contractDetailDto.RequestRelations.Select(p => p.Request.Id);
						var requests = Executor.GetQuery<GetRequestsByIdsQuery>()
													.Process(q => q.Execute(requestIds.ToArray())).ToArray();

						RequestNums = contractItem.GosNumber;
						RequestTypes = string.Join("; ", requests.Select(r => r.RequestType.NameRu));

						var projectedDocIds = contractDetailDto.ProtectionDocRelations.Select(p => p.ProtectionDoc.Id);
						var protecedDocs = Executor.GetQuery<GetProtectionDocsByIdsQuery>()
													.Process(q => q.Execute(projectedDocIds.ToArray())).ToArray();

						LinkedProtectionDocNums = string.Join("; ", protecedDocs.Select(pd => pd.GosNumber));
						ProtectionDocTypes = string.Join("; ", protecedDocs.Select(pd => pd.Type.NameRu));

						paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(paymentInvoices,
												opt => opt.Items["ContractCustomers"] = paymentInvoice.Contract.ContractCustomers).First();
						break;

					case Owner.Type.ProtectionDoc:
						ProtectionDocTypes = paymentInvoice.ProtectionDoc.Type.NameRu;
						RequestNums = paymentInvoice.ProtectionDoc.GosNumber;
						ProtectionDocTypeNumber = paymentInvoice.ProtectionDoc.GosNumber;
						RequestTypes = paymentInvoice.ProtectionDoc.Request?.RequestType?.NameRu;
						DepartmentName = paymentInvoice.ProtectionDoc.Request?.Department.NameRu;

						paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(paymentInvoices,
												opt => opt.Items["ProtectionDocCustomers"] = paymentInvoice.ProtectionDoc.ProtectionDocCustomers).First();
						break;

					case Owner.Type.Request:
						if (paymentInvoice.Request.Contracts.Any())
						{
							var contractId = paymentInvoice.Request.Contracts.First().ContractId;
							contractItem = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractId)).Result;

							ContractNums = contractItem.GosNumber;
							ContractTypes = contractItem.Type.NameRu;
						}


						RequestNums = paymentInvoice.Request.RequestNum;
						RequestTypes = paymentInvoice.Request.RequestType?.NameRu;
						DepartmentName = paymentInvoice.Request.Department.NameRu;

						paymentInvoiceDto = _mapper.Map<IEnumerable<PaymentInvoiceDto>>(paymentInvoices,
							opt => opt.Items["RequestCustomers"] = paymentInvoice.Request.RequestCustomers).First();
						break;

					default:
						throw new NotImplementedException();
				}

				var exportTo1CList = new List<ExoprtPaymentTo1CModel>();
				if (((status != null && status == DicPaymentStatusCodes.Charged) || paymentInvoice.Status.Code == DicPaymentStatusCodes.Charged)
					&& (paymentInvoice.DateExportedTo1C == null || changeFlag != PaymentInvoiveChangeFlag.NewChargedPaymentInvoice))
				{
					var paymentsLinkedToInvoive = paymentInvoice.PaymentUses.GroupBy(p => p.PaymentId);

					foreach (var paymentUse in paymentsLinkedToInvoive)
					{
						var paymentID = paymentUse.First().PaymentId.Value;
						var paymentUseID = paymentUse.First().Id;

						var amount = paymentUse.Sum(p => p.Amount);
						var paymentItem = await Executor.GetQuery<GetPaymentByIdQuery>()
								.Process(q => q.ExecuteAsync(paymentID));

						var exportTo1C = new ExoprtPaymentTo1CModel();

						exportTo1C.PaymentUseId = paymentUseID.ToString();
						exportTo1C.Payment1CNumber = paymentItem.Payment1CNumber;
						exportTo1C.PaymentDate = paymentItem.PaymentDate;
						exportTo1C.PaymentAmount = paymentItem.Amount.Value;
						exportTo1C.PurposeDescription = paymentItem.PurposeDescription;
						exportTo1C.Payer = paymentItem.Payer;
						exportTo1C.PaymentID = paymentItem.Id;
						exportTo1C.PaymentCNumberBVU = paymentItem.PaymentCNumberBVU;
						exportTo1C.PayerBinOrInn = paymentItem.PayerBinOrInn;
						exportTo1C.PayerRNN = paymentItem.PayerRNN;
						exportTo1C.TariffCode = paymentInvoice.Tariff.Code;
						exportTo1C.TariffName = paymentInvoice.Tariff.NameRu;
						exportTo1C.LinkedProtectionDocNums = LinkedProtectionDocNums;
						exportTo1C.ProtectionDocTypeNumber = ProtectionDocTypeNumber;
						exportTo1C.RequestNum = RequestNums;
						exportTo1C.ProtectionDocType = ProtectionDocTypes;
						exportTo1C.RequestType = RequestTypes;
						exportTo1C.ContractNum = ContractNums;
						exportTo1C.ContractType = ContractTypes;

						exportTo1C.TotalAmountNds = amount;
						var statusName = paymentInvoiceDto.StatusNameRu;

						if (status != null)
						{
							statusName = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>()
										.Process(q => q.Execute(status)).NameRu;
						}

						exportTo1C.StatusName = statusName;
						exportTo1C.CreditDate = paymentInvoiceDto.CreditDate;
						exportTo1C.CreditUser = paymentInvoiceDto.CreditUser;
						exportTo1C.WriteOffDate = paymentInvoiceDto.WriteOffDate;

						if (changeFlag == PaymentInvoiveChangeFlag.NewChargedPaymentInvoice)
						{
							exportTo1C.WriteOffDate = chargedDate.Value;
						}

						exportTo1C.WriteOffUser = paymentInvoiceDto.WriteOffUser;
						exportTo1C.DepartmentName = DepartmentName;
						exportTo1C.ChangeFlag = changeFlag;

						if (changeFlag == PaymentInvoiveChangeFlag.PaymentInvoiceChargedDateIsChanged)
						{
							exportTo1C.DeleteChangeDate = paymentInvoice.DateOfChangingChargedPaymentInvoice;
							exportTo1C.EmployeeAndPositonWhoChangedRecord = paymentInvoice.EmployeeAndPositonWhoChangedChargedPaymentInvoice;
							exportTo1C.DeleteChangeReason = paymentInvoice.ReasonOfChangingChargedPaymentInvoice;
						}
						else if (changeFlag == PaymentInvoiveChangeFlag.PaymentInvoiceChargedDateIsDeleted)
						{
							exportTo1C.DeleteChangeDate = paymentInvoice.DateOfDeletingChargedPaymentInvoice;
							exportTo1C.EmployeeAndPositonWhoChangedRecord = paymentInvoice.EmployeeAndPositonWhoDeleteChargedPaymentInvoice;
							exportTo1C.DeleteChangeReason = paymentInvoice.ReasonOfDeletingChargedPaymentInvoice;
						}

						exportTo1CList.Add(exportTo1C);
					}
				}

				dynamic connector = null;
				dynamic connection = null;
				dynamic newRecord = null;
				try
				{
					var connectionString = GetConnectionString();
					connector = Activator.CreateInstance(Type.GetTypeFromProgID("V83.COMConnector"));
					connection = connector.Connect(connectionString);

					foreach (var item in exportTo1CList)
					{
						newRecord = connection.РегистрыСведений.ИнтеграцияНИИС.СоздатьМенеджерЗаписи();

						newRecord.КодПодраз = item.DepartmentName;
						newRecord.PaymentID = item.PaymentID.ToString();
						newRecord.PaymentUseId = item.PaymentUseId;
						newRecord.БИН = item.PayerBinOrInn;

						if (item.WriteOffDate.HasValue)
						{
							newRecord.ДатаСписания = item.WriteOffDate.Value.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
						}

						newRecord.Сумма = item.TotalAmountNds;
						newRecord.РНН = item.PayerRNN;
						newRecord.ИмяКонтры = item.Payer;
						newRecord.СтатусОплаты = item.StatusName;
						newRecord.ВидЗаявления = item.ContractType;
						newRecord.НомерДоговора = item.ContractNum;
						newRecord.ВидОПС = item.ProtectionDocType;
						newRecord.НомерЗаявки = item.RequestNum;
						newRecord.КодГруппыЗаявки = item.RequestType;
						newRecord.СвязанныеОбъекты = item.LinkedProtectionDocNums;
						newRecord.НазваниеУслуги = item.TariffName;
						newRecord.КодГруппыУслуги = item.TariffCode;
						newRecord.НомерДокумента1С = item.PaymentCNumberBVU;
						newRecord.НомерПлатежа = item.Payment1CNumber;
						newRecord.НомерОД = item.ProtectionDocTypeNumber;

						if (item.PaymentDate.HasValue)
						{
							newRecord.ДатаПлатежа = item.PaymentDate.Value.ToLocalTime().ToString("dd.MM.yyyy");
						}

						newRecord.Назначение = item.PurposeDescription;
						newRecord.ПричинаУдаленияИзмененияЗаписи = item?.DeleteChangeReason;
						newRecord.ФИО_ДолжностьСотрудника_УдалившегоИзменившегоЗапись = item?.EmployeeAndPositonWhoChangedRecord;

						if (item.DeleteChangeDate.HasValue)
						{
							newRecord.ДатаУдаленияИзмененияСписаннойОплаты = item.DeleteChangeDate.Value.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
						}

						newRecord.Признак = (int)item.ChangeFlag;
						newRecord.ИсполнительСписал = item.WriteOffUser;
						newRecord.ИсполнительЗачел = item.CreditUser;

						if (item.CreditDate.HasValue)
						{
							newRecord.ДатаЗачтенияОплаты = item.CreditDate.Value.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
						}

						newRecord.Записать();

						string message = $"Данные о списанном платеже переданы:  {DateTimeOffset.UtcNow}, " +
							$"{item.PaymentCNumberBVU}, " +
							$"{item.PayerBinOrInn}, " +
							$"{item.PayerRNN}, " +
							$"{item.PaymentAmount}, " +
							$"{item.PaymentDate.Value}, " +
							$"{item.TariffCode}, " +
							$"{item.ProtectionDocTypeNumber}, " +
							$"{item.RequestNum}, " +
							$"{item.ProtectionDocType}, " +
							$"{item.ContractNum}, " +
							$"{item.ContractType}, " +
							$"{item.TotalAmountNds}, " +
							$"{item.StatusName}, " +
							$"{(int)item.ChangeFlag }.";

						var log = new LogRecord();
						log.LogType = LogType.ExportTo1C;
						log.LogErrorType = LogErrorType.Information;
						log.Message = message;

						var logId = await Executor.GetCommand<CreateLogRecordCommand>().Process(q => q.ExecuteAsync(log));
					}
				}
				catch (Exception exc)
				{
					string message = $"Ошибка подключения к БД 1С {DateTimeOffset.UtcNow}, {exc.Message}, {exc.InnerException} ,{exc.StackTrace}";

					var log = new LogRecord();
					log.LogType = LogType.ExportTo1C;
					log.LogErrorType = LogErrorType.Error;
					log.Message = message;

					var logId = await Executor.GetCommand<CreateLogRecordCommand>().Process(q => q.ExecuteAsync(log));

					if (newRecord != null)
					{
						Marshal.ReleaseComObject(newRecord);
					}
					if (connection != null)
					{
						Marshal.ReleaseComObject(connection);
					}
					if (connector != null)
					{
						Marshal.ReleaseComObject(connector);
					}

					returnValue = false;
					throw;
				}

				if (newRecord != null)
				{
					Marshal.ReleaseComObject(newRecord);
				}
				if (connection != null)
				{
					Marshal.ReleaseComObject(connection);
				}
				if (connector != null)
				{
					Marshal.ReleaseComObject(connector);
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				var log = new LogRecord();
				log.LogType = LogType.General;
				log.LogErrorType = LogErrorType.Error;
				log.Message = "Error 1C Export" + ex.StackTrace;
				var logId = await Executor.GetCommand<CreateLogRecordCommand>().Process(q => q.ExecuteAsync(log));
			}
			return returnValue;
		}

		private string GetConnectionString()
		{
			return _connectTo1C.GetConnectionString();
		}
	}
}