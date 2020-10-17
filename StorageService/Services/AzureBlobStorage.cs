using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using StorageService.Interfaces;

namespace StorageService.Services
{
    public class AzureBlobStorage : IAzureBlobStorageService
    {
       
        private readonly BlobContainerClient _client;
        public AzureBlobStorage(BlobContainerClient client )
        {
            _client = client;
        }

        public async Task UploadJsonStreamAsync(string content, string fileId)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            await _client.UploadBlobAsync(fileId, stream);
        }

        public async Task<string> GetBlobAsync(string id)
        {
           var blobClient = _client.GetBlobClient(id);
            try
            {
                var dwnInfo = await blobClient.DownloadAsync();
                using var reader = new StreamReader(dwnInfo.Value.Content);
                return await reader.ReadToEndAsync();
            }
            catch
            {
                // ignored
            }
            return "";
        }
    }
}
