using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System.Collections.Generic;
using System.Linq;


namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertProtectionDocRelationsHandler : BaseHandler
    {
        private readonly OldNiisProtectionDocRelationService _oldNiisProtectionDocRelationService;
        private NewNiisProtectionDocRelationService _newNiisProtectionDocRelationService;

        public InsertProtectionDocRelationsHandler(
            OldNiisProtectionDocRelationService oldNiisProtectionDocRelationService,
            NiisWebContextMigration context) : base(context)
        {
            _oldNiisProtectionDocRelationService = oldNiisProtectionDocRelationService;
        }

        public void Execute(List<int> protectionDocIds, NiisWebContextMigration niisWebContext)
        {
            _newNiisProtectionDocRelationService = new NewNiisProtectionDocRelationService(niisWebContext);

            InsertDicColorTZProtectionDocRelation(protectionDocIds);
            InsertProtectionDocCustomers(protectionDocIds);
            InsertProtectionDocIcfems(protectionDocIds);
            InsertProtectionDocIcgses(protectionDocIds);
            InsertProtectionDocIcises(protectionDocIds);
            InsertProtectionDocIpcs(protectionDocIds);
            InsertProtectionDocEarlyRegs(protectionDocIds);
            InsertProtectionDocRedefines(protectionDocIds);
        }

        #region Private Methods

        private void InsertDicColorTZProtectionDocRelation(List<int> protectionDocIds)
        {
            var protectionDocColors = _oldNiisProtectionDocRelationService.GetDicColorTZProtectionDocRelations(protectionDocIds);
            if (protectionDocColors.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocColors);
            }
        }

        private void InsertProtectionDocCustomers(List<int> protectionDocIds)
        {
            var protectionDocCustomers = _oldNiisProtectionDocRelationService.GetProtectionDocCustomers(protectionDocIds);
            if (protectionDocCustomers.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocCustomers);
            }
        }

        private void InsertProtectionDocIcfems(List<int> protectionDocIds)
        {
            var protectionDocIcfems = _oldNiisProtectionDocRelationService.GetDicIcfemProtectionDocRelations(protectionDocIds);
            if (protectionDocIcfems.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocIcfems);
            }
        }

        private void InsertProtectionDocIcgses(List<int> protectionDocIds)
        {
            var protectionDocIcgses = _oldNiisProtectionDocRelationService.GetICGSProtectionDocs(protectionDocIds);
            if (protectionDocIcgses.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocIcgses);
            }
        }

        private void InsertProtectionDocIcises(List<int> protectionDocIds)
        {
            var protectionDocIcises = _oldNiisProtectionDocRelationService.GetICISProtectionDocs(protectionDocIds);
            if (protectionDocIcises.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocIcises);
            }
        }

        private void InsertProtectionDocIpcs(List<int> protectionDocIds)
        {
            var protectionDocIpcs = _oldNiisProtectionDocRelationService.GetIPCProtectionDocs(protectionDocIds);
            if (protectionDocIpcs.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocIpcs);
            }
        }

        private void InsertProtectionDocEarlyRegs(List<int> protectionDocIds)
        {
            var protectionDocEarlyRegs = _oldNiisProtectionDocRelationService.GetProtectionDocEarlyRegs(protectionDocIds);
            if (protectionDocEarlyRegs.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocEarlyRegs);
            }
        }

        private void InsertProtectionDocRedefines(List<int> protectionDocIds)
        {
            var protectionDocRedefines = _oldNiisProtectionDocRelationService.GetProtectionDocRedefines(protectionDocIds);
            if (protectionDocRedefines.Any())
            {
                _newNiisProtectionDocRelationService.CreateRangeProtectionDocRelations(protectionDocRedefines);
            }
        }
        #endregion
    }
}
