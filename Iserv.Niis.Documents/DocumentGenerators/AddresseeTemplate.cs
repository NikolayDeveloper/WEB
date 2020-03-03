using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(0, "Addressee")]
    public class AddresseeTemplate : DocumentGeneratorBase
    {
        public AddresseeTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, 
            IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override Content PrepareValue()
        {
            var customer = GetCustomer();

            return new Content(
                   new FieldContent("ShortAddress", string.IsNullOrEmpty(customer.ShortAddress) ? customer.NameRu : customer.ShortAddress),
                   new FieldContent("PostAddress", customer.Address),
                   new FieldContent("Phones",  GetPhones(customer)));
        }

        private string GetPhones(DicCustomer customer)
        {
            return string.Join(Environment.NewLine, customer.ContactInfos.Where(d => d.Type.Code == DicContactInfoType.Codes.MobilePhone).Select(d => d.Info));
        }

        private DicCustomer GetCustomer()
        {
            var ownerType = (Owner.Type)(int)Parameters["OwnerType"];
            var ownerId = Convert.ToInt32(Parameters["RequestId"]);
            DicCustomer result = null;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(ownerId));
                    result = request.Addressee;
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(ownerId));
                    result = protectionDoc.Addressee;
                    break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute(ownerId));
                    result = contract.Addressee;
                    break;
                default:
                case Owner.Type.Material:
                    var material = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(ownerId));
                    result = material.Addressee;
                    break;
            }

            return result;
        }
    }
}
