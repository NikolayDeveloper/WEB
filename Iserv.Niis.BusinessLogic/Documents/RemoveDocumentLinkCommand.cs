using AutoMapper;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class RemoveDocumentLinkCommand : BaseCommand
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);

        public async Task ExecuteAsync(DocumentLinkDto dto)
        {
            var repo = Uow.GetRepository<DocumentLink>();
            var link = await repo.GetByIdAsync(dto.Id);
            repo.Delete(link);
            await Uow.SaveChangesAsync();
        }
    }
}
