using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.ProtectionDocCustomers;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GenerateGosNumbersForMultipleProtectionDocsHandler: BaseHandler
    {
        private const int PatentOwner = 3;

        public async Task ExecuteAsync(int[] ids)
        {
            var protectionDocs = new List<ProtectionDoc>();
            foreach (var id in ids)
            {
                protectionDocs.Add(await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id)));
            }

            var groupedPds = protectionDocs
                .GroupBy(pd => pd.Type.Code)
                .Select(pd => pd);

            var ipcPds = groupedPds.Where(g =>
                    new[]
                    {
                        DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                        DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
                    }.Contains(g.Key)).SelectMany(g => g)
                .OrderBy(protectionDoc =>
                    protectionDoc?.IpcProtectionDocs?.FirstOrDefault(ipc => ipc.IsMain)?.Ipc?.Code ??
                    protectionDoc?.RegNumber ?? string.Empty);

            var regNumberPds = groupedPds.Where(g =>
                    !new[]
                    {
                        DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                        DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
                    }.Contains(g.Key)).SelectMany(g => g)
                .OrderBy(r => r.RegNumber);

            foreach (var protectionDoc in regNumberPds)
            {
                protectionDoc.GosNumber = GenerateGosNumber(NumberGenerator.ProtectionDocNumberCodePrefix + NumberGenerator.ProtectionDocGosNumberGenerationByRegNumberCode);

                var validDate = GetValidDate(protectionDoc);
                protectionDoc.ValidDate = validDate;

                protectionDoc.GosDate = DateTimeOffset.Now;
                protectionDoc.ExtensionDate = GetExtensionDate(protectionDoc);

                await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));

                await AddProtectionDocCustomers(protectionDoc.Id);
                UpdatePaymentInvoicesStatus(protectionDoc.Id);
            }
            foreach (var protectionDoc in ipcPds)
            {
                protectionDoc.GosNumber = GenerateGosNumber(NumberGenerator.ProtectionDocNumberCodePrefix + NumberGenerator.ProtectionDocGosNumberGenerationByIpcCode);

                var validDate = GetValidDate(protectionDoc);
                protectionDoc.ValidDate = validDate;

                protectionDoc.GosDate = DateTimeOffset.Now;
                protectionDoc.ExtensionDate = GetExtensionDate(protectionDoc);

                await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));

                await AddProtectionDocCustomers(protectionDoc.Id);
                UpdatePaymentInvoicesStatus(protectionDoc.Id);
            }
        }

        private async Task AddProtectionDocCustomers(int id)
        {
            var protectionDocCustomers =  await Executor.GetQuery<GetProtectionDocCustomersByProtectionDocIdQuery>().Process(d => d.ExecuteAsync(id));
            protectionDocCustomers = protectionDocCustomers.Where(d => d.CustomerRole.Code == DicCustomerRoleCodes.Declarant).ToList();

            foreach(var protectionDocCustomer in protectionDocCustomers)
            {
                protectionDocCustomer.Id = 0;
                protectionDocCustomer.Customer = null;
                protectionDocCustomer.CustomerRole = null;
                protectionDocCustomer.ProtectionDoc = null;
                protectionDocCustomer.CustomerRoleId = PatentOwner;
            }

            await Executor.GetCommand<AddProtectionDocCustomersCommand>().Process(c => c.ExecuteAsync(id, protectionDocCustomers));
        }

        private void UpdatePaymentInvoicesStatus(int id)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByProtectionDocIdQuery>().Process(c => c.Execute(id));
            var dreditedPaymentInvoices = paymentInvoices.Where(d => d.Status.Code == DicPaymentStatusCodes.Credited && (d.Tariff.Code == DicTariffCodes.TrademarNmptRegistrationAndPublishing || d.Tariff.Code == DicTariffCodes.TimeRestore)).ToList();

            var chargedStatus = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>().Process(q => q.Execute(DicPaymentStatusCodes.Charged));
            var systemUser = Executor.GetQuery<GetUserByXinQuery>().Process(q => q.Execute(UserConstants.SystemUserXin));

            foreach (var dreditedPaymentInvoice in dreditedPaymentInvoices)
            {
                dreditedPaymentInvoice.Status = chargedStatus;
                dreditedPaymentInvoice.StatusId = chargedStatus.Id;
                dreditedPaymentInvoice.DateComplete = DateTimeOffset.Now;
                dreditedPaymentInvoice.WriteOffUserId = systemUser?.Id;
                Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(d => d.Execute(dreditedPaymentInvoice));
            }
        }
        private DateTimeOffset GetExtensionDate(ProtectionDoc protectionDoc)
        {
            return protectionDoc.Bulletins != null && protectionDoc.Bulletins.Count > 0 ?
                    protectionDoc.Bulletins.First().Bulletin.DateCreate.AddMonths(6)
                    : DateTimeOffset.Now.DayOfWeek == DayOfWeek.Friday ?
                        DateTimeOffset.Now.AddMonths(6)
                        : (int)DateTimeOffset.Now.DayOfWeek < 5 && (int)DateTimeOffset.Now.DayOfWeek > 0 ?
                           DateTimeOffset.Now.AddDays(((int)DateTimeOffset.Now.DayOfWeek - 5) * -1).AddMonths(6)
                           : DateTimeOffset.Now.DayOfWeek == DayOfWeek.Saturday ?
                               DateTimeOffset.Now.AddDays(6).AddMonths(6)
                             : DateTimeOffset.Now.AddDays(5).AddMonths(6);
        }
        private DateTimeOffset GetValidDate(ProtectionDoc protectionDoc)
        {
            int addYears = 0;
            switch (protectionDoc.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                    addYears = 10;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                    switch (protectionDoc.SelectionAchieveType.Code)
                    {
                        case DicSelectionAchieveTypeCodes.Agricultural:
                            addYears = 25;
                            break;
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            addYears = 30;
                            break;
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            addYears = 35;
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                    addYears = 5;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                    addYears = 20;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                    addYears = 15;
                    break;
            }

            return protectionDoc.DateCreate.AddYears(addYears);
        }

        private string GenerateGosNumber(string code)
        {
            return $"{Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(code))}";
        }
    }
}
