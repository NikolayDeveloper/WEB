using AutoMapper;
using Iserv.Niis.Model.Models.Bulletin;

namespace Iserv.Niis.Model.Mappings.Bulletin
{
    public class BulletinDtoProfile: Profile
    {
        public BulletinDtoProfile()
        {
            CreateMap<Domain.Entities.Bulletin.Bulletin, BulletinDto>();
            CreateMap<BulletinDto, Domain.Entities.Bulletin.Bulletin>();
        }
    }
}
