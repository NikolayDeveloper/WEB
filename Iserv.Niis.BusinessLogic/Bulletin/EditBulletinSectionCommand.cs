using Iserv.Niis.Domain.Entities.Bulletin;
using Iserv.Niis.Model.Models.Bulletin;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class EditBulletinSectionCommand : BaseCommand
    {
        public async Task Execute(int bulletinSectionId, EditBulletinSectionRequestDto requestDto)
        {
            var bulletinSectionRepository = Uow.GetRepository<BulletinSection>();
            var bulletinSection = bulletinSectionRepository.GetById(bulletinSectionId);

            bulletinSection.Name = requestDto.Name;
            bulletinSection.DateUpdate = DateTimeOffset.Now;

            bulletinSectionRepository.Update(bulletinSection);

            await Uow.SaveChangesAsync();
        }
    }
}