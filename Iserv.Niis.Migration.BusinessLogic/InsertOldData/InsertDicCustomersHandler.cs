using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertDicCustomersHandler : BaseHandler
    {
        private readonly OldNiisDictionaryService _oldNiisDictionaryService;
        private readonly NewNiisDictionaryService _newNiisDictionaryService;
        private readonly AppConfiguration _appConfiguration;

        public InsertDicCustomersHandler(
            NiisWebContextMigration context,
            OldNiisDictionaryService oldNiisDictionaryService,
            NewNiisDictionaryService newNiisDictionaryService,
            AppConfiguration appConfiguration) : base(context)
        {
            _oldNiisDictionaryService = oldNiisDictionaryService;
            _newNiisDictionaryService = newNiisDictionaryService;
            _appConfiguration = appConfiguration;
        }
        public void Execute()
        {
            int packageIndex = 1;
            int allCustomers = _oldNiisDictionaryService.GetDicCustomersCount();
            int customersCommited = _newNiisDictionaryService.GetDicCustomersCount();
            int customersCount = allCustomers - customersCommited;
            bool isStop = false;

            var stopwatch = Stopwatch.StartNew();
            while (!isStop)
            {
                ActionTransaction(() =>
                {
                    var lastId = _newNiisDictionaryService.GetLastDicCustomerId();
                    var (customers, customerAttorneyInfos) = _oldNiisDictionaryService.GetDicCustomersAndCustomerAttorneyInfos(_appConfiguration.BigPackageSize, lastId ?? 0);

                    if (!customers.Any())
                    {
                        isStop = true;
                        return;
                    }

                    _newNiisDictionaryService.CreateRangeDictionaries(customers);
                    _newNiisDictionaryService.CreateRangeCustomerAttorneyInfos(customerAttorneyInfos);

                    Console.Write($"\rCustomers commited - {packageIndex * _appConfiguration.BigPackageSize}/{customersCount}. Time elapsed: {stopwatch.Elapsed}");
                    packageIndex++;
                });
            }
        }
    }
}
