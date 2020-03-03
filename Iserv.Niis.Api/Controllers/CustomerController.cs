using System;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Requests;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerUpdater _customerUpdater;
        private readonly IMapper _mapper;
        private readonly IExecutor _executor;

        public CustomerController(IMapper mapper, ICustomerUpdater customerUpdater, IExecutor executor)
        {
            _mapper = mapper;
            _customerUpdater = customerUpdater;
            _executor = executor;
        }

        [HttpGet("GetByXin/{xin}/{isPatentAttorney}")]
        public async Task<IActionResult> GetByXin(string xin, bool? isPatentAttorney)
        {
            var customer = await _customerUpdater.GetCustomer(xin, isPatentAttorney);
            var customerDto = _mapper.Map<DicCustomer, CustomerShortInfoDto>(customer);

            return Ok(customerDto);
        }
        [HttpGet("getAddresseeByOwnerId/{ownerType}/{id}")]
        public IActionResult GetAddresseeByOwnerId(Owner.Type ownerType, int id)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    {
                        var addresseeInfo = _executor.GetQuery<GetAddresseeByRequestIdQuery>().Process(q => q.Execute(id));
                        var addressee = addresseeInfo.addressee;
                        var subject = _mapper.Map<CustomerShortInfoDto>(addressee);
                        subject.OwnerAddresseeAddress = addresseeInfo.requestAddresseeAddress;
                        return Ok(subject);
                    }

                case Owner.Type.Contract:
                    {
                        var addresseeInfo = _executor.GetQuery<GetAddresseeByContractIdQuery>().Process(q => q.Execute(id));
                        var addressee = addresseeInfo.addressee;
                        var subject = _mapper.Map<CustomerShortInfoDto>(addressee);
                        subject.OwnerAddresseeAddress = addresseeInfo.contractAddresseeAddress;
                        return Ok(subject);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}