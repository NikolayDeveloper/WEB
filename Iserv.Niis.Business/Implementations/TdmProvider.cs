using Iserv.Niis.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Models;

namespace Iserv.Niis.Business.Implementations
{
	public class TdmProvider: ITdmProvider
	{
		private static readonly Encoding encoding = Encoding.UTF8;

		private string PostImageUrl { get; set; }
		private string PostPhoneticUrl { get; set; }
		private string PostSemanticUrl { get; set; }
		private string ContentTypeMultipartPattern { get; set; }
		private string ContentType { get; set; }
		private string UserAgent { get; set; }

		public TdmProvider(string postImageUrl, string postPhoneticUrl, string postSemanticUrl, string contentTypeMultipartPattern, string contentType, string userAgent)
		{
			PostImageUrl = postImageUrl;
			PostPhoneticUrl = postPhoneticUrl;
			PostSemanticUrl = postSemanticUrl;
			ContentTypeMultipartPattern = contentTypeMultipartPattern;
			ContentType = contentType;
			UserAgent = userAgent;
		}

		public string GetResultsSearchByImage(byte[] image)
		{
			FileParameter fileParameter = new FileParameter(image, "1.png", ContentTypeMultipartPattern);
			Dictionary<string, object> postParameters = new Dictionary<string, object>();
			postParameters.Add(ContentType, fileParameter);
			string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
			byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

			string result = PostImage(PostImageUrl, UserAgent, String.Format(ContentTypeMultipartPattern + "; boundary = " + formDataBoundary), formData);
			return result;
		}

		public string GetResultsSearchByPhonetic(string value)
		{
			return PostPhonetic(PostPhoneticUrl, value);
		}

		public string GetResultsSearchBySemantic(string value)
		{
			return PostSemantic(PostSemanticUrl, value);
		}

		private static string PostImage(string postUrl, string userAgent, string contentType, byte[] formData)
		{
			HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

			if (request == null)
			{
				throw new NullReferenceException("request is not a http request");
			}

			request.Method = "POST";
			request.ContentType = contentType;
			request.UserAgent = userAgent;
			request.CookieContainer = new CookieContainer();
			request.ContentLength = formData.Length;

			// You could add authentication here as well if needed:
			// request.PreAuthenticate = true;
			// request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
			// request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

			// Send the form data to the request.
			using (Stream requestStream = request.GetRequestStream())
			{
				requestStream.Write(formData, 0, formData.Length);
			}

			string res = "";
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			if (response != null)
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
					{
						res = readStream.ReadToEnd();
					}
				}
			}
			return res;
		}

		private static string PostPhonetic(string postUrl, string value)
		{
			WebClient myWebClient = new WebClient();
			NameValueCollection myQueryStringCollection = new NameValueCollection();		
				myQueryStringCollection.Add("queryName", value);
			myWebClient.QueryString = myQueryStringCollection;
			var response=myWebClient.DownloadString(postUrl);
			return response;
		}
		private static string PostSemantic(string postUrl, string value)
		{
			WebClient myWebClient = new WebClient();
			NameValueCollection myQueryStringCollection = new NameValueCollection();

			myQueryStringCollection.Add("queryName", value);
			myWebClient.QueryString = myQueryStringCollection;
			var response = myWebClient.DownloadString(postUrl);
			return response;
		}
		private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
		{
			Stream formDataStream = new System.IO.MemoryStream();
			bool needsCLRF = false;

			foreach (var param in postParameters)
			{
				if (needsCLRF)
					formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

				needsCLRF = true;

				if (param.Value is FileParameter)
				{
					FileParameter fileToUpload = (FileParameter)param.Value;

					string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
						 boundary,
						 "file",
						 fileToUpload.FileName ?? param.Key,
						 fileToUpload.ContentType ?? "application/octet-stream");

					formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

					formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
				}
				else
				{
					string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
						 boundary,
						 "file",
						 param.Value);
					formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
				}
			}

			string footer = "\r\n--" + boundary + "--\r\n";
			formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

			formDataStream.Position = 0;
			byte[] formData = new byte[formDataStream.Length];
			formDataStream.Read(formData, 0, formData.Length);
			formDataStream.Close();

			return formData;
		}
	}
}
