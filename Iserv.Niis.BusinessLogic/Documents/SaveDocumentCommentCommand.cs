using AutoMapper;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class SaveDocumentCommentCommand : BaseCommand
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);

        public async Task<DocumentComment> ExecuteAsync(DocumentCommentDto dto)
        {
            var repo = Uow.GetRepository<DocumentComment>();
            var commant = Mapper.Map<DocumentComment>(dto);
            await repo.CreateAsync(commant);
            await Uow.SaveChangesAsync();
            return commant;
        }
    }
}
