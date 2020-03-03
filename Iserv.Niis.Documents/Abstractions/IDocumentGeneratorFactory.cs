namespace Iserv.Niis.Documents.Abstractions
{
    public interface IDocumentGeneratorFactory
    {
        IDocumentGenerator Create(string templateCode);
    }
}