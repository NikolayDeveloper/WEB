using System;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetPaySumService : IGetPaySumService
    {
        private readonly NiisWebContext _niisWebContext;

        public GetPaySumService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public void GetPaySum(
            GetPaySumResult result, 
            GetPaySumArgument argument, 
            int countForTariff, int? tariffId)
        {
            if (tariffId != null)
            {
                var paySum = GetTariff(
                                 (int)tariffId,
                                 !argument.ResidencyPayer.Equals(ResidencyPayer.NonresidentRK),
                                 argument.IsJur,
                                 argument.IsFiz,
                                 argument.IsFizBenefit) * countForTariff;

                if (paySum != null)
                {
                    if (argument.ExpiredPayment)
                    {
                        paySum = (decimal)1.2 * paySum;
                    }
                    result.PaySum = (decimal)paySum;
                }
            }
        }

        public int GetCountForTariff(
            int getPaySumArgumentCount, 
            IQueryable<IntegrationPaymentCalc> paymentCalcs)
        {
            int paymentCalcsCount = paymentCalcs.Count();
            switch (paymentCalcsCount)
            {
                case 1:
                    return getPaySumArgumentCount;
                case 2:
                    var minCount = paymentCalcs
                        .OrderByDescending(p => p.MinCount)
                        .FirstOrDefault()?
                        .MinCount;
                    var tempCount = getPaySumArgumentCount - minCount + 1;
                    if (tempCount > 0)
                        return (int)tempCount;
                    break;
            }
            return 1;
        }

        public IQueryable<IntegrationPaymentCalc> GetPaymentCalcs(
            int documentTypeId, 
            int mainDocumentTypeId, 
            bool minCountIsNull)
        {
            var result = _niisWebContext.IntegrationPaymentCalcs.Where(p =>
                    p.CorId == documentTypeId
                    && p.PatentType == mainDocumentTypeId
                    && (minCountIsNull ? p.MinCount == null : p.MinCount != null));
            return result;
        }

        #region PrivateMethod
        private Expression<Func<IntegrationNiisRefTariff, decimal?>> GetDiscountField(
            bool isResident, 
            bool isJur, 
            bool isFiz, 
            bool flIsFizBenefit)
        {
            var discountFieldName = "ValueFull";
            if (isResident)
            {
                if (isJur)
                {
                    discountFieldName = "ValueJur";
                }
                else if (flIsFizBenefit)
                {
                    discountFieldName = "ValueFizBenefit";
                }
                else if (isFiz)
                {
                    discountFieldName = "ValueFiz";
                }
            }

            ParameterExpression personExpression = Expression.Parameter(typeof(IntegrationNiisRefTariff), "p");
            Expression<Func<IntegrationNiisRefTariff, decimal?>> discountField =
                Expression.Lambda<Func<IntegrationNiisRefTariff, decimal?>>(Expression.PropertyOrField(personExpression, discountFieldName), personExpression);
            return discountField;
        }

        private decimal? GetTariff(
            int tariffId, 
            bool isResident, 
            bool isJur, 
            bool isFiz, 
            bool isFisBenefit)
        {
            var discountColumn = GetDiscountField(isResident, isJur, isFiz, isFisBenefit);
            return _niisWebContext.IntegrationNiisRefTariffs
                .Where(n => n.Id == tariffId)
                .Select(discountColumn)
                .FirstOrDefault();
        }

        #endregion
    }
}