using System;
using System.ComponentModel;
using System.IO;
using System.Web.Services;
using Autofac;
using Autofac.Integration.Wcf;
using IntegrationSoapReader;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Feature;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;

namespace Iserv.Niis.ExternalServices.Host
{
    /// <summary>
    /// Summary description for IntegrationContract
    /// </summary>
    [WebService(Namespace = "http://niis-integration-contract-service4efiling.kz/", Description = "")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class IntegrationContract : System.Web.Services.WebService, ISoapReaderExtension
    {
        private readonly SoapHelper _soapHelper;
        private readonly IMediator _mediator;

        public IntegrationContract()
        {
            _soapHelper = AutofacHostFactory.Container.Resolve<SoapHelper>();
            _mediator = AutofacHostFactory.Container.Resolve<IMediator>();
        }


        public void LogRequest(MemoryStream memoryStream)
        {
            _soapHelper.LogSoap(memoryStream);
        }

        public void LogResponse(MemoryStream memoryStream)
        {
            _soapHelper.LogSoap(memoryStream);
        }

        [WebMethod]
        public ContractResponse ContractApplicationSend(ContractRequest request)
        {
            var result = new ContractResponse();
            var query = new ContractApplicationSend.Query { Request = request, Responce = result };
            _mediator.Send(query).Wait();
            return result;
        }

        /// <summary>
        /// Метод получения данных о патенте 
        /// </summary>
        /// <param name="request">Входные параметры: Номер патента, тип патента</param>
        /// <returns></returns>
        [WebMethod]
        public CustomerPatentValidityResponce GetCustomerPatentValidityInfo(CustomerPatentValidityRequest request)
        {
            var result = new CustomerPatentValidityResponce();
            var query = new GetCustomerPatentValidity.Query { Request = request, Responce = result };
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public ContractApplicationTypeResponse GetAllContractApplicationTypes()
        {
            return null;
        }



        [WebMethod]
        public IntellectualPropertyObjectTypeResponse GetAllIPObjectTypes()
        {
            return null;
        }

        [WebMethod]
        public CustomerRoleResponse GetAllCustomerRoles()
        {
            return null;
        }

        [WebMethod]
        public CustomerTypeResponse GetAllCustomerTypes()
        {
            return null;
        }


    }
}
