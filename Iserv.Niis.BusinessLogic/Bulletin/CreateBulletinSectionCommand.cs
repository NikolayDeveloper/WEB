using Iserv.Niis.Domain.Entities.Bulletin;
using Iserv.Niis.Model.Models.Bulletin;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class CreateBulletinSectionCommand : BaseCommand
    {
        public async Task Execute(CreateBulletinSectionRequestDto requestDto)
        {
            var bulletinSectionRepository = Uow.GetRepository<BulletinSection>();

            var bulletinSection = new BulletinSection
            {
                Name = requestDto.Name,
                DateCreate = DateTimeOffset.Now
            };

            await bulletinSectionRepository.CreateAsync(bulletinSection);
            await Uow.SaveChangesAsync();
        }
    }
}