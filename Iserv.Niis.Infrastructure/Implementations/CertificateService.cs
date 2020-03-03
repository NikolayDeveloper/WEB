using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CertCheckerClient;
using CertCheckerClient.External;
using Iserv.Niis.Infrastructure.Abstract;
using KalkanCryptCOMLib;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Iserv.Niis.Infrastructure.Implementations
{
    public class CertificateService : ICertificateService, IDisposable
    {
        /// <summary>
        ///     The gost alg identifier
        /// </summary>
        public const string GostAlgId = "1.3.6.1.4.1.6801.1.2.2";

        public const string GostAlgV2Id = "1.2.398.3.10.1.1.1.2";

        /// <summary>
        ///     The Rsa alg identifier
        /// </summary>
        public const string RsaAlgId = "1.2.840.113549.1.1.11";

        /// <summary>
        ///     The RSA alg type
        /// </summary>
        public const string RsaAlgType = "rsa";

        /// <summary>
        ///     The gost alg type
        /// </summary>
        public const string GostAlgType = "gost";

        private readonly CertCheckerUniversal _certCheckerClient;
        private readonly IConfiguration _configuration;
        private readonly KalkanCryptCOM _kalkanCryptCom;

        public CertificateService(IConfiguration configuration)
        {
            _configuration = configuration;
            _certCheckerClient = CreateCertCheckerUniversal();
            _kalkanCryptCom = new KalkanCryptCOM();
            _kalkanCryptCom.Init();
        }

        public CertificateData GetCertificateData(string certData, string algType)
        {
            var certificate = GetX509Certificate2(certData);

            if (IsGostAlgId(certificate.SignatureAlgorithm.Value)
                && !GostAlgType.Equals(algType))
            {
                throw new Exception("Неподходящий тип сертификата, ожидался " + algType);
            }

            if (IsRsaAlgId(certificate.SignatureAlgorithm.Value)
                && !RsaAlgType.Equals(algType))
            {
                throw new Exception("Неподходящий тип сертификата, ожидался " + algType);
            }


            return GetCertificateData();

            CertificateData GetCertificateData()
            {
                var dnAttrs = GetDnAttrs(certificate.Subject).ToArray();
                var iinAttr = GetDnAttribute("SERIALNUMBER", dnAttrs);
                if (string.IsNullOrEmpty(iinAttr) || !iinAttr.StartsWith("IIN"))
                {
                    throw new Exception("Не удалось обнаружить ИИН пользователя");
                }

                var binAttr = GetDnAttribute("OU", dnAttrs);
                var fullName = GetDnAttribute("CN", dnAttrs).Split(' ');
                var certificateData = new CertificateData
                {
                    Iin = iinAttr.Substring(3),
                    Bin = binAttr?.Substring(3) ?? string.Empty,
                    CertOwnerCorpName = GetDnAttribute("O", dnAttrs),
                    LastName = fullName[0],
                    FirstName = fullName.Length == 2 ? fullName[1] : string.Empty,
                    MiddleName = GetDnAttribute("G", dnAttrs) ?? string.Empty,
                    Certificate = certificate
                };

                return certificateData;
            }
        }

        public bool VerifyRsaCertificate(X509Certificate2 certificate, string plainData, string signPlainData)
        {
            var rsa = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            var hashFunction = certificate.SignatureAlgorithm.Value == RsaAlgId
                ? (object)SHA256.Create()
                : SHA1.Create();
            var isValidData = rsa.VerifyData(Encoding.ASCII.GetBytes(plainData), hashFunction,
                Convert.FromBase64String(signPlainData));
            //var isValidCert = CheckCert(certificate, true);
            //return isValidCert && isValidData;
            return isValidData;
        }

        public bool VerifyGostCertificate(X509Certificate2 certificate, string signedData)
        {
            var errorText = "";
            try
            {
                _kalkanCryptCom.VerifyXML(" ", 0, signedData, out _);
                _kalkanCryptCom.GetLastErrorString(out errorText, out var errorCode);
                if (errorCode > 0)
                {
                    Log.Error(errorText);
                    return false;
                }
                //return CheckCert(certificate, false);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, errorText);
                return false;
            }
        }

        private X509Certificate2 GetX509Certificate2(string certData)
        {
            if (string.IsNullOrEmpty(certData))
            {
                throw new Exception("Не удалось обнаружить сертификат");
            }

            return new X509Certificate2(Convert.FromBase64String(certData));
        }

        private bool CheckCert(X509Certificate2 certificate, bool disableCrlValidation)
        {
            var errorText = "";
            try
            {
                var result = _certCheckerClient.Request(certificate.RawData, disableCrlValidation);
                errorText = result.ErrorText;
                return result.Error == CertCheckError.NoError;
            }
            catch (RevocationServerNotAvailableException)
            {
                Log.Error("Невозможно проверить функцию отзыва ЭЦП, т.к. сервер отзыва сертификатов НУЦ РК недоступен.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, errorText);
                return false;
            }
        }

        private CertCheckerUniversal CreateCertCheckerUniversal()
        {
            var certConfig = _configuration.GetSection("CertChecker");

            var certCheckerClientId = certConfig.GetSection("CertCheckerClientId").Value;
            var certCheckerUrls = certConfig
                .GetSection("CertCheckerUrls")
                .Value?
                .Split('|')
                .Select(s => s.Trim())
                .ToArray();

            return new CertCheckerUniversal(certCheckerClientId)
            {
                UrlArray = certCheckerUrls
            };
        }

        #region Helpers

        private static bool IsGostAlgId(string algId)
        {
            return GostAlgId.Equals(algId) || GostAlgV2Id.Equals(algId);
        }

        private static bool IsRsaAlgId(string algId)
        {
            return RsaAlgId.Equals(algId);
        }

        private static string GetDnAttribute(string key, IEnumerable<string> dnAttrs)
        {
            key = key + "=";
            foreach (var parsedByDelemiterDnAttr in dnAttrs)
            {
                var attr = parsedByDelemiterDnAttr.Trim(' ', '\r', '\n');
                if (attr.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    return attr.Substring(key.Length);
                }
            }

            return null;
        }

        private static IEnumerable<string> GetDnAttrs(string dn)
        {
            var csvLines = new List<CsvLine>();
            using (var memStream = new MemoryStream(Encoding.Unicode.GetBytes(dn)))
            using (var sr = new StreamReader(memStream, Encoding.Unicode))
            {
                var csvParser = new CsvParser(sr, ',');
                CsvLine csvLine;
                while ((csvLine = csvParser.GetNextLine()) != null)
                {
                    if (csvLine.Items.Length == 0)
                    {
                        break;
                    }

                    csvLines.Add(csvLine);
                }
            }

            return csvLines[0].Items;
        }

        #endregion

        #region NestedClasses

        /// <summary>
        ///     The CSV line class
        /// </summary>
        public class CsvLine
        {
            /// <summary>
            ///     The _items
            /// </summary>
            private readonly List<string> _items = new List<string>();

            /// <summary>
            ///     Gets the items.
            /// </summary>
            /// <value>
            ///     The items.
            /// </value>
            public string[] Items => _items.ToArray();


            /// <summary>
            ///     Adds the specified item.
            /// </summary>
            /// <param name="item">The item.</param>
            public void Add(string item)
            {
                _items.Add(item);
            }
        }

        /// <summary>
        ///     The CSV parser class
        /// </summary>
        public class CsvParser
        {
            /// <summary>
            ///     The _delemiter
            /// </summary>
            private readonly char _delemiter;

            /// <summary>
            ///     The _text
            /// </summary>
            private readonly StreamReader _text;

            /// <summary>
            ///     Initializes a new instance of the <see cref="CsvParser" /> class.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="delemiter">The delemiter.</param>
            public CsvParser(StreamReader text, char delemiter)
            {
                _text = text;
                _delemiter = delemiter;
            }

            /// <summary>
            ///     Gets the next line.
            /// </summary>
            public CsvLine GetNextLine()
            {
                if (_text.EndOfStream || (char)_text.Peek() == '\0')
                {
                    return null;
                }

                var isStringStarted = false;
                var csvLine = new CsvLine();
                var curToken = string.Empty;

                while (!_text.EndOfStream)
                {
                    var curChar = (char)_text.Read();
                    if (curChar == '\0')
                    {
                        break;
                    }

                    if (curChar == '"')
                    {
                        if (isStringStarted)
                        {
                            if (IsNextSymbolQuote())
                            {
                                curToken += '"';
                                //skip double quote
                                _text.Read();
                                continue;
                            }

                            isStringStarted = false;
                            continue;
                        }

                        if (string.IsNullOrEmpty(curToken))
                        {
                            isStringStarted = true;
                        }
                        else
                        {
                            curToken += '"';
                        }

                        continue;
                    }

                    if (!isStringStarted)
                    {
                        if (curChar == _delemiter)
                        {
                            csvLine.Add(curToken.Replace("\r", string.Empty).Replace("\n", Environment.NewLine));
                            curToken = string.Empty;
                            continue;
                        }

                        if (curChar == '\n')
                        {
                            csvLine.Add(curToken.Replace("\r", string.Empty).Replace("\n", Environment.NewLine));
                            return csvLine;
                        }
                    }

                    curToken += curChar;
                }

                csvLine.Add(curToken);
                return csvLine;
            }

            /// <summary>
            ///     Determines whether [is next symbol quote].
            /// </summary>
            private bool IsNextSymbolQuote()
            {
                return !_text.EndOfStream && (char)_text.Peek() == '"';
            }
        }


        public class CertificateData
        {
            public string Iin { get; set; }
            public string Bin { get; set; }
            public string CertOwnerCorpName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public X509Certificate2 Certificate { get; set; }
        }

        #endregion

        public void Dispose()
        {
            if (_kalkanCryptCom != null)
                _kalkanCryptCom.Finalize();
        }
    }
}