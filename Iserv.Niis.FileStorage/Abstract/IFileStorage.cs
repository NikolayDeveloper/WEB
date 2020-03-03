using System.IO;
using System.Threading.Tasks;

namespace Iserv.Niis.FileStorage.Abstract
{
    public interface IFileStorage
    {
        Task AddAsync(string bucketName, string objectName, byte[] bytes, string contentType);
        Task AddAsync(string bucketName, string objectName, Stream stream, string contentType);
        Task<byte[]> GetAsync(string bucketName, string objectName);
        Task Remove(string bucketName, string objectName, bool removeBucketIfEmpty = true);
    }
}
