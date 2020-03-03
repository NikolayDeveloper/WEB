using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Infrastructure.Abstract;
using Iserv.Niis.Infrastructure.Implementations;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Workflow.Abstract;
using MediatR;

namespace Iserv.Niis.Features.Materials
{
    public class SignDocument
    {
        public class Command : IRequest<DocumentUserSignatureDto>
        {
            public Command(DocumentUserSignatureDto documentUserSignatureDto)
            {
                DocumentUserSignatureDto = documentUserSignatureDto;
            }

            public DocumentUserSignatureDto DocumentUserSignatureDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(s => s.DocumentUserSignatureDto).NotNull();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, DocumentUserSignatureDto>
        {
            private readonly ICertificateService _certificateService;
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly ISignedDocumentApplier<Domain.Entities.Request.Request> _signedDocumentApplierForRequest;
            private readonly ISignedDocumentApplier<Domain.Entities.Contract.Contract> _signedDocumentApplierForContract;

            public CommandHandler(
                NiisWebContext context,
                IMapper mapper,
                ICertificateService certificateService,
                ISignedDocumentApplier<Domain.Entities.Request.Request> signedDocumentApplierForRequest,
                ISignedDocumentApplier<Domain.Entities.Contract.Contract> signedDocumentApplierForContract
                )
            {
                _context = context;
                _mapper = mapper;
                _certificateService = certificateService;
                _signedDocumentApplierForRequest = signedDocumentApplierForRequest;
                _signedDocumentApplierForContract = signedDocumentApplierForContract;
            }

            public async Task<DocumentUserSignatureDto> Handle(Command message)
            {
                var documentSigner = _mapper.Map<DocumentUserSignature>(message.DocumentUserSignatureDto);
                var certificateData =
                    _certificateService.GetCertificateData(documentSigner.SignerCertificate,
                        CertificateService.GostAlgType);
                if (!_certificateService.VerifyGostCertificate(certificateData.Certificate,
                    documentSigner.SignedData))
                {
                    throw new Exception("Certificate is not valid");
                }

                var currentUserXin = _context.Users.Single(u => u.Id == documentSigner.UserId).XIN;
                if (!currentUserXin.Equals(certificateData.Bin) && !currentUserXin.Equals(certificateData.Iin))
                {
                    throw new Exception("XIN not suitable");
                }
                documentSigner.IsValidCertificate = true;
                var documentId = _context.DocumentWorkflows.Find(documentSigner.WorkflowId)?.OwnerId;
                if (documentId.HasValue)
                {
                    await _signedDocumentApplierForRequest.ApplyAsync(documentSigner.UserId, documentId.Value);
                    await _signedDocumentApplierForContract.ApplyAsync(documentSigner.UserId, documentId.Value);
                }
                

                _context.DocumentUserSignatures.Add(documentSigner);
                await _context.SaveChangesAsync();
                return _mapper.Map<DocumentUserSignatureDto>(documentSigner);
            }
        }
    }
}