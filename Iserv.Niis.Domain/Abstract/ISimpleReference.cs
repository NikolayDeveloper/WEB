namespace Iserv.Niis.Domain.Abstract
{
    public interface ISimpleReference
    {
        int Id { get; set; }
        string NameRu { get; set; }
        string NameKz { get; set; }
        string NameEn { get; set; }
    }
}