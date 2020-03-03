using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ZXing;
using ZXing.QrCode;

namespace Iserv.Niis.Documents.Infrastructure
{
    public class QrCodeGenerator
    {
        private int _maxDataLength = 900;

        public IEnumerable<Image> BuildQrCodes(string data)
        {
            IList<Image> result = new List<Image>();

            if (data.Length > _maxDataLength)
            {
                var dataParts = GenerateQrCodeDataParts(data);
                foreach (var dataPart in dataParts)
                    result.Add(BuildImage(dataPart));
            }
            else
            {
                result.Add(BuildImage(data));
            }

            return result;
        }


        private Image BuildImage(string data)
        {
            QrCodeEncodingOptions options;
            BarcodeWriter writer;

            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Margin = 5
            };

            writer = new BarcodeWriter
            {
                Options = options,
                Format = BarcodeFormat.QR_CODE
            };
            return new Bitmap(writer.Write(data));
        }

        private List<string> GenerateQrCodeDataParts(string data)
        {
            List<string> dataParts = (from Match m in Regex.Matches(data, @".{1," + _maxDataLength + "}")
                             select m.Value).ToList();

            List<string> result = new List<string>();

            int i = 1;
            foreach (var dataPart in dataParts)
            {
                XmlDocument xml = new XmlDocument();
                xml.AppendChild(xml.CreateElement("QrCode"));
                XmlElement root = xml.DocumentElement;

                XmlElement dataElem = xml.CreateElement("data");
                dataElem.InnerText = dataPart;

                XmlElement metaElem = xml.CreateElement("meta");
                metaElem.Attributes.Append(xml.CreateAttribute("part"));
                metaElem.Attributes.Append(xml.CreateAttribute("date"));
                metaElem.Attributes["part"].Value = i + " of " + dataParts.Count;
                metaElem.Attributes["date"].Value = DateTime.Now.ToShortDateString();
               
                root.AppendChild(metaElem);
                root.AppendChild(dataElem);

                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Position = 0;
                    xml.Save(stream);
                    result.Add(XmlToString(stream));
                }
                i++;
            }

            result.Reverse();
            return result;
        }

        private string XmlToString(MemoryStream stream)
        {
            return Encoding.Default.GetString(stream.GetBuffer()); ;
        }
    }
}
