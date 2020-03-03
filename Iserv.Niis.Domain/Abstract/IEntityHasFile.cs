namespace Iserv.Niis.Domain.Abstract
{
    public interface IEntityHasFile<TKey>
    {
        string FileName { get; set; }
        long FileSize { get; set; }
        byte[] File { get; set; }
        string FileType { get; set; }
        string FileFingerPrint { get; set; }
    }
}