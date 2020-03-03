using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisRequestRelationService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisRequestRelationService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeDicColorTZRequests(List<DicColorTZRequestRelation> colorsRequests)
        {
            _context.DicColorTzRequestRelations.AddRange(colorsRequests);
            _context.SaveChanges();
        }

        public void CreateRangeRequestCustomers(List<RequestCustomer> requestCustomers)
        {
            _context.RequestCustomers.AddRange(requestCustomers);
            _context.SaveChanges();
        }

        public void CreateRangeDicIcfemRequestRelations(List<DicIcfemRequestRelation> icfemRequestRelations)
        {
            _context.DicIcfemRequestRelations.AddRange(icfemRequestRelations);
            _context.SaveChanges();
        }

        public void CreateRangeICGSRequests(List<ICGSRequest> icgsRequest)
        {
            _context.ICGSRequests.AddRange(icgsRequest);
            _context.SaveChanges();
        }

        public void CreateRangeICISRequests(List<ICISRequest> icgsRequest)
        {
            _context.ICISRequests.AddRange(icgsRequest);
            _context.SaveChanges();
        }

        public void CreateRangeIPCRequests(List<IPCRequest> ipcRequest)
        {
            _context.IPCRequests.AddRange(ipcRequest);
            _context.SaveChanges();
        }

        public void CreateRangeRequestEarlyRegs(List<RequestEarlyReg> requestEarlyReg)
        {
            _context.RequestEarlyRegs.AddRange(requestEarlyReg);
            _context.SaveChanges();
        }
    }
}
