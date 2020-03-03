using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.Constants;

namespace Iserv.Niis.ExternalServices.Features.Utils
{
    public class IntegrationDictionaryHelper
    {
        private const string Refused = "Отказано";
        private const string Issued = "Выдано";

        private readonly NiisWebContext _niisContext;

        public IntegrationDictionaryHelper(NiisWebContext niisContext)
        {
            _niisContext = niisContext;
        }

        public int? GetDivisionId(int userId)
        {
            return _niisContext.Users
                .Where(x => x.Id == userId)
                .Select(x => (int?)x.Department.DivisionId)
                .FirstOrDefault();
        }

        public int GetOnlineStatus(int routeStageId)
        {
            return _niisContext.DicRouteStages
                       .Where(x => x.Id == routeStageId)
                       .Select(x => x.OnlineRequisitionStatusId)
                       .FirstOrDefault() ?? 0;
        }

        public int? GetRouteIdDocumentTypeId(int typeId)
        {
            return _niisContext.DicDocumentTypes
                .Where(x => x.Id == typeId)
                .Select(x => x.RouteId.GetValueOrDefault())
                .FirstOrDefault();
        }

        public int GetRouteIdByProtectionDocType(int protectionDocTypeId)
        {
            return _niisContext.DicProtectionDocTypes
                .Where(x => x.Id == protectionDocTypeId)
                .Select(x => x.RouteId)
                .FirstOrDefault();
        }

        public DicRouteStage GetRouteStage(int routeId)
        {
            return _niisContext.DicRouteStages
                .Where(x => x.RouteId == routeId && x.IsFirst)
                .OrderBy(x => x.Code)
                .FirstOrDefault();
        }

        public DicCustomer GetCustomerIdOrCreateNew(DicCustomer customer)
        {
            if (string.IsNullOrEmpty(customer.NameRu))
                throw new Exception("Имя контрагента не может быть пустым");

            var dicCustomer = _niisContext.DicCustomers
                .Where(x =>
                    !string.IsNullOrEmpty(customer.Xin) && x.Xin == customer.Xin && x.TypeId == customer.TypeId
                    || !string.IsNullOrEmpty(customer.Xin) && customer.Xin.Equals(x.Xin, StringComparison.CurrentCultureIgnoreCase)
                    || !string.IsNullOrEmpty(customer.NameRu) && customer.NameRu.Equals(x.NameRu, StringComparison.CurrentCultureIgnoreCase)
                )
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (dicCustomer != null)
            {
                dicCustomer.Login = customer.Login;
                dicCustomer.Phone = customer.Phone;
                dicCustomer.PhoneFax = customer.PhoneFax;
                dicCustomer.Address = customer.Address;
                dicCustomer.ShortAddress = customer.ShortAddress;
                dicCustomer.Email = customer.Email;
                dicCustomer.NameRu = customer.NameRu;

                _niisContext.DicCustomers.Update(dicCustomer);
                _niisContext.SaveChanges();

                return dicCustomer;
            }
               
            _niisContext.DicCustomers.Add(customer);
            _niisContext.SaveChanges();
            return customer;
        }

        public int? GetCustomerIdByRequestId(int requestId)
        {
            var customerId = _niisContext.Requests.Where(r => r.Id == requestId).Select(r => r.AddresseeId).FirstOrDefault();
            return customerId;
        }


        public string GetReceiveTypeCode(string sender)
        {
            if (CommonConstants.SenderPep.Equals(sender, StringComparison.CurrentCultureIgnoreCase))
                return DicReceiveTypeCodes.ElectronicFeedEgov;
            if (CommonConstants.SenderHand.Equals(sender, StringComparison.CurrentCultureIgnoreCase))
                return DicReceiveTypeCodes.Courier;
            return DicReceiveTypeCodes.ElectronicFeed;
        }
        public string GetResultStatus(string onlineStatusCode)
        {
            switch (onlineStatusCode)
            {
                case DicOnlineRequisitionStatus.Codes.RefusalReg:
                case DicOnlineRequisitionStatus.Codes.WithdrawnDiscontinued:
                    return Refused;
                case DicOnlineRequisitionStatus.Codes.ApplicationRegistered:
                case DicOnlineRequisitionStatus.Codes.Registered:
                    return Issued;
                default:
                    return string.Empty;
            }
        }
    }
}