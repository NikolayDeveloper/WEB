using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using Autofac;
using Autofac.Integration.Wcf;
using IntegrationSoapReader;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Utils;
using MediatR;

namespace Iserv.Niis.ExternalServices.Host
{
    [WebService(Namespace = "http://egov.niis.kz", Name = "IntelEGov", Description = "Интеграция НИИС с EGov")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebServiceIntelEGov : WebService, ISoapReaderExtension
    {
        private readonly SoapHelper _soapHelper;
        private readonly IMediator _mediator;

        public WebServiceIntelEGov()
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

        /// <summary>
        /// Прием заявки
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        [WebMethod]
        public RequisitionSendResult RequisitionSend(RequisitionSendArgument argument)
        {
            var result = new RequisitionSendResult();
            var command =
                new RequisitionSend.Command { Argument = argument, Result = result };
            _mediator.Send(command).Wait();
            return result;
        }

        /// <summary>
        /// Прием документа переписки
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        [WebMethod]
        public MessageSendResult MessageSend(MessageSendArgument argument)
        {
            var result = new MessageSendResult();
            var command = new MessageSend.Command{Argument = argument, Result = result};
            _mediator.Send(command).Wait();
            return result;
        }

        [WebMethod]
        public GetPaySumResult GetPaySum(GetPaySumArgument argument)
        {
            var result = new GetPaySumResult();
            var query = new GetPaySum.Query { Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public CheckPatentStatementResult CheckPatentStatement(CheckPatentStatementArgument argument)
        {
            var result = new CheckPatentStatementResult();
            var query = new CheckPatentStatement.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetCountTextForPaySumResult GetCountTextForPaySum(GetCountTextForPaySumArgument argument)
        {
            var result = new GetCountTextForPaySumResult();
            var query = new GetCountTextForPaySum.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetPayTarifResult GetPayTarif(GetPayTarifArgument argument)
        {
            var result = new GetPayTarifResult();
            var query = new GetPayTariff.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetAttorneyInfoResult GetAttorneyInfo(GetAttorneyInfoArgument argument)
        {
            var result = new GetAttorneyInfoResult();
            var query = new GetAttorneyInfo.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetRequisitionListByMessageTypeResult GetRequisitionListByMessageType(
            GetRequisitionListByMessageTypeArgument argument)
        {
            var result = new GetRequisitionListByMessageTypeResult();
            var query = new GetRequisitionListByMessageType.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetRequisitionListForPaymentResult GetRequisitionListForPayment(
            GetRequisitionListForPaymentArgument argument)
        {
            var result = new GetRequisitionListForPaymentResult();
            var query = new GetRequisitionListForPayment.Query{Argument = argument,Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetRequisitionInfoResult GetRequisitionInfo(GetRequisitionInfoArgument argument)
        {
            var result = new GetRequisitionInfoResult();
            var query = new GetRequisitionInfo.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }


        [WebMethod]
        public GetMessageFileResult GetMessageFile(GetMessageFileArgument argument)
        {
            var result = new GetMessageFileResult();
            var query = new GetMessageFile.Query{Argument = argument, Result = result};
            _mediator.Send(query).Wait();
            return result;
        }

        [WebMethod]
        public GetAttachedFileMetadataResult GetAttachedFileMetadata(GetAttachedFileMetadataArgument argument)
        {
            var result = new GetAttachedFileMetadataResult();
            var query = new GetAttachedFileMetadata.Query{Result = result, Argument = argument};
            _mediator.Send(query).Wait();
            return result;
        }

        /// <summary>
        /// Получение справочника тарифов
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<Tariff> GetAllTariffs()
        {
            var query = new GetAllTariffs.Query();
            var result = _mediator.Send(query).Result;
            return result;
        }

        /// <summary>
        /// Получение справочника цветов
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<Color> GetAllColors()
        {
            var query = new GetAllColors.Query();
            var result = _mediator.Send(query).Result;
            return result;
        }

        [WebMethod]
        public List<PatentAttorney> GetAllPatentAttorneys()
        {
            var query = new GetAllPatentAttorneys.Query();
            var result = _mediator.Send(query).Result;
            return result;
        }

        /// <summary>
        /// Получение справочника типов документов
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<DocumentInfo> GetAllDocuments()
        {
            var query = new GetAllDocuments.Query();
            var result = _mediator.Send(query).Result;
            return result;
        }

        /// <summary>
        /// Метод загрузки зарегистрированных патентов по ИИНу заявителя
        /// </summary>
        /// <param name="xin">ИИН</param>
        /// <returns>Патенты</returns>
        [WebMethod]
        public List<CustomerPatentInfo> GetPatentsByCustomerXin(string xin)
        {
            var result = new List<CustomerPatentInfo>();
            var query = new GetCustomerPatentInfo.Query { Argument = xin, Result = result };
            _mediator.Send(query).Wait();
            return query.Result;
        }
    }
}