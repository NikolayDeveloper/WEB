using System;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iserv.Niis.DataAccess.EntityFramework.Infrastructure
{
    public static class MappingExtensions
    {
        /* public static ModelBuilder MapConcurrencyTokens(this ModelBuilder modelBuilder)
         {
             var interfaceType = typeof(IHaveConcurrencyToken);
             var entityTypes = modelBuilder.Model.GetEntityTypes()
                 .Where(x => x.ClrType.GetInterfaces().Contains(interfaceType));

             foreach (var entityType in entityTypes)
             {
                 var property = entityType.GetOrAddProperty("Timestamp", typeof(byte[]));
                 property.IsConcurrencyToken = true;
                 property.ValueGenerated = ValueGenerated.OnAddOrUpdate;
             }

             return modelBuilder;
         }*/

        /*
         *public static ModelBuilder MapShadowProperties(this ModelBuilder modelBuilder)
         {
             var interfaceType = typeof(IEntity<>);
             var entityTypes = modelBuilder.Model.GetEntityTypes()
                 .Where(x => x.ClrType.GetInterfaces()
                     .Any(i =>
                         i.IsGenericType
                         && i.GetGenericTypeDefinition() == interfaceType))
                 .ToList();

             foreach (var entityType in entityTypes)
             {
                 entityType.GetOrAddProperty("DateCreate", typeof(DateTimeOffset));
                 entityType.GetOrAddProperty("DateUpdate", typeof(DateTimeOffset));
             }
             return modelBuilder;
         }
         */

        public static ModelBuilder MapRestrictionsForBasicPropertiesOfDictionary(this ModelBuilder modelBuilder)
        {
            Type interfaceType = typeof(IDictionaryEntity<>);

            var mutableEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(x => x.ClrType.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == interfaceType));

            foreach (var mutableEntityType in mutableEntityTypes)
            {
                mutableEntityType.FindProperty("Code").SetMaxLength(50);
                mutableEntityType.FindProperty("NameRu").SetMaxLength(2000);
                mutableEntityType.FindProperty("NameEn").SetMaxLength(2000);
                mutableEntityType.FindProperty("NameKz").SetMaxLength(2000);
                //mutableEntityType.FindProperty("Description").SetMaxLength(4000);
            }

            var longNameTypes = mutableEntityTypes.Where(x => x.ClrType.GetInterface(nameof(IHaveLongName), true) != null);

            foreach (var mutableEntityType in longNameTypes)
            {
                mutableEntityType.FindProperty("Code").SetMaxLength(50);
                mutableEntityType.FindProperty("NameRu").SetMaxLength(4000);
                mutableEntityType.FindProperty("NameEn").SetMaxLength(4000);
                mutableEntityType.FindProperty("NameKz").SetMaxLength(4000);
            }

            return modelBuilder;
        }

        public static ModelBuilder MapPrimaryKeysGenerationOnAddStrategy(this ModelBuilder modelBuilder)
        {
            var primaryKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(x => x.IsPrimaryKey()));

            foreach (var primaryKey in primaryKeys)
            {
                primaryKey.ValueGenerated = ValueGenerated.OnAdd;
            }

            return modelBuilder;
        }

        public static ModelBuilder MapForeignKeysOnCascadeDeleteBehavior(this ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                //TODO: Workaround. Убрать неявное назначение DeleteBehavior
                if ((relationship.DeclaringEntityType.ClrType == typeof(RequestInfo) || relationship.DeclaringEntityType.ClrType == typeof(ContractRequestRelation) || relationship.DeclaringEntityType.ClrType == typeof(ContractRequestICGSRequestRelation)) && 
                    relationship.DeleteBehavior == DeleteBehavior.Cascade) 
                {
                    continue;
                }

                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            return modelBuilder;
        }

        public static ModelBuilder MapModelsSpecificProperties(this ModelBuilder modelBuilder)
        {
            var iMapBuilderTypes = typeof(IMapBuilder).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IMapBuilder)))
                .ToList();

            foreach (var type in iMapBuilderTypes)
            {
                var instance = Activator.CreateInstance(type) as IMapBuilder;
                instance?.Build(modelBuilder);
            }

            return modelBuilder;
        }
    }
}