using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetRequisitionInfoService : IGetRequisitionInfoService
    {
        private const string ImageName = "image.png";

        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly NiisWebContext _niisContext;
        private readonly IntegrationValidationHelper _validationHelper;

        public GetRequisitionInfoService(NiisWebContext niisContext,
            IntegrationAttachFileHelper integrationAttachFileHelper, IntegrationValidationHelper validationHelper)
        {
            _niisContext = niisContext;
            _attachFileHelper = integrationAttachFileHelper;
            _validationHelper = validationHelper;
        }
        //TODO возможно информацию нужно будет брать еще и с материалов
        public void GetRequisitionInfo(GetRequisitionInfoArgument argument, GetRequisitionInfoResult result)
        {
            var image = _niisContext.Requests
                .Where(x => x.Barcode == argument.DocumentID)
                .Select(x => x.Image)
                .FirstOrDefault();
            if (image != null)
            {
                if (_validationHelper.SenderIsPep(argument.SystemInfo.Sender))
                {
                    var shepFile = _attachFileHelper.ShepFileUpload(image, ImageName);
                    result.Image = new File { ShepFile = shepFile };
                }
                else
                {
                    result.Image = new File { Content = image, Name = ImageName };
                }
            }
            result.ApplicantList = GetApplicants(argument.DocumentID).ToArray();
            result.DocumentID = argument.DocumentID;
        }
        private List<Applicant> GetApplicants(int requestOrDocId)
        {
            var customers = _niisContext.ProtectionDocs
                .Include(pd => pd.ProtectionDocCustomers).ThenInclude(x => x.Customer)
                .Include(pd => pd.ProtectionDocCustomers).ThenInclude(x => x.CustomerRole)
                .Where(x => x.Request.Barcode == requestOrDocId && x.ProtectionDocCustomers != null)
                .SelectMany(x => x.ProtectionDocCustomers)
                .Where(x => x.CustomerRole != null && x.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
                .Select(x => new { x.Customer.Id, x.Customer.Xin, x.Customer.NameRu });

            if (!customers.Any())
            {
                customers = _niisContext.DocumentCustomers
                    .Include(x => x.Customer)
                    .Where(x => x.Document.Barcode == requestOrDocId
                                && x.CustomerRole != null
                                && x.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
                    .Select(x => new {Id = x.CustomerId.Value, x.Customer.Xin, x.Customer.NameRu});
            }

            return MakeApplicants(customers);
        }

        private List<Applicant> MakeApplicants(dynamic customers)
        {
            var applicants = new List<Applicant>();
            foreach (var item in customers)
                applicants.Add(new Applicant
                {
                    XIN = item.Xin,
                    CustomerId = item.Id,
                    Name = item.NameRu
                });
            return applicants;
        }
    }
}