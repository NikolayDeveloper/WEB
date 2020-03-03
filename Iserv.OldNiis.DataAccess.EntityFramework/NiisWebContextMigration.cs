using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.OldNiis.DataAccess.EntityFramework.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.OldNiis.DataAccess.EntityFramework
{
    public class NiisWebContextMigration : NiisWebContext
    {
        private readonly List<Type> _addedTypes = new List<Type>
        {
            typeof(ApplicationUser),
            typeof(ApplicationRole),
        };

        private readonly List<Type> _ignoreTypes = new List<Type>
        {
            typeof(Attachment),
            typeof(RequestWorkflow),
            typeof(ContractWorkflow),
            typeof(CustomerAttorneyInfo),
            typeof(DicTypeTrademark),
            typeof(DicSelectionAchieveType),
            typeof(DocumentWorkflow),
            typeof(DicBeneficiaryType),
            //typeof(DicPaymentStatus),
            typeof(DicStageExpirationByDocType),
            typeof(DicPositionType),
            typeof(DicDocumentStatus),
            typeof(DicApplicantType),
            typeof(ProtectionDocCustomer),
            typeof(ICGSProtectionDoc),
            typeof(ICISProtectionDoc),
            typeof(IPCProtectionDoc),
            typeof(ProtectionDocEarlyReg),
            typeof(ProtectionDocRedefine),
            typeof(RequestInfo),
            typeof(ProtectionDocInfo),
            typeof(ProtectionDocWorkflow),

            typeof(DocumentExecutor),
            typeof(DocumentUserSignature),
            typeof(PaymentRegistryData),


            typeof(ContractCustomer),

            typeof(ContractProtectionDocRelation),
            typeof(ProtectionDocDocument),
            typeof(ContractDocument),
            typeof(ContractRequestRelation),
            typeof(RequestDocument),
            typeof(DocumentDocumentRelation),
            typeof(ProtectionDocProtectionDocRelation),
            typeof(DicTariffProtectionDocType)
        };

        public NiisWebContextMigration(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(600);
        }

        public override int SaveChanges()
        {
            UpdateShadowProperties();

            var tableNames = GetCurrentTableNames();

            SetTablesIdentityInsertOn(tableNames);
            var result = base.SaveChanges();
            SetTablesIdentityInsertOff(tableNames);

            return result;
        }

        #region Private Methods

        private void SetTablesIdentityInsertOn(List<string> tableNames)
        {
            foreach (var tableName in tableNames)
            {
                CustomSqlQuery.SetTableIdentityInsertOn(this, tableName);
            }
        }

        private void SetTablesIdentityInsertOff(List<string> tableNames)
        {
            foreach (var tableName in tableNames)
            {
                CustomSqlQuery.SetTableIdentityInsertOff(this, tableName);
            }
        }

        private List<string> GetCurrentTableNames()
        {
            var tableNames = new List<string>();

            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.State != EntityState.Added)
                {
                    continue;
                }
                var entityType = changedEntity.Entity.GetType();
                if ((changedEntity.Entity is Entity<int> || _addedTypes.Contains(entityType))
                    && _ignoreTypes.Contains(entityType) == false)
                {
                    var tableName = this.GetTableNameByEntityType(entityType);
                    if (tableNames.Contains(tableName) == false)
                    {
                        tableNames.Add(tableName);
                    }
                }
            }

            return tableNames;

        }
        private void UpdateShadowProperties()
        {
            var entries = ChangeTracker.Entries()
                .Where(x =>
                    (x.State == EntityState.Added || x.State == EntityState.Modified)
                    && x.Entity is DictionaryEntity<int>);

            foreach (var item in entries)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Property("DateCreate").CurrentValue = DateTimeOffset.Now;
                        item.Property("DateUpdate").CurrentValue = DateTimeOffset.Now;
                        break;
                    case EntityState.Modified:
                        item.Property("DateUpdate").CurrentValue = DateTimeOffset.Now;
                        break;
                }
            }
        }
        #endregion
    }
}
