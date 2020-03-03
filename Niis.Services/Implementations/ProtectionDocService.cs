using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowServices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Services.Implementations
{
    public class ProtectionDocService : IProtectionDocService
    {
        private readonly IExecutor _executor;

        public ProtectionDocService(IExecutor executor)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public async Task<int[]> GenerateGosNumbers(int[] protectionDocIds, SelectionMode selectionMode, bool hasIpc, bool isAllSelected)
        {
            protectionDocIds = _executor.GetHandler<GetProtectionDocSelectionFromJournalHandler>()
                .Process(h => h.Execute(isAllSelected, hasIpc, selectionMode, protectionDocIds));

            await _executor.GetHandler<GenerateGosNumbersForMultipleProtectionDocsHandler>()
               .Process(c => c.ExecuteAsync(protectionDocIds));

            return protectionDocIds;
        }

        public void StartWorkflowProccess(ProtectionDocumentWorkFlowRequest workFlowRequest)
        {
            NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(workFlowRequest);
        }

        public async Task CreateAuthorsCertificate(int protectionDocId, int[] authorIds, int userId)
        {
            var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(protectionDocId));
            string authorCertificateCode = null;
            switch (protectionDoc.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                    authorCertificateCode = DicDocumentTypeCodes.InventionAuthorCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                    switch (protectionDoc.SelectionAchieveType?.Code)
                    {
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            authorCertificateCode = DicDocumentTypeCodes.AnimalHusbandrySelectiveAchievementAuthorCertificate;
                            break;
                        case DicSelectionAchieveTypeCodes.Agricultural:
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            authorCertificateCode = DicDocumentTypeCodes.AgriculturalSelectiveAchievementAuthorCertificate;
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                    authorCertificateCode = DicDocumentTypeCodes.UsefulModelAuthorCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                    authorCertificateCode = DicDocumentTypeCodes.IndustrialDesignAuthorCertificate;
                    break;
            }
            if (authorCertificateCode != null)
            {
                var authors =
                    protectionDoc.ProtectionDocCustomers.Where(pc => authorIds.Contains(pc.Id));

                List<PaymentInvoice> invoices = null;
                if (protectionDoc.PaymentInvoices == null || protectionDoc.PaymentInvoices.Count < 1)
                    throw new DataNotFoundException(nameof(PaymentInvoice), DataNotFoundException.OperationType.Read, protectionDocId);
                invoices = protectionDoc
                    .PaymentInvoices
                    .Where(pi => new string[] { "702", "702.1", "702.2", "702.3" }.Contains(pi.Tariff.Code) && pi.Status.Code == DicPaymentStatusCodes.Credited).ToList();
                if (invoices == null || invoices.Count < 1)
                    throw new DataNotFoundException(nameof(PaymentInvoice), DataNotFoundException.OperationType.Read, protectionDocId);

                for (int i = 0; i < authors.Count(); i++)
                {
                    var userInputDto = new UserInputDto
                    {
                        Code = authorCertificateCode,
                        Fields = new List<KeyValuePair<string, string>>(),
                        OwnerId = protectionDocId,
                        OwnerType = Owner.Type.ProtectionDoc,
                        SelectedRequestIds = new List<int> { protectionDocId },
                        Index = i + 1, 
                    };

                    var documentType = _executor.GetQuery<BusinessLogic.Dictionaries.DicDocumentType.GetDicDocumentTypeByCodeQuery>()
                                        .Process(q => q.Execute(authorCertificateCode));
                    if (documentType == null)
                        throw new DataNotFoundException(nameof(Domain.Entities.Dictionaries.DicDocumentType), DataNotFoundException.OperationType.Read,
                            authorCertificateCode);

                    int currentUserId = userId;
                    int? addresseeId = protectionDoc.ProtectionDocCustomers.FirstOrDefault(c =>
                                c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence)?.CustomerId;
                    

                    var document = new Domain.Entities.Document.Document
                    {
                        TypeId = documentType.Id,
                        AddresseeId = addresseeId,
                        DocumentType = DocumentType.Internal,
                        DateCreate = DateTimeOffset.Now,
                        DateUpdate = DateTimeOffset.Now,
                        StatusId = _executor.GetQuery<BusinessLogic.Dictionaries.DicDocumentStatusQuery.GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork)).Id

                    };
                    var documentId = await _executor.GetCommand<CreateDocumentCommand>().Process(c => c.ExecuteAsync(document));

                    await _executor.GetCommand<BusinessLogic.Documents.ManyToManyRelations.AddProtectionDocDocumentsCommand>().Process(c =>
                        c.ExecuteAsync(new List<Domain.Entities.ProtectionDoc.ProtectionDocDocument>
                        {
                            new Domain.Entities.ProtectionDoc.ProtectionDocDocument
                            {
                                ProtectionDocId = protectionDocId,
                                DocumentId = documentId
                            }
                        }));

                    var documentWorkflow = await _executor.GetQuery<BusinessLogic.Workflows.Documents.GetInitialDocumentWorkflowQuery>().Process(q =>
                        q.ExecuteAsync(documentId, userId));
                    await _executor.GetCommand<BusinessLogic.Workflows.Documents.ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(documentWorkflow));
                    await _executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
                    await _executor.GetHandler<BusinessLogic.Documents.Numbers.GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));
                    await _executor.GetHandler<BusinessLogic.Documents.Numbers.GenerateRegisterDocumentNumberHandler>().Process(c => c.ExecuteAsync(documentId));
                    await _executor.GetCommand<BusinessLogic.Documents.UserInput.CreateUserInputCommand>().Process(c => c.ExecuteAsync(documentId, userInputDto));
                    
                    if(documentId > 0 && invoices.Count > 0)
                    {
                        var invoice = invoices[i];
                        invoice.StatusId = _executor.GetHandler<GetPaymentStatusByCodeQuery>().Process(x => x.Execute(DicPaymentStatusCodes.Distributed)).Id;
                        invoice.DateUpdate = DateTimeOffset.Now;
                        invoice.WriteOffUserId = NiisAmbientContext.Current.User.Identity.UserId;
                    
                    }
                }

                _executor.GetCommand<UpdatePaymentInvoicesCommand>().Process(x => x.Execute(invoices));
            }
        }
    }
}
