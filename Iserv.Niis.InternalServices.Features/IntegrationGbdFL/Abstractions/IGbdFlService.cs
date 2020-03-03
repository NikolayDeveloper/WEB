using Iserv.Niis.Domain.Entities.AccountingData;

namespace Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Abstractions
{
    public interface IGbdFlService
    {
        DicCustomer GetCustomer(string iin);
    }
}