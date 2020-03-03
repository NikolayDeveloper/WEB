using Iserv.Niis.Migration.BusinessLogic.InsertOldData;
using System;

namespace Iserv.Niis.Migration.Payment
{
    public class MigratePayments
    {
        private readonly InsertDicCustomersHandler _insertDicCustomersHandler;
        private readonly InsertOnlyPaymentsHandler _insertOnlyPaymentsHandler;

        public MigratePayments(
            InsertDicCustomersHandler insertDicCustomersHandler,
            InsertOnlyPaymentsHandler insertOnlyPaymentsHandler)
        {
            _insertDicCustomersHandler = insertDicCustomersHandler;
            _insertOnlyPaymentsHandler = insertOnlyPaymentsHandler;
        }

        public void Migrate()
        {
            Console.WriteLine("Check customers");
            _insertDicCustomersHandler.Execute();
            Console.WriteLine("Migrate payments");
            _insertOnlyPaymentsHandler.Execute();
        }
    }
}
