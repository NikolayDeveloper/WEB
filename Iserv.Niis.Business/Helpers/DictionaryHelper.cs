using System;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Business.Helpers
{
    public class DictionaryHelper
    {
        private readonly NiisWebContext _niisContext;
        private readonly IDicTypeResolver _dicTypeResolver;

        public DictionaryHelper(NiisWebContext niisContext, IDicTypeResolver dicTypeResolver)
        {
            _niisContext = niisContext;
            _dicTypeResolver = dicTypeResolver;
        }

        public List<int> GetChildClass(int[] parentsId)
        {
            var childClass = new List<int>();
            var allDocClass = _niisContext.DicDocumentClassifications
                .ToList();
            SetChildClass(allDocClass, parentsId, childClass);
            return childClass;
        }

        //public int? GetTypeTrademarkId(bool isTmInStandartFontField, bool isTmVolumeField)
        //{
        //    return null;
        //}


        public int GetSpeciesTradeMarkId(bool isCollectiveTradeMark)
        {
            //Коллективный товарный знак
            const string ktm = DicProtectionDocSubtypeCodes.CollectiveTrademark;
            //Товарный знак
            const string ttm = DicProtectionDocSubtypeCodes.RegularTradeMark;

            if (isCollectiveTradeMark)
                return GetDictionaryIdByCode(nameof(DicProtectionDocSubType), ktm);

            return GetDictionaryIdByCode(nameof(DicProtectionDocSubType), ttm);
        }

        public (DocumentType type, int typeId) GetDocumentType(int typeId)
        {
            var type = GetDictionaryByExternalId(nameof(DicDocumentType), typeId);
            if (type.IsDeleted)
            {
                var actualType = GetActualDictionaryByCode(nameof(DicDocumentType), type.Code);
                if (actualType != null) type = actualType;
            }
            if (type.RouteId == null) return (DocumentType.Incoming, type.Id);
            var routeCode = GetDictionaryCodeById(nameof(DicRoute), type.RouteId);

            switch (routeCode)
            {
                case DicRoute.Codes.IN:
                    return (DocumentType.Incoming, type.Id);

                case DicRoute.Codes.OUT:
                    return (DocumentType.Outgoing, type.Id);

                case DicRoute.Codes.W:
                    return (DocumentType.Internal, type.Id);

                case DicRoute.Codes.DR:
                    return (DocumentType.DocumentRequest, type.Id);

                default:
                    return (DocumentType.Incoming, type.Id);
            }
        }
        
        public bool ChechDictionaryId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryCode = dictionaries.Where(d => d.Id == id).Select(d => d.Code).FirstOrDefault();
            if (string.IsNullOrEmpty(dictionaryCode))
            {
                return false;
            }

            return true;
        }

        public int GetDictionaryIdByCode(string entityName, string code)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryId = dictionaries.Where(d => d.Code == code && d.IsDeleted == false).Select(d => d.Id).FirstOrDefault();
            if (dictionaryId == default(int))
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, code);
            }
            return dictionaryId;
        }

        public int? GetNullableDictionaryIdByCode(string entityName, string code)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryId = dictionaries.Where(d => d.Code == code && d.IsDeleted == false).Select(d => d.Id).FirstOrDefault();
            if (dictionaryId == default(int))
            {
                return null;
            }
            return dictionaryId;
        }

        public string GetDictionaryCodeByExternalId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryCode = dictionaries.Where(d => d.ExternalId == id).Select(d => d.Code).FirstOrDefault();
            if (string.IsNullOrEmpty(dictionaryCode))
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, id);
            }

            return dictionaryCode;
        }

        public IDictionaryEntity<int> GetDictionaryEntityByExternalId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryEntity = dictionaries.Where(d => d.ExternalId == id).FirstOrDefault() ?? dictionaries.Where(d => d.Id == id).FirstOrDefault();
            if (dictionaryEntity == null)
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, id);
            }

            return dictionaryEntity;
        }

        public string GetDictionaryCodeById(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionaryCode = dictionaries.Where(d => d.Id == id).Select(d => d.Code).FirstOrDefault();
            if (string.IsNullOrEmpty(dictionaryCode))
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, id);
            }

            return dictionaryCode;
        }

        public dynamic GetActualDictionaryByCode(string entityName, string code)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionary = dictionaries.FirstOrDefault(d => d.Code == code && d.IsDeleted == false);

            return dictionary;
        }

        public dynamic GetDictionaryById(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }
            var dictionary = dictionaries.FirstOrDefault(d => d.Id == id);
            
            if (dictionary == null)
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, id);
            }

            return dictionary;
        }

        public dynamic GetDictionaryByExternalId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }

            var dictionary = dictionaries.FirstOrDefault(d => d.ExternalId == id && !d.IsDeleted) ?? dictionaries.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
            if (dictionary == null)
            {
                throw new DataNotFoundException(entityName,
                    DataNotFoundException.OperationType.Read, id);
            }

            return dictionary;
        }

        public int GetDictionaryIdByExternalId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }

            var dictionary = dictionaries.FirstOrDefault(d => d.ExternalId == id && !d.IsDeleted) ?? dictionaries.FirstOrDefault(d => d.Id == id && !d.IsDeleted);

            return dictionary.Id;
        }

        public int? GetNullableDictionaryIdByExternalId(string entityName, int id)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _niisContext.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            if (dictionaries == null)
            {
                throw new ArgumentException($"Not found {entityName}");
            }

            var dictionary = dictionaries.FirstOrDefault(d => d.ExternalId == id && !d.IsDeleted) ?? dictionaries.FirstOrDefault(d => d.Id == id && !d.IsDeleted);

            return dictionary?.Id;
        }

        private void SetChildClass(List<DicDocumentClassification> classifications, int[] parentsId,
            List<int> resultChildClass)
        {
            foreach (var item in parentsId)
            {
                var result = classifications
                    .Where(x => x.ParentId == item)
                    .Select(x => x.Id)
                    .ToArray();
                if (result.Length != 0)
                {
                    resultChildClass.AddRange(result);
                    SetChildClass(classifications, result, resultChildClass);
                }
            }
        }
    }
}
