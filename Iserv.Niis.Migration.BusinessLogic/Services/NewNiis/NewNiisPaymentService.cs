using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisPaymentService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisPaymentService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangePayments(IEnumerable<object> payments)
        {
            _context.AddRange(payments);
            _context.SaveChanges();
        }

        public int? GetLastBarcode(Type paymentType)
        {
            var payments = _context.Set(paymentType) as IQueryable<Entity<int>>;
            return payments
                .AsNoTracking()
                .OrderByDescending(d => d.Id)
                .FirstOrDefault()?.Id;
        }

        public int GetPaymentCount(Type paymentType)
        {
            var payments = _context.Set(paymentType) as IQueryable<Entity<int>>;
            return payments.AsNoTracking().Count();
        }

        public void CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }
    }
}
