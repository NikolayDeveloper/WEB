using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class RegisterContractCommand : BaseCommand
    {
        private readonly IMapper _mapper;
        private readonly INumberGenerator _numberGenerator;

        public RegisterContractCommand(IMapper mapper, INumberGenerator numberGenerator)
        {
            _mapper = mapper;
            _numberGenerator = numberGenerator;
        }

        public async Task<int> ExecuteAsync(ContractDetailDto contractDetailDto)
        {
            var contractRepository = Uow.GetRepository<Contract>();

            var contract = await contractRepository
                .AsQueryable()
                .Include(r => r.ProtectionDocType)
                .Include(r => r.Type)
                .Include(r => r.Category)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(cr => cr.Request).ThenInclude(cr => cr.ProtectionDocType)
                .FirstOrDefaultAsync(r => r.Id == contractDetailDto.Id);

            if (contract == null)
            {
                throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Update, contractDetailDto.Id);
            }

            _mapper.Map(contractDetailDto, contract);
            _numberGenerator.GenerateGosNumber(contract);

            contractRepository.Update(contract);
            Uow.SaveChanges();

            return contract.Id;
        }
    }
}
