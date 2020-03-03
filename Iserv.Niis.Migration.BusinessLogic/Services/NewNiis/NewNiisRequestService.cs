using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisRequestService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisRequestService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeRequests(List<Request> requests)
        {
            _context.Requests.AddRange(requests);
            _context.SaveChanges();
        }

        public void UpdateRangeRequests(List<Request> requests)
        {
            _context.Requests.UpdateRange(requests);
            _context.SaveChanges();
        }

        public void CreateRangeRequestWorkflows(List<RequestWorkflow> requestWorkflows)
        {
            _context.RequestWorkflows.AddRange(requestWorkflows);
            _context.SaveChanges();
        }

        public void CreateRangeRequestInfos(List<RequestInfo> requestInfos)
        {
            _context.RequestInfos.AddRange(requestInfos);
            _context.SaveChanges();
        }

        public int? GetLastBarcodeOfRequest()
        {
            return _context.Requests
                 .AsNoTracking()
                 .OrderByDescending(d => d.Id)
                 .FirstOrDefault()?.Barcode;
        }

        public int GetRequetsCount()
        {
            return _context.Requests
                .AsNoTracking()
                .Count();
        }
    }
}
