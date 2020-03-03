using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Bulletin
{
    public class EditBulletinSectionRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}