namespace Iserv.Niis.Domain.Abstract
{
    public interface IReference
    {
        int Id { get; set; }
        string Code { get; set; }
        string NameRu { get; set; }
        string NameKz { get; set; }
        string NameEn { get; set; }
    }
}