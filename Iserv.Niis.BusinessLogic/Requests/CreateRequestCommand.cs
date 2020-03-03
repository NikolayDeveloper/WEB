using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class CreateRequestCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public CreateRequestCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(RequestDetailDto requestDetailDto)
        {
            var request = _mapper.Map<RequestDetailDto, Request>(requestDetailDto);

            var requestRepository = Uow.GetRepository<Request>();
            await requestRepository.CreateAsync(request);
            await Uow.SaveChangesAsync();
            return request.Id;
        }

        public async Task<int> ExecuteAsync(Request request)
        {
            var requestRepository = Uow.GetRepository<Request>();
            await requestRepository.CreateAsync(request);
            await Uow.SaveChangesAsync();
            return request.Id;
        }
    }
}
