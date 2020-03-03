using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertRequestRelationsHandler : BaseHandler
    {
        private NewNiisRequestRelationService _newNiisRequestRelationService;
        private readonly OldNiisRequestRelationService _oldNiisRequestRelationService;
        private readonly OldNiisRequestService _oldNiisRequestService;

        public InsertRequestRelationsHandler(NiisWebContextMigration context,
            OldNiisRequestRelationService oldNiisRequestRelationService,
            OldNiisRequestService oldNiisRequestService) : base(context)
        {
            _oldNiisRequestRelationService = oldNiisRequestRelationService;
            _oldNiisRequestService = oldNiisRequestService;
        }
        

        public void Execute(List<int> requestIds, NiisWebContextMigration niisWebContext)
        {
            _newNiisRequestRelationService = new NewNiisRequestRelationService(niisWebContext);
            InsertDicColorTZRequestRelations(requestIds);
            InsertRequestCustomers(requestIds);
            InsertDicIcfemRequestRelations(requestIds);
            InsertICGSRequests(requestIds);
            InsertICISRequests(requestIds);
            InsertIPCRequests(requestIds);
            InsertRequestEarlyRegs(requestIds);
        }

        private void InsertDicColorTZRequestRelations(List<int> requestIds)
        {
            var requestColors = _oldNiisRequestRelationService.GetRequestColotTZs(requestIds);
            if (requestColors.Any())
                _newNiisRequestRelationService.CreateRangeDicColorTZRequests(requestColors);
        }

        private void InsertRequestCustomers(List<int> requestIds)
        {
            var requestCustomers = _oldNiisRequestRelationService.GetRequestCustomers(requestIds);
            if (requestCustomers.Any())
                _newNiisRequestRelationService.CreateRangeRequestCustomers(requestCustomers);
        }

        private void InsertDicIcfemRequestRelations(List<int> requestIds)
        {
            var icfemRequestRelations = _oldNiisRequestRelationService.GetDicIcfemRequestRelations(requestIds);
            if (icfemRequestRelations.Any())
                _newNiisRequestRelationService.CreateRangeDicIcfemRequestRelations(icfemRequestRelations);
        }

        private void InsertICGSRequests(List<int> requestIds)
        {
            var icgsRequests = _oldNiisRequestRelationService.GetICGSRequests(requestIds);
            if (icgsRequests.Any())
                _newNiisRequestRelationService.CreateRangeICGSRequests(icgsRequests);
        }

        private void InsertICISRequests(List<int> requestIds)
        {
            var icisRequests = _oldNiisRequestRelationService.GetICISRequests(requestIds);
            if (icisRequests.Any())
                _newNiisRequestRelationService.CreateRangeICISRequests(icisRequests);
        }

        private void InsertIPCRequests(List<int> requestIds)
        {
            var ipcRequests = _oldNiisRequestRelationService.GetIPCRequests(requestIds);
            if (ipcRequests.Any())
                _newNiisRequestRelationService.CreateRangeIPCRequests(ipcRequests);
        }

        private void InsertRequestEarlyRegs(List<int> requestIds)
        {
            var requestEarlyRegs = _oldNiisRequestRelationService.GetRequestEarlyRegs(requestIds);
            if (requestEarlyRegs.Any())
                _newNiisRequestRelationService.CreateRangeRequestEarlyRegs(requestEarlyRegs);
        }
    }
}
