using System.IO;
using System.Web.Services;
using Autofac;
using Autofac.Integration.Wcf;
using IntegrationSoapReader;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Features;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;

namespace Iserv.Niis.ExternalServices.Host
{
    [WebService(Namespace = "http://SituationCenter.niis.kz", Name = "IntelSituationCenter",
        Description = "Интеграция НИИС с Ситуационным Центром")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebServiceSituationCenter : WebService, ISoapReaderExtension
    {
        private readonly IMediator _mediator;
        private readonly SoapHelper _soapHelper;

        public WebServiceSituationCenter()
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
        public GetBtBasePatentListResult GetBtBasePatentList(GetBtBasePatentListArgument arg)
        {
            var result = new GetBtBasePatentListResult();
            var query = new ProtectionDocList.Query
            {
                Result = result,
                Argument = arg
            };
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetDocumentListResult GetDocumentList(GetDocumentListArgument arg)
        {
            var result = new GetDocumentListResult();
            var query = new DocumentList.Query
            {
                Result = result,
                Argument = arg
            };
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetRfPatentListResult GetRfPatentList(GetRfPatentListArgument arg)
        {
            var result = new GetRfPatentListResult();
            var query = new RfProtectionDocList.Query
            {
                Result = result,
                Argument = arg
            };
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetReferenceResult GetReference(GetReferenceArgument arg)
        {
            var result = new GetReferenceResult();
            var query = new TypeInfoList.Query
            {
                Result = result,
                Argument = arg
            };
            _mediator.Send(query).Wait();
            return result;
        }
    }
}