using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertMainEntityRelationsHandler : BaseHandler
    {
        private readonly OldNiisMainEntityRelationService _oldNiisMainEntityRelationService;
        private readonly AppConfiguration _appConfiguration;

        private NewNiisMainEntityRelationService _newNiisMainEntityRelationService;

        public InsertMainEntityRelationsHandler(
            NiisWebContextMigration context,
            OldNiisMainEntityRelationService oldNiisMainEntityRelationService,
            AppConfiguration appConfiguration) : base(context)
        {
            _oldNiisMainEntityRelationService = oldNiisMainEntityRelationService;
            _appConfiguration = appConfiguration;
        }

        public void Execute()
        {

            Console.WriteLine($"1 start migrate ContractProtectionDocRelation");
            MigrateMainEntityRelations(typeof(ContractProtectionDocRelation), nameof(ContractProtectionDocRelation), _oldNiisMainEntityRelationService.GetContractProtectionDocRelations);
            Console.WriteLine($"2 start migrate ProtectionDocDocument");
            MigrateMainEntityRelations(typeof(ProtectionDocDocument), nameof(ProtectionDocDocument), _oldNiisMainEntityRelationService.GetProtectionDocDocumentRelations);
            Console.WriteLine($"3 start migrate ContractDocument");
            MigrateMainEntityRelations(typeof(ContractDocument), nameof(ContractDocument), _oldNiisMainEntityRelationService.GetContractDocumentRelations);
            Console.WriteLine($"4 start migrate ContractRequestRelation");
            MigrateMainEntityRelations(typeof(ContractRequestRelation), nameof(ContractRequestRelation), _oldNiisMainEntityRelationService.GetContractRequestRelations);
            Console.WriteLine($"5 start migrate RequestDocument");
            MigrateMainEntityRelations(typeof(RequestDocument), nameof(RequestDocument), _oldNiisMainEntityRelationService.GetRequestDocumentRelations);
            Console.WriteLine($"6 start migrate DocumentDocumentRelation");
            MigrateMainEntityRelations(typeof(DocumentDocumentRelation), nameof(DocumentDocumentRelation), _oldNiisMainEntityRelationService.GetDocumentDocumentRelations);
            Console.WriteLine($"7 start migrate ProtectionDocProtectionDocRelation");
            MigrateMainEntityRelations(typeof(ProtectionDocProtectionDocRelation), nameof(ProtectionDocProtectionDocRelation), _oldNiisMainEntityRelationService.GetProtectionDocProtectionDocRelations);
        }

        #region Private Methods

        private void MigrateMainEntityRelations(Type newType, string entityName, Func<int, int, IEnumerable<object>> getOldData)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            var countIndex = 0;

            while (true)
            {
                using (var db = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _newNiisMainEntityRelationService = new NewNiisMainEntityRelationService(db);

                    using (var transaction = NewNiisContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var lastBarcode = _newNiisMainEntityRelationService.GetLastBarcode(newType);
                            var mainEntityRelations = getOldData(_appConfiguration.PackageSize, lastBarcode ?? 1);

                            if (mainEntityRelations.Any() == false)
                            {
                                break;
                            }

                            //if (mainEntityRelations is List<DocumentDocumentRelation>)
                            //     Console.WriteLine(string.Join(",", ((List<DocumentDocumentRelation>)mainEntityRelations).Select(r => $"C:{r.ParentId} pd:{r.ChildId} ed{r.ExternalId}")));

                            _newNiisMainEntityRelationService.CreateRangeMainEntityRelations(mainEntityRelations);
                            transaction.Commit();

                            countIndex += _appConfiguration.PackageSize;
                            Console.Write($"\rМигрировано {countIndex} записей");
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex);
                            if (ex.InnerException != null)
                                Console.WriteLine(ex.InnerException);
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }

            Console.WriteLine($"{entityName}s migrated");
        }

        #endregion
    }
}
