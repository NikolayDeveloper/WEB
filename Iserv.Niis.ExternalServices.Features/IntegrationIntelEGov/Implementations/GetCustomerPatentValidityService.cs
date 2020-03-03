using System;
using System.Linq;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    /// <summary>
    /// Сервис получения дпатента по номеру и типу
    /// </summary>
    public class GetCustomerPatentValidityService : IGetCustomerPatentValidityService
    {
        private readonly NiisWebContext _niisWebContext;
        private readonly DictionaryHelper _dictionaryHelper;

        public GetCustomerPatentValidityService(NiisWebContext niisWebContext, DictionaryHelper dictionaryHelper)
        {
            _niisWebContext = niisWebContext;
            _dictionaryHelper = dictionaryHelper;
        }

        /// <summary>
        /// Метод получения патента
        /// </summary>
        /// <param name="gosNumber">Гос. Номер</param>
        /// <param name="patentType">Тип патента</param>
        /// <param name="responce">Данные для ответа</param>
        /// <returns></returns>
        public CustomerPatentValidityResponce Get(string gosNumber, ReferenceInfo patentType, CustomerPatentValidityResponce responce)
        {
            //Полуаем типа патента
            if (!(_dictionaryHelper.GetDictionaryByExternalId(nameof(DicProtectionDocType), patentType.Id) is DicProtectionDocType type))
                throw new NotSupportedException($"Тип с идентефикатором {patentType.Id} не найден");

            //Достаем инфрмация о патенте
            var patent = _niisWebContext
                .ProtectionDocs
                .Where(d => d.GosNumber == gosNumber && d.Request.ProtectionDocTypeId == type.Id) //Тип берется из заявки из за конфликта справочников и несоответсвия Id
                .Select(d => new
                {
                    d.Barcode,
                    d.NameRu,
                    d.NameEn,
                    d.NameKz,
                    d.ValidDate,
                    RequestBarcode = d.Request.Barcode,
                    Owners = d.ProtectionDocCustomers.Select(c => new PatentOwner
                    {
                        Xin = c.Customer.Xin ?? string.Empty,
                        AddressEn = c.AddressEn ?? c.Customer.AddressEn ?? string.Empty,
                        AddressKz = c.AddressKz ?? c.Customer.AddressKz ?? string.Empty,
                        AddressRu = c.Address ?? c.Customer.Address ?? string.Empty,
                        AddressPostCode = string.Empty,
                        OwnerNameEn = c.Customer.NameEn ?? string.Empty,
                        OwnerNameKz = c.Customer.NameKz ?? string.Empty,
                        OwnerNameRu = c.Customer.NameRu ?? string.Empty,
                        CustomerType = c.CustomerRole.ExternalId ?? d.Id,
                        Location = new ReferenceInfo
                        {
                            Id = c.Customer.Country != null ? c.Customer.Country.ExternalId ?? c.Customer.Country.Id : int.MinValue,
                            Note = c.Customer.Country != null ? c.Customer.Country.NameRu : string.Empty
                        }
                    })
                })
                .FirstOrDefault();

            if (patent == null)
                throw new NotSupportedException($"Патент с номером {gosNumber} и типом {type.NameRu}({patentType.Id}) не найден");

            responce.PatentId = patent.Barcode;
            responce.DocumentId = patent.RequestBarcode.ToString();
            responce.PatentNameRu = patent.NameRu ?? string.Empty;
            responce.PatentNameEn = patent.NameEn ?? string.Empty;
            responce.PatentNameKz = patent.NameKz ?? string.Empty;

            if (patent.ValidDate.HasValue)
                responce.ValidityDate = patent.ValidDate.Value.DateTime;

            responce.Owners = patent.Owners.ToArray();

            return responce;
        }
    }
}