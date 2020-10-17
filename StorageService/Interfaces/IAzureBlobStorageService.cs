using System.Threading.Tasks;

namespace StorageService.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<string> GetBlobAsync(string id);
        Task UploadJsonStreamAsync(string content, string fileId);
    }
}