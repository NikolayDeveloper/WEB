using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertOnlyPaymentsHandler : BaseHandler
    {
        private readonly OldNiisPaymentService _oldNiisPaymentService;
        private NewNiisPaymentService _newNiisPaymentService;
        private readonly AppConfiguration _appConfiguration;

        public InsertOnlyPaymentsHandler(
            NiisWebContextMigration context,
            OldNiisPaymentService oldNiisPaymentService,
            AppConfiguration appConfiguration) : base(context)
        {
            _oldNiisPaymentService = oldNiisPaymentService;
            _appConfiguration = appConfiguration;
        }

        public void Execute()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NiisWebContextMigration>();
            optionsBuilder.UseSqlServer(_appConfiguration.NiisConnectionString);

            int? lastBarcode;

            using (var context = new NiisWebContextMigration(optionsBuilder.Options))
            {
                _newNiisPaymentService = new NewNiisPaymentService(context);

                lastBarcode = _newNiisPaymentService.GetLastBarcode(typeof(Payment)) ?? 0;

                _newNiisPaymentService = null;
            }

            var oldPaymentsIds = _oldNiisPaymentService.GetPaymentIdsLargerLastBarcode(lastBarcode);
            foreach (var oldPaymentId in oldPaymentsIds)
            {
                using (var context = new NiisWebContextMigration(optionsBuilder.Options))
                {
                    _newNiisPaymentService = new NewNiisPaymentService(context);

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var payment = _oldNiisPaymentService.GetPayment(oldPaymentId);
                            if (payment == null)
                            {
                                continue;
                            }
                            _newNiisPaymentService.CreatePayment(payment);
                            _newNiisPaymentService = null;

                            transaction.Commit();

                            var paymentInfo = $"Платеж: ID - {payment.Id}, PaymentNumber - {payment.PaymentNumber ?? string.Empty}, Description: {payment.PurposeDescription ?? string.Empty}, Amount - {payment.Amount}";
                            Console.WriteLine(paymentInfo);
                            using (var writeText = new StreamWriter(_appConfiguration.FileLogPath, true))
                            {
                                writeText.WriteLine(paymentInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            using (var writeText = new StreamWriter(_appConfiguration.FileLogPath, true))
                            {

                                writeText.WriteLine($"{ex.Message ?? string.Empty}");
                            }

                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
