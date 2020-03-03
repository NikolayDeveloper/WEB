using System.Security.Cryptography.X509Certificates;
using Iserv.Niis.Infrastructure.Implementations;

namespace Iserv.Niis.Infrastructure.Abstract
{
    public interface ICertificateService
    {
        CertificateService.CertificateData GetCertificateData(string certData, string algType);
        bool VerifyRsaCertificate(X509Certificate2 certificate, string plainData, string signPlainData);
        bool VerifyGostCertificate(X509Certificate2 certificate, string signedData);
    }
}