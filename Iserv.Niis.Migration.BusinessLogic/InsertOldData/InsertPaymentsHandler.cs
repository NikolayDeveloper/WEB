using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertPaymentsHandler : BaseHandler
    {
        private readonly OldNiisPaymentService _oldNiisPaymentService;

        public InsertPaymentsHandler(
            NiisWebContextMigration context,
            OldNiisPaymentService oldNiisPaymentService) : base(context)
        {
            _oldNiisPaymentService = oldNiisPaymentService;
        }

        public void Execute()
        {
            Console.WriteLine("start PaymentInvoice migrate");
            MigrateSQLPayments(_oldNiisPaymentService.GetPaymentInvoices);

            Console.WriteLine("start Payment migrate");
            MigrateSQLPayments(_oldNiisPaymentService.GetPayments);

            Console.WriteLine("start PaymentUse migrate");
            MigrateSQLPayments(_oldNiisPaymentService.GetPaymentUses);
        }

        #region Private Methods

        private void MigrateSQLPayments(Action<int> getOldData)
        {
            try
            {
                getOldData(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
