using AutoMapper;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class SaveDocumentLinkCommand : BaseCommand
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);

        public async Task<DocumentLink> ExecuteAsync(DocumentLinkDto dto)
        {
            var repo = Uow.GetRepository<DocumentLink>();
            var link = Mapper.Map<DocumentLink>(dto);
            await repo.CreateAsync(link);
            await Uow.SaveChangesAsync();
            return link;
        }
    }
}
