namespace Iserv.Niis.Domain.Abstract
{
    /// <summary>
    /// Сущность имеет логотип и содержит название из которго можно генерировать логотип
    /// </summary>
    public interface IHaveImageAttachment
    {
        string NameRu { get; set; }
        string NameKz { get; set; }
        string NameEn { get; set; }
        bool IsImageFromName { get; set; }
        byte[] Image { get; set; }
        byte[] PreviewImage { get; set; }
    }
}
