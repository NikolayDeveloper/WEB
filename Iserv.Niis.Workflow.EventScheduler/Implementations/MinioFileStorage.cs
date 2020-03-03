using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iserv.Niis.FileStorage.Abstract;
using Minio;
using RestSharp.Extensions;

namespace Iserv.Niis.Workflow.EventScheduler.Implementations
{
    public class MinioFileStorage : IFileStorage
    {
        private MinioClient _client;

        public MinioFileStorage()
        {
            InitializeMinioFromConfig();
        }
        public async Task AddAsync(string bucketName, string objectName, byte[] bytes, string contentType)
        {
            await MakeBucketIfNotExists(bucketName);

            using (var stream = new MemoryStream(bytes))
            {
                await AddAsync(bucketName, objectName, stream, contentType);
            }
        }

        public async Task AddAsync(string bucketName, string objectName, Stream stream, string contentType)
        {
            stream.Seek(0, SeekOrigin.Begin);

            await MakeBucketIfNotExists(bucketName);
            await _client.PutObjectAsync(bucketName, objectName, stream, stream.Length, contentType);
        }

        public async Task<byte[]> GetAsync(string bucketName, string objectName)
        {
            byte[] result = null;

            await _client.GetObjectAsync(bucketName, objectName,
                stream =>
                {
                    result = stream.ReadAsBytes();
                });

            return result;
        }

        public async Task Remove(string bucketName, string objectName, bool removeBucketIfEmpty = true)
        {
            await _client.RemoveObjectAsync(bucketName, objectName);

            if (removeBucketIfEmpty && !await _client.ListObjectsAsync(bucketName).Any())
            {
                await _client.RemoveBucketAsync(bucketName);
            }
        }

        private void InitializeMinioFromConfig()
        {
            var endpoint = Properties.Settings.Default.MinioEndpoint;
            var withSsl = Properties.Settings.Default.MinioWithSsl;

            var accessKey = Properties.Settings.Default.MinioAccessKey;
            var secretKey = Properties.Settings.Default.MinioSecretKey;

            _client = withSsl
                ? new MinioClient(endpoint, accessKey, secretKey).WithSSL()
                : new MinioClient(endpoint, accessKey, secretKey);
        }
        private async Task MakeBucketIfNotExists(string bucketName)
        {
            if (!await _client.BucketExistsAsync(bucketName))
                await _client.MakeBucketAsync(bucketName);
        }
    }
}
