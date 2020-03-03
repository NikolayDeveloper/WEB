using AutoMapper;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ExpertSearch;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class CreateExpertSearchSimilarsCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public CreateExpertSearchSimilarsCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(ExpertSearchSimilarDto[] similarDtos)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();
            var expertSearchSimilars = _mapper.Map<ExpertSearchSimilar[]>(similarDtos);
            repo.CreateRange(expertSearchSimilars);
            Uow.SaveChanges();
        }
    }
}