using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class CreateContractCommand : BaseCommand
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public CreateContractCommand(IMapper mapper, IExecutor executor)
        {
            _mapper = mapper;
            _executor = executor;
        }

        public async Task<int> ExecuteAsync(ContractDetailDto contractDetailDto)
        {
            var contract = _mapper.Map<ContractDetailDto, Contract>(contractDetailDto);
            return await ExecuteAsync(contract);
        }

        public async Task<int> ExecuteAsync(Contract contract)
        {
            contract.RegDate = NiisAmbientContext.Current.DateTimeProvider.Now;
            contract.IncomingDate = NiisAmbientContext.Current.DateTimeProvider.Now;

            _executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(contract));
            _executor.GetHandler<GenerateContractIncomingNumberHandler>().Process<int>(h => h.Execute(contract));

            var contractRepository = Uow.GetRepository<Contract>();
            contractRepository.Create(contract);
            await Uow.SaveChangesAsync();
            return contract.Id;
        }

        public async Task<int> ExecuteFullObjAsync(Contract contract)
        {
            var contractRepository = Uow.GetRepository<Contract>();
            contractRepository.Create(contract);
            await Uow.SaveChangesAsync();
            return contract.Id;
        }
    }
}