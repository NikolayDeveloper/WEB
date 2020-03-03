using Iserv.Niis.Infrastructure.Abstract;
using Iserv.Niis.Infrastructure.Implementations;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using Iserv.Niis.DI;

namespace Iserv.Niis.BusinessLogic.Sing
{
    public class ValidateSingHandler : BaseHandler
    {
        private readonly ICertificateService _certificateService;

        public ValidateSingHandler(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public bool Execute(Domain.Entities.Document.DocumentUserSignature documentUserSignature)
        {
            var certificateData = _certificateService.GetCertificateData(documentUserSignature.SignerCertificate, CertificateService.GostAlgType);
            if (!_certificateService.VerifyGostCertificate(certificateData.Certificate, documentUserSignature.SignedData))
            {
                throw new Exception("Certificate is not valid");
            }

            var userXin = NiisAmbientContext.Current.User.Identity.UserXin;
            if (!userXin.Equals(certificateData.Bin) && !userXin.Equals(certificateData.Iin))
            {
                throw new Exception("XIN not suitable");
            }

            return true;
        }
    }
}
