using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Bulletin
{
    public class CreateBulletinSectionRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}