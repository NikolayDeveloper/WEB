using System;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations
{
    public class IntegrationDocumentService : IIntegrationDocumentService
    {
        private readonly Configuration _configuration;
        private readonly NiisWebContext _niisWebContext;

        public IntegrationDocumentService(
            NiisWebContext niisWebContext,
            Configuration configuration)
        {
            _niisWebContext = niisWebContext;
            _configuration = configuration;
        }

        public IQueryable<IntegrationDocument> GetUnsentDocuments()
        {
            return _niisWebContext.IntegrationDocuments.FromSql(
                $"select * from dbo.IntegrationDocuments where DateSent is null and datediff(MINUTE, DateCreate, getdate()) > {_configuration.DocWaitTimeInMinutes}");
        }

        public void MarkSentDocument(int id)
        {
            var integrationDocument = _niisWebContext.IntegrationDocuments.FirstOrDefault(x => x.Id == id);
            if (integrationDocument != null)
            {
                integrationDocument.DateSent = DateTime.Now;
                _niisWebContext.SaveChanges();
            }
        }
    }
}