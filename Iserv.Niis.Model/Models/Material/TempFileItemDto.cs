namespace Iserv.Niis.Model.Models.Material
{
    public class TempFileItemDto
    {
        public TempFileItemDto(string name, string tempName)
        {
            Name = name;
            TempName = tempName;
        }
        public string Name { get; set; }
        public string TempName { get; set; }
    }
}
