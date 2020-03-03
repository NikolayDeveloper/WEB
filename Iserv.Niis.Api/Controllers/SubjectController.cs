using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.ContractCustomers;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Requests;
using Iserv.Niis.BusinessLogic.ProtectionDocCustomers;
using Iserv.Niis.BusinessLogic.RequestCustomers;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Subject;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Exceptions;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/Subject")]
    public class SubjectController : BaseNiisApiController
    {
        private readonly IContractCategoryIdentifier _categoryIdentifier;
        private readonly IExecutor Executor;
        private readonly IMapper Mapper;
        private readonly ICustomerUpdater _customerUpdater;

        public SubjectController(
            IExecutor executor,
            IMapper mapper,
            IContractCategoryIdentifier categoryIdentifier,
            ICustomerUpdater customerUpdater)
        {
            Executor = executor;
            Mapper = mapper;
            _categoryIdentifier = categoryIdentifier;
            _customerUpdater = customerUpdater;
        }

        [HttpGet("{ownerId}/{ownerType}")]
        public async Task<IActionResult> GetByParent(int ownerId, Owner.Type ownerType)
        {
            List<SubjectDto> subjects;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestCustomers = await Executor.GetQuery<GetRequestCustomersByRequestIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    subjects = Mapper.Map<List<SubjectDto>>(requestCustomers);
                    break;
                case Owner.Type.Contract:
                    var contractCustomers = Executor.GetQuery<GetContractCustomersByContractIdQuery>()
                        .Process(q => q.Execute(ownerId));
                    subjects = Mapper.Map<List<SubjectDto>>(contractCustomers);
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDocCustomers = await Executor
                        .GetQuery<GetProtectionDocCustomersByProtectionDocIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    subjects = Mapper.Map<List<SubjectDto>>(protectionDocCustomers);
                    break;
                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }
            subjects = subjects.OrderBy(s => s.DisplayOrder).ToList();

            return Ok(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> GetByXinAndName()
        {
            var xin = Request.Query["xin"].ToString();
            var name = Request.Query["name"].ToString();
            var id = Request.Query["id"].ToString();
            var isPatentAttorney = Request.Query["isPatentAttorney"].ToString();
            var powerAttorneyFullNum = Request.Query["powerAttorneyFullNum"].ToString();
            var customerTypeId = Request.Query["customerTypeId"].ToString();

            var stringQuery = string.Join(";", id, xin, name, isPatentAttorney, powerAttorneyFullNum, customerTypeId);

            var dicCustomersRequest = GetDicCustomersRequest.ConstructFromQueryStringParameters(stringQuery);
            var customers = Executor.GetQuery<GetDicCustomersQuery>().Process(q => q.Execute(dicCustomersRequest));

            if (customers.Any() == false && dicCustomersRequest.HasXin)
            {
                var customer = await _customerUpdater.GetCustomer(dicCustomersRequest.Xin, dicCustomersRequest.IsPatentAttorney);
                if (customer != null)
                {
                    customers = Executor.GetQuery<GetDicCustomersQuery>().Process(q => q.Execute(dicCustomersRequest));
                }
            }
            var result  = customers.ProjectTo<SubjectDto>();
            var pagedList = result.ToPagedList(Request.GetPaginationParams());
            return pagedList.AsOkObjectResult(Response);
        }

        [HttpPost("{ownerType}")]
        public async Task<IActionResult> AttachCustomer([FromBody] SubjectDto subjectDto, Owner.Type ownerType)
        {
            var subject = await AttachCustomerPrivate(subjectDto, ownerType);
            return Ok(subject);
        }

        [HttpPost("create/{ownerType}")]
        public async Task<IActionResult> CreateCustomer([FromBody] SubjectDto subjectDto, Owner.Type ownerType)
        {
            var customer = Mapper.Map<SubjectDto, DicCustomer>(subjectDto);
            //var isCustomerExistsByXin = string.IsNullOrEmpty(customer.Xin) == false && Executor.GetQuery<IsCustomerAlreadyExistsByXinQuery>().Process(q => q.Execute(customer.Xin));
            //if (isCustomerExistsByXin)
            //{
            //    throw new ValidationException($"Customer with XIN {customer.Xin} already exists");
            //}

            await Executor.GetCommand<CreateDicCustomerCommand>().Process(c => c.ExecuteAsync(customer));

            var newSubject = Mapper.Map<SubjectDto>(customer);
            if (ownerType != Owner.Type.None)
            {
                newSubject.OwnerId = subjectDto.OwnerId;
                newSubject.Id = subjectDto.Id;
                newSubject.RoleId = subjectDto.RoleId;
                newSubject = await AttachCustomerPrivate(newSubject, ownerType);
            }

            return Ok(newSubject);
        }

        [HttpPut("{ownerType}/{id}")]
        public async Task<IActionResult> Put(int id, Owner.Type ownerType, [FromBody] SubjectDto subjectDto)
        {
            SubjectDto subject;
            var subjectId = subjectDto.Id = id;
            var customer = Mapper.Map<SubjectDto, DicCustomer>(subjectDto);
            _customerUpdater.Update(customer);

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestCustomer = await Executor.GetQuery<GetRequestCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(subjectId));

                    if (requestCustomer == null)
                    {
                        throw new DataNotFoundException(nameof(RequestCustomer),
                            DataNotFoundException.OperationType.Update, id);
                    }

                    Mapper.Map(subjectDto, requestCustomer);
                    requestCustomer.Customer = null;
                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                        .Process(c => c.ExecuteAsync(requestCustomer));

                    if (requestCustomer.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)
                    {
                        var correspondence = Executor.GetQuery<GetDicCustomerByIdQuery>()
                            .Process(q => q.Execute(requestCustomer.CustomerId ?? 0));
                        correspondence.Address = requestCustomer.Address;
                        correspondence.AddressEn = requestCustomer.AddressEn;
                        correspondence.AddressKz = requestCustomer.AddressKz;
                        await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(correspondence));

                        //var request = await Executor.GetQuery<GetRequestByIdQuery>()
                        //    .Process(q => q.ExecuteAsync(requestCustomer.RequestId ?? 0));
                        //request.AddresseeId = correspondence.Id;
                        //await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                    }

                    var requestCustomerWithIncludes = await Executor.GetQuery<GetRequestCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(requestCustomer.Id));
                    subject = Mapper.Map<RequestCustomer, SubjectDto>(requestCustomerWithIncludes);
                    break;
                case Owner.Type.Contract:
                    var contractCustomer = await Executor.GetQuery<GetContractCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(subjectId));
                    if (contractCustomer == null)
                    {
                        throw new DataNotFoundException(nameof(ContractCustomer),
                            DataNotFoundException.OperationType.Update, id);
                    }

                    Mapper.Map(subjectDto, contractCustomer);
                    contractCustomer.Customer = null;
                    await Executor.GetCommand<UpdateContractCustomerCommand>()
                        .Process(c => c.ExecuteAsync(contractCustomer));
                    await _categoryIdentifier.IdentifyAsync(contractCustomer.ContractId);

                    var contractCustomerWithIncludes = await Executor.GetQuery<GetContractCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(contractCustomer.Id));
                    subject = Mapper.Map<ContractCustomer, SubjectDto>(contractCustomerWithIncludes);
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDocCustomer = await Executor.GetQuery<GetProtectionDocCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(subjectId));
                    if (protectionDocCustomer == null)
                    {
                        throw new DataNotFoundException(nameof(ProtectionDocCustomer),
                            DataNotFoundException.OperationType.Update, id);
                    }

                    Mapper.Map(subjectDto, protectionDocCustomer);
                    protectionDocCustomer.Customer = null;
                    await Executor.GetCommand<UpdateProtectionDocCustomerCommand>()
                        .Process(c => c.ExecuteAsync(protectionDocCustomer));

                    var protectionDocCustomerWithIncludes = await Executor.GetQuery<GetProtectionDocCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(protectionDocCustomer.Id));
                    subject = Mapper.Map<ProtectionDocCustomer, SubjectDto>(protectionDocCustomerWithIncludes);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Ok(subject);
        }

        [HttpDelete("{ownerType}/{id}")]
        public async Task<IActionResult> Delete(Owner.Type ownerType, int id)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<DeleteRequestCustomerCommand>().Process(c => c.ExecuteAsync(id));
                    break;
                case Owner.Type.Contract:
                    var contractCustomer = await Executor.GetQuery<GetContractCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(id));
                    await Executor.GetCommand<DeleteContractCustomerCommand>().Process(c => c.ExecuteAsync(id));
                    await _categoryIdentifier.IdentifyAsync(contractCustomer.ContractId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return NoContent();
        }

        [HttpPost("several/{ownerType}")]
        public async Task<IActionResult> PostSeveral(Owner.Type ownerType, [FromBody] SubjectDto[] subjectDtos)
        {
            foreach (var subjectDto in subjectDtos)
            {
                await AttachCustomerPrivate(subjectDto, ownerType);
            }
            return NoContent();
        }

        [HttpPut("several/{ownerType}")]
        public async Task<IActionResult> PutSeveral(Owner.Type ownerType, [FromBody] SubjectDto[] subjectDtos)
        {
            foreach (var subjectDto in subjectDtos)
            {
                await Put(subjectDto.Id, ownerType, subjectDto);
            }
            return NoContent();
        }

        [HttpPost("deleteseveral/{ownerType}")]
        public async Task<IActionResult> DeleteSeveral(Owner.Type ownerType, [FromBody] int[] ids)
        {
            foreach (var id in ids)
            {
                await Delete(ownerType, id);
            }
            return NoContent();
        }

        #region PrivateMethods

        private async Task<SubjectDto> AttachCustomerPrivate(SubjectDto subjectDto, Owner.Type ownerType)
        {
            SubjectDto subject;
            var customer = Mapper.Map<SubjectDto, DicCustomer>(subjectDto);
            if (customer.Id == 0)
            {
                throw new ValidationException("Unable to add power attorney!!!");
            }
            _customerUpdater.Update(customer);

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var newRequestCustomer = Mapper.Map<SubjectDto, RequestCustomer>(subjectDto);
                    var requestCusomerId = await Executor.GetCommand<CreateRequestCustomerCommand>()
                        .Process(c => c.ExecuteAsync(newRequestCustomer));
                    var requestCustomer = await Executor.GetQuery<GetRequestCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(requestCusomerId));
                    subject = Mapper.Map<RequestCustomer, SubjectDto>(requestCustomer);
                    await Executor.GetHandler<UpdateRequestBeneficiaryTypeHandler>().Process(h => h.HandleAsync(subject));
                    var request = await Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(requestCustomer.RequestId ?? 0));
                    var protectionDocTypeCode = request?.ProtectionDocType?.Code;
                    if (protectionDocTypeCode == DicProtectionDocTypeCodes.RequestTypeTrademarkCode)
                    {
                        string subtypeCode = "";
                        if (request.SpeciesTradeMark?.Code == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                        {
                            subtypeCode = DicProtectionDocSubtypeCodes.NationalTradeMark;
                        }
                        if (requestCustomer.CustomerRole?.Code == DicCustomerRoleCodes.Declarant)
                        {
                            if (requestCustomer.Customer?.Country?.Code == "KZ")
                            {
                                subtypeCode = DicProtectionDocSubtypeCodes.NationalTradeMark;
                            }
                            else
                            {
                                subtypeCode = DicProtectionDocSubtypeCodes.InternationalTrademark;
                            }
                        }
                        var requestSubtype = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                            .Process(q => q.Execute(subtypeCode));
                        request.RequestType = requestSubtype ?? request.RequestType;
                        request.RequestTypeId = requestSubtype?.Id ?? request.RequestTypeId;
                        await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                    }
                    break;
                case Owner.Type.Contract:
                    var newContractCustomer = Mapper.Map<SubjectDto, ContractCustomer>(subjectDto);
                    var contractCustomerId = await Executor.GetCommand<CreateContractCustomerCommand>()
                        .Process(c => c.ExecuteAsync(newContractCustomer));
                    await _categoryIdentifier.IdentifyAsync(newContractCustomer.ContractId);

                    var contractCustomer = await Executor.GetQuery<GetContractCustomerByIdQuery>()
                        .Process(q => q.ExecuteAsync(contractCustomerId));
                    subject = Mapper.Map<ContractCustomer, SubjectDto>(contractCustomer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return subject;
        }

        #endregion
    }
}