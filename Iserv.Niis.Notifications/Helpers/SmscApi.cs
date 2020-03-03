using System.IO;
using System.Net;
using System.Web;
using Iserv.Niis.Notifications.Models;
using Newtonsoft.Json;

namespace Iserv.Niis.Notifications.Helpers
{
    public class SmscApi
    {
        bool _smscPost; // использовать метод POST

        const string SmscCharset = "utf-8"; // кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8

        public string[][] D2Res;

        public SmsResponceModel SendSms(string phones, string message, NotificationsCredentials credentials, int translit = 1, string time = "", int id = 0,
            int format = 0, string sender = "", string query = "")
        {
            string[] formats = {"flash=1", "push=1", "hlr=1", "bin=1", "bin=2", "ping=1", "mms=1", "mail=1", "call=1"};

            // (id, cnt, cost, balance) или (id, -error)

            return SmscSendCmd("send", "phones=" + UrlEncode(phones)
                                                 + "&mes=" + UrlEncode(message) + "&id=" + id + "&translit=" + translit
                                                 + (format > 0 ? "&" + formats[format - 1] : "") +
                                                 (sender != "" ? "&sender=" + UrlEncode(sender) : "")
                                                 + (time != "" ? "&time=" + UrlEncode(time) : "") +
                                                 (query != "" ? "&" + query : ""),
                                                 credentials);
        }


        private SmsResponceModel SmscSendCmd(string cmd, string arg, NotificationsCredentials credentials)
        {
            string url, _url;

            arg = "login=" + UrlEncode(credentials.SmscLogin) + "&psw=" + UrlEncode(credentials.SmscPassword) + "&fmt=3&charset=" +
                  SmscCharset + "&" + arg;

            url = _url = $"http://smsc.kz/sys/{cmd}.php{(_smscPost ? "" : "?" + arg)}";

            string ret;
            int index = 0;
            HttpWebRequest request;
            StreamReader sr;
            HttpWebResponse response;

            do
            {
                if (index++ > 0)
                    url = _url.Replace("smsc.kz/", "www" + index + ".smsc.kz/");

                request = (HttpWebRequest) WebRequest.Create(url);

                try
                {
                    response = (HttpWebResponse) request.GetResponse();

                    sr = new StreamReader(response.GetResponseStream());
                    ret = sr.ReadToEnd();
                }
                catch (WebException)
                {
                    ret = "";
                }
            } while (ret == "" && index < 5);

            return JsonConvert.DeserializeObject<SmsResponceModel>(ret);
        }

        // кодирование параметра в http-запросе
        private string UrlEncode(string str)
        {
            if (_smscPost) return str;

            return HttpUtility.UrlEncode(str);
        }
    }
}