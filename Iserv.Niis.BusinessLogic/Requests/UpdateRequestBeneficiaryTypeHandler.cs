using Iserv.Niis.BusinessLogic.RequestCustomers;
using Iserv.Niis.Model.Models.Subject;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class UpdateRequestBeneficiaryTypeHandler : BaseHandler
    {
        public async Task HandleAsync(SubjectDto subjectDto)
        {
            var requestCustomes = await Executor.GetQuery<GetRequestCustomersByRequestIdQuery>().Process(q => q.ExecuteAsync(subjectDto.OwnerId.Value));
            if (requestCustomes == null) return;

            int? beneficiaryTypeId = subjectDto.BeneficiaryTypeId;

            if (beneficiaryTypeId != null)
            {
                foreach (var requestCustomer in requestCustomes)
                {
                    if (requestCustomer.Customer.BeneficiaryTypeId != beneficiaryTypeId)
                    {
                        beneficiaryTypeId = null;
                        break;
                    }
                }

                var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(subjectDto.OwnerId.Value));
                request.BeneficiaryTypeId = beneficiaryTypeId;
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
            }
        }
    }
}
