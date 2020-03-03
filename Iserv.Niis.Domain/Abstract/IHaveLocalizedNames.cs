namespace Iserv.Niis.Domain.Abstract
{
    /// <summary>
    /// Используется для локализации
    /// </summary>
    public interface IHaveLocalizedNames
    {
        string NameRu { get; set; }
        string NameEn { get; set; }
        string NameKz { get; set; }
    }
}
