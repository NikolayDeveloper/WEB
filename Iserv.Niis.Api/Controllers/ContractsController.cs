using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic.ContractCustomers;
using Iserv.Niis.BusinessLogic.ContractRequestRelations;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers;
using Iserv.Niis.BusinessLogic.Workflows.Contracts;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.BusinessLogic.ContractProtectionDocRelations;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Intergrations;
using Iserv.Niis.Domain.Intergrations.SendRegNumber;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Utils.Helpers;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContractsController : BaseNiisApiController
    {
        #region Fields
        private readonly ILkIntergarionHelper _lkIntergarionHelper;
        private readonly DictionaryHelper _dictionaryHelper;

        private readonly string _сorrespondenceCustomerRole = DicCustomerRole.Codes.Correspondence;
        #endregion

        public ContractsController(
            DictionaryHelper dictionaryHelper,
            ILkIntergarionHelper lkIntergarionHelper)
        {
            _dictionaryHelper = dictionaryHelper;
            _lkIntergarionHelper = lkIntergarionHelper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (contract == null)
            {
                throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Read, id);
            }

            if (contract.IsRead == false && contract.CurrentWorkflow.CurrentUserId == NiisAmbientContext.Current.User.Identity.UserId)
            {
                Executor.GetCommand<UpdateContractAsReadCommand>().Process(c => c.Execute(id));
            }

            var contractDetailDto = Mapper.Map<Contract, ContractDetailDto>(contract, opt => opt.Items[nameof(contract.ContractCustomers)] = contract.ContractCustomers);
            return Ok(contractDetailDto);
        }

        [HttpGet("byOwner/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetContractsByOwner(Owner.Type ownerType, int ownerId)
        {
            List<Contract> contracts = null;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    contracts = await Executor.GetQuery<GetContractsByRequestIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    break;
                case Owner.Type.ProtectionDoc:
                    contracts = await Executor.GetQuery<GetContractsByProtectionDocIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    break;
                default:
                    throw new NotImplementedException();
            }
            var contractDtos = Mapper.Map<List<ContractItemDto>>(contracts);
            return Ok(contractDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContractDetailDto contractDetailDto)
        {
            var contractId = await Executor.GetCommand<CreateContractCommand>().Process(c => c.ExecuteAsync(contractDetailDto));
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractId));



            var addressee = contract.Addressee;
            if (addressee != null /*&& string.IsNullOrWhiteSpace(contractDetailDto.Apartment) == false*/)
            {
                addressee.Apartment = contractDetailDto.Apartment;
                addressee.Address = contractDetailDto.AddresseeAddress;
                

                var сorrespondenceСustomerRoleId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerRole), _сorrespondenceCustomerRole);
                var сorrespondenceCustomer = new ContractCustomer()
                {
                    CustomerId = contract.AddresseeId,
                    ContractId = contractId,
                    CustomerRoleId = сorrespondenceСustomerRoleId,
                    Address = addressee.Address,
                    AddressEn = addressee.AddressEn,
                    AddressKz = addressee.AddressKz,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now
                };

                await Executor.GetCommand<CreateContractCustomerCommand>().Process(q => q.ExecuteAsync(сorrespondenceCustomer));
                await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
            }

            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var contractWorkflow = await Executor.GetQuery<GetInitialContractWorkflowQuery>().Process(q => q.ExecuteAsync(contract, userId));
            if (contractWorkflow != null)
            {
                await Executor.GetHandler<ProcessContractWorkflowHandler>().Process(h => h.Handle(contractWorkflow, userId));
            }

            var contractRequestRelationsDtos = new List<ContractRequestRelationDto>();
            var contractProtectionDocRelationDtos = new List<ContractProtectionDocRelationDto>();

            foreach (var ownerDto in contractDetailDto.Owners)
            {
                contractRequestRelationsDtos.Add(new ContractRequestRelationDto
                {
                    ContractId = contractId,
                    Request = new RequestItemDto { Id = ownerDto.OwnerId }
                });
            }

            foreach (var ownerDto in contractDetailDto.ProtectionDocsOwners)
            {
                contractProtectionDocRelationDtos.Add(new ContractProtectionDocRelationDto
                {
                    ContractId = contractId,
                    ProtectionDoc = new ProtectionDocItemDto { Id = ownerDto.OwnerId }
                });
            }

            // var contractRequestRelationsDtos = contractDetailDto.RequestRelations.ToList();
            Executor.CommandChain()
                .AddCommand<DeleteContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<CreateContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<UpdateContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<DeleteContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .AddCommand<CreateContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .AddCommand<UpdateContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .ExecuteAllWithTransaction();

            var createdContract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractId));
            var createdContractDetailDto = Mapper.Map<Contract, ContractDetailDto>(createdContract);

            return Ok(createdContractDetailDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContractDetailDto contractDetailDto)
        {
            var contractId = contractDetailDto.Id = id;

            var contractRequestRelationsDtos = new List<ContractRequestRelationDto>();
            var contractProtectionDocRelationDtos = new List<ContractProtectionDocRelationDto>();

            foreach (var ownerDto in contractDetailDto.Owners)
            {
                contractRequestRelationsDtos.Add(new ContractRequestRelationDto
                {
                    ContractId = contractId,
                    Request = new RequestItemDto { Id = ownerDto.OwnerId }
                });
            }

            foreach (var ownerDto in contractDetailDto.ProtectionDocsOwners)
            {
                contractProtectionDocRelationDtos.Add(new ContractProtectionDocRelationDto
                {
                    ContractId = contractId,
                    ProtectionDoc = new ProtectionDocItemDto { Id = ownerDto.OwnerId }
                });
            }

            var contract = Executor.GetQuery<GetContractByIdWithoutIncludingQuery>().Process(q => q.Execute(contractDetailDto.Id));
            Mapper.Map(contractDetailDto, contract);

            Executor.CommandChain()
                .AddCommand<UpdateContractCommand>(c => c.Execute(contract))
                .AddCommand<DeleteContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<CreateContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<UpdateContractRequestRelationsCommand>(c => c.Execute(contractId, contractRequestRelationsDtos))
                .AddCommand<DeleteContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .AddCommand<CreateContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .AddCommand<UpdateContractProtectionDocRelationsCommand>(c => c.Execute(contractId, contractProtectionDocRelationDtos))
                .ExecuteAllWithTransaction();

            var updatedContract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractId));

            var addressee = updatedContract.Addressee;
            if (addressee != null)
            {
                addressee.Apartment = contractDetailDto.Apartment;
                addressee.Address = contractDetailDto.AddresseeAddress;
                await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
            }

            var updatedContractDetailDto = Mapper.Map<Contract, ContractDetailDto>(updatedContract);

            var contractWorkFlowRequest = new ContractWorkFlowRequest
            {
                ContractId = updatedContract.Id,
                NextStageUserId = updatedContract.CurrentWorkflow.CurrentUserId ?? default(int),
                NextStageCode = updatedContract.CurrentWorkflow.CurrentStage.Code,
            };

            NiisWorkflowAmbientContext.Current.ContractWorkflowService.Process(contractWorkFlowRequest);

            return Ok(updatedContractDetailDto);
        }

        [HttpPut("register/{id}")]
        public async Task<IActionResult> RegisterContract(int id, [FromBody] ContractDetailDto contractDetailDto)
        {
            contractDetailDto.Id = id;
            var updatedContractId = await Executor.GetCommand<RegisterContractCommand>().Process(c => c.ExecuteAsync(contractDetailDto));
            var updatedContract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(updatedContractId));
            var updatedContractDetailDto = Mapper.Map<Contract, ContractDetailDto>(updatedContract);
            return Ok(updatedContractDetailDto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Executor.GetCommand<DeleteContractCommand>().Process(c => c.Execute(id));
            return NoContent();
        }

        /// <summary>
        /// Генерация номера договора
        /// </summary>
        /// <param name="id">Идентефикатор договора</param>
        /// <returns>Рег. Номер и статус отправки</returns>
        [HttpGet("generateContractNum/{id}")]
        public async Task<IActionResult> GenerateContractNum(int id)
        {
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(id));
            await Executor.GetHandler<GenerateContractNumberHandler>().Process(c => c.ExecuteAsync(contract));

            ServerStatus status = null;

            if (contract.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeed)
            {
                status = SendContractNum(contract);

                if (status.Code == SendRegNumberStatusCodes.Successfully)
                {
                    contract.IsSyncContractNum = true;
                    Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
                }
            }

            return Ok(new
            {
                contractNum  = contract.ContractNum,
                status
            });
        }

        /// <summary>
        /// Отправка рег. номера контракта в ЛК
        /// </summary>
        /// <param name="id">Идентефикатор Контракта</param>
        /// <returns>Статус запроса</returns>
        [HttpGet("sendContractNum/{id}")]
        public async Task<IActionResult> SendContractNum(int id)
        {
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(id));

            var status = SendContractNum(contract);

            if (status.Code == SendRegNumberStatusCodes.Successfully)
            {
                contract.IsSyncContractNum = true;
                Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
            }

            return Ok(status);
        }

        /// <summary>
        /// Отправка рег. номера заявка в ЛК
        /// </summary>
        /// <param name="contract">Контракт</param>
        /// <returns>Статус запроса</returns>
        private ServerStatus SendContractNum(Contract contract)
        {
            var protectionDocTypeId = contract.ProtectionDocType.ExternalId ?? contract.ProtectionDocType.Id;

            var sendStatusBody = new SendRegNumberBody
            {
                Input = new SendRegNumber
                {
                    DocumentId = contract.Barcode,
                    PatentTypeId = protectionDocTypeId,
                    DocumentRegNumber = contract.ContractNum,
                    ApplicationDate = DateTime.Now.ToString("dd-MM-yyyy")
                }
            };

            var result = _lkIntergarionHelper.CallWebService(sendStatusBody, SoapActions.SendRegNumber);

            return result;
        }
    }
}