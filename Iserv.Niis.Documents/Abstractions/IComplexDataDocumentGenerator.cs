namespace Iserv.Niis.Documents.Abstractions
{
    /// <summary>
    /// Используется для шаблонов  в которых требуется вставка сложных структур (таблица, список, прочее).
    /// Основное назначение для того, чтобы потом не искать шаблоны.
    /// </summary>
    public interface IComplexDataDocumentGenerator : IDocumentGenerator
    {
    }
}
