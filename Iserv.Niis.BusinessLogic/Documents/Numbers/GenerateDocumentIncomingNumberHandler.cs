using System;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDivision;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Exceptions;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateDocumentIncomingNumberHandler: BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));

            await ExecuteAsync(document);
        }

        public async Task ExecuteAsync(Document document)
        {
            if (!string.IsNullOrEmpty(document.IncomingNumber))
                return;

            var documentType = await Executor.GetQuery<GetDicDocumentTypeByIdQuery>().Process(q => q.ExecuteAsync(document.TypeId));
            if (DicDocumentTypeCodes.IgnoreGenerateIncomingNumber().Contains(documentType?.Code))
                return;

            if (document.DocumentType != DocumentType.Incoming)
                throw new Exception("Can not generate incoming number for not incoming document!");

            var division = Executor.GetQuery<GetDicDivisionByIdQuery>()
                .Process(q => q.Execute(document.DivisionId ?? 0));
            if (division == null)
                throw new DataNotFoundException(nameof(DicDepartment), DataNotFoundException.OperationType.Read,
                    document.Id);
            int count;

            if (division.Code.Equals(DicDivisionCodes.RGP_NIIS))
            {
                count = Executor.GetHandler<GetNextCountHandler>().Process(h =>
                    h.Execute(NumberGenerator.DocumentIncomingNumberCodePrefix + division.Code));

                document.IncomingNumber =
                    $"{DateTime.Now.Year}-{count:D5}";
            }
            else
            {
                if (document.IncomingNumberFilial != null)
                {
                    return;
                }
                count = Executor.GetHandler<GetNextCountHandler>().Process(h =>
                    h.Execute(NumberGenerator.DocumentIncomingNumberFilialCodePrefix + division.Code));

                document.IncomingNumberFilial =
                    $"{count:D5}-{division.IncomingNumberCode}";
            }
        }
    }
}
