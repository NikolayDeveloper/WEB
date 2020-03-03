using System;
using System.Web;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class ContractRequest : SystemInfo
    {
        public ReferenceInfo DocumentType { get; set; } //Тип договора

        public ContractPatent[] PatentNumbers { get; set; } //Номера патентов

        private string _descriptionEn;
        public string DocumentDescriptionEn
        {
            get
            {
                return _descriptionEn;
            }
            set
            {
                _descriptionEn = HttpUtility.HtmlDecode(value);
            }
        }

        private string _descriptionRu;
        public string DocumentDescriptionRu
        {
            get
            {
                return _descriptionRu;
            }
            set
            {
                _descriptionRu = HttpUtility.HtmlDecode(value);
            }
        }
        private string _descriptionKz;
        public string DocumentDescriptionKz
        {
            get
            {
                return _descriptionKz;
            }
            set
            {
                _descriptionKz = HttpUtility.HtmlDecode(value);
            }
        }

        public Addressee Addressee { get; set; } //Адресат

        public CorrespondenceAddress CorrespondenceAddress { get; set; } //Адрес для переписки

        public Customer[] BlockCustomers { get; set; } //Кастомеры (к примеру сторона 1 и сторона 2)

        public EgovPay EgovPayment { get; set; } //оплата егов

        public File RequisitionFile { get; set; } // файл заявки

        public AttachedFile[] AttachmentFiles { get; set; } //доп.прикрепляемые файлы
    }
}
