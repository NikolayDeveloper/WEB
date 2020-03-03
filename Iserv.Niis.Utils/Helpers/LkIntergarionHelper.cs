using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Iserv.Niis.Domain.Intergrations;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Utils.Helpers
{
    /// <summary>
    /// Сервис отправки запросов в ЛК
    /// </summary>
    public class LkIntergarionHelper : ILkIntergarionHelper
    {
        #region Constructor

        private readonly string _lkUrl;
        private readonly string _lkSen;

        public LkIntergarionHelper(IConfiguration configuration)
        {
            _lkUrl = configuration.GetSection("IntegrationLk:ServerUrl")?.Value;
            _lkSen = configuration.GetSection("IntegrationLk:ServerSen")?.Value;
        }

        #endregion

        /// <summary>
        /// Отправка запроса в ЛК
        /// </summary>
        /// <typeparam name="T">Тип обхекта для сериализации</typeparam>
        /// <param name="body">Объект с параметрами для запроса</param>
        /// <param name="action">Тип запроса</param>
        /// <param name="senCode">Код Экшина</param>
        /// <returns>Статус запроса</returns>
        public ServerStatus CallWebService<T>(T body, string action, string senCode = "sen")
        {
            var sen = $"{_lkSen}{action}";

            var soapEnvelopeXml = CreateSoapEnvelope(sen, body, senCode);
            var webRequest = CreateWebRequest(action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            
            var asyncResult = webRequest.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            
            using (var webResponse = webRequest.EndGetResponse(asyncResult))
            {
                string soapResult;
                using (var rd = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    soapResult = rd.ReadToEnd();
                }

                return DeserializeResult(soapResult);
            }
        }

        #region Private Method

        /// <summary>
        /// Десериализация ответа сервера ЛК
        /// </summary>
        /// <param name="soapResult">Тело XML ответа</param>
        /// <returns>Статус запроса</returns>
        private ServerStatus DeserializeResult(string soapResult)
        {
            var xsSubmit = new XmlSerializer(typeof(EnvelopeResponce));

            using (TextReader reader = new StringReader(soapResult))
            {
                var settings = new XmlReaderSettings();
                using (var xmlReader = XmlReader.Create(reader, settings))
                {
                    var result = (EnvelopeResponce)xsSubmit.Deserialize(xmlReader);

                    return result.Body?.ServerStatus;
                }
            }
        }

        /// <summary>
        /// Создание запроса
        /// </summary>
        /// <param name="action">Тип запроса</param>
        /// <returns>Запрос</returns>
        private HttpWebRequest CreateWebRequest(string action)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(_lkUrl);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        /// <summary>
        /// Создание и отправка запроса
        /// </summary>
        /// <typeparam name="T">Тип контента запроса</typeparam>
        /// <param name="sen">Экшн +Урл SOAP запроса</param>
        /// <param name="body">Тело запроса</param>
        /// <param name="senCode">Код Экшина</param>
        /// <returns>XML ответ сервера</returns>
        private XmlDocument CreateSoapEnvelope<T>(string sen, T body, string senCode)
        {
            var soapEnvelopeDocument = new XmlDocument();

            var envelope = new Envelope
            {
                Header = new object(),
                Body = body
            };
            
            var xsSubmit = new XmlSerializer(typeof(Envelope), new[] { typeof(T) });

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                    ns.Add(senCode, sen);

                    xsSubmit.Serialize(writer, envelope, ns);
                    xml = sww.ToString();
                }
            }

            soapEnvelopeDocument.LoadXml(xml);

            return soapEnvelopeDocument;
        }

        /// <summary>
        /// Заполнение SOAP запроса
        /// </summary>
        /// <param name="soapEnvelopeXml">SOAP запрос</param>
        /// <param name="webRequest">Web запрос</param>
        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (var stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        #endregion
    }
}