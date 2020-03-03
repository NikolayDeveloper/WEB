using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Bulletin
{
    public class BulletinSection : Entity<int>
    {
        public string Name { get; set; }
    }
}