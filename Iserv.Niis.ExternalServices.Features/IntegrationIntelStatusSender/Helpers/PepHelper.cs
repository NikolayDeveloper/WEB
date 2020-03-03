using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using AutoMapper;
using IntegrationSoapReader;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.ExternalPEP;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Helpers
{
    public class PepHelper
    {
        private const string ProjectType = "StatusSendPEP";
        private const string MessageSend = "MessageSend";

        private readonly Configuration _configuration;
        private readonly LoggingHelper _logging;
        private readonly FileLoggerSettings _settings;

        public PepHelper(LoggingHelper logging, Configuration configuration)
        {
            _configuration = configuration;
            _logging = logging;
            _settings = new FileLoggerSettings {FolderBase = _configuration.LogXmlDir, FolderDepth = DatePart.Hour};
        }

        public void SendStatus(IntegrationStatus status, IntegrationRequisition requisition)
        {
            var requestInfo = new RequestInfo
            {
                chainId = requisition.ChainId,
                Sender = CommonConstants.SystemInfoSenderNiis,
                messageId = Guid.NewGuid().ToString(),
                messageDate = DateTime.Now
            };
            var logAction = new LogAction
            {
                DbDateTime = DateTimeOffset.Now,
                Project = CommonConstants.StatusSender,
                Type = ProjectType,
                Note = $"RequestBarcode = {status.RequestBarcode} StatusId = {status.OnlineRequisitionStatusId}",
                SystemInfoQueryId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(requestInfo))
            };
            _logging.CreateLogAction(logAction);
            var responseInfo = new RequestInfo {status = new StatusInfo()};
            string url = null;

            try
            {
                var dopUsluga = requisition.Callback != null && requisition.Callback.Contains(MessageSend);
                var response = SendStatusToPep(status, requisition.ProtectionDocTypeId, requestInfo, dopUsluga, out url);

                var xml = new XmlDocument();
                xml.LoadXml(response);
                var responseNode = xml.GetElementsByTagName("response")[0];

                responseInfo.status.Code = responseNode.ChildNodes[1].ChildNodes[0].InnerText;
                responseInfo.status.Message = responseNode.ChildNodes[1].ChildNodes[1].InnerText;
            }
            catch (WebException ex)
            {
                responseInfo.status.Code = "-99";
                responseInfo.status.Message = ex.Message + " // " + GetResponseText(ex.Response);
            }
            catch (Exception ex)
            {
                responseInfo.status.Code = "-88";
                responseInfo.status.Message = ex.ToString();
            }
            logAction.SystemInfoAnswerId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(requestInfo));
            _logging.UpdateLogAction(logAction);
            if (!int.TryParse(responseInfo.status.Code, out var code) || code != 3)
            {
                var text = $"code =  {responseInfo.status.Code} ; PEP => URL: {url} => {responseInfo.status.Message}";
                throw new Exception(text);
            }
        }

        #region PrivateMethods

        private string SendStatusToPep(IntegrationStatus status, int patentTypeId,
            RequestInfo requestInfo, bool dopUsluga, out string url)
        {
            var requestXml = GetRequestXml(status, patentTypeId, requestInfo, dopUsluga);
            LogPep(requestXml, nameof(SendStatusToPep));
            url = _configuration.PepUrl;
            url = string.Format(url, GetPatentCodeByTypeUid(patentTypeId, dopUsluga));
            var request = WebRequest.Create(url);

            var bytesRequest = Encoding.UTF8.GetBytes(requestXml);

            request.Method = "POST";
            request.Timeout = 3 * 60 * 1000; //3 минуты
            request.ContentType = "text/xml";
            request.ContentLength = bytesRequest.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytesRequest, 0, bytesRequest.Length);
            }

            string responseFromServer;

            using (var response = request.GetResponse())
            {
                using (var answerStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(answerStream ??
                                                  throw new InvalidOperationException(
                                                      $"Не удалось получить ответ от {url}"));
                    responseFromServer = reader.ReadToEnd();
                    LogPep(responseFromServer, "SendStatusToPepResponse");

                    var httpStatus = (int) ((HttpWebResponse) response).StatusCode;

                    if (httpStatus != 200)
                        throw new Exception("HTTP Status : " + httpStatus + " :" + responseFromServer);
                }
            }
            return responseFromServer;
        }

        private void LogPep(string xml, string method)
        {
            FileLogger.LogSOAP(xml, _settings, method);
        }

        private string GetResponseText(WebResponse webResponse)
        {
            if (webResponse == null)
                return null;

            using (var stream = webResponse.GetResponseStream())
            {
                using (var reader =
                    new StreamReader(
                        stream ?? throw new InvalidOperationException(
                            $"Не удалось извлечь информацию с {webResponse.ResponseUri}")))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string GetPatentCodeByTypeUid(int uid, bool dopUsluga) // TODO переделать
        {
            string result;

            switch (uid)
            {
                case 1: //1	Патент на изобретение
                    result = "403";
                    break;
                case 2: //2	Патент на Полезную Модель
                    result = "404";
                    break;
                case 3: //3	Патент на Промышленный Образец
                    result = "405";
                    break;
                case 4: //4	Товарные Знаки
                    result = "408";
                    break;
                case 5: //5	Наименование Мест Происхождения Товаров
                    result = "407";
                    break;
                case 6: //6	Патент на селекционные достижения
                    result = "406";
                    break;
                case 9: //9	Инновационный патент
                    result = "402";
                    break;
                default:
                    throw new Exception("Неизвестный uid типа патента: " + uid);
            }
            if (dopUsluga)
                result += "A";

            return result;
        }

        private string GetRequestXml(IntegrationStatus status, int patentTypeId,
            RequestInfo requestInfo, bool dopUsluga)
        {
            var patentCode = GetPatentCodeByTypeUid(patentTypeId, dopUsluga);
            return $@"
            <?xml version=""1.0"" encoding=""utf-16""?>
                <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http:www.w3.org/2001XMLSchema"">
                  <soap:Body>
                    <sendResponse xmlns=""http://MU.P{patentCode}.Library/MU/P{patentCode}"">
                      <request xmlns="">
                        <responseData>
                          <documentId>{status.RequestBarcode}</documentId>
                          <status>{status.OnlineRequisitionStatusId}</status>
                          <additionalInfo>{status.AdditionalInfo}</additionalInfo>
                        </responseData>
                        <systemInfo>
                          <requestNumber />
                          <messageId>{requestInfo.messageId}</messageId>
                          <chainId>{requestInfo.chainId}</chainId>
                          <messageDate>{DateTime.Now:o}</messageDate>
                          <digiSign />
                          <status />
                          <option />
                          <Sender>NIIS</Sender>
                          <RequestIINBIN />
                          <RequestedIINBIN />
                          <ClientIPAddress />
                        </systemInfo>
                      </request>
                    </sendResponse>
                  </soap:Body>
                </soap:Envelope>";
        }

        #endregion
    }
}