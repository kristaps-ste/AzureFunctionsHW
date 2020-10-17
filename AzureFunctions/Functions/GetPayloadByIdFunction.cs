using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using StorageService.Interfaces;

namespace AzureFunctions.Functions
{
    public class GetPayloadByIdFunction
    {
        private readonly IAzureBlobStorageService _storageServiceService;
        public GetPayloadByIdFunction(IAzureBlobStorageService storageServiceService)
        {
            _storageServiceService = storageServiceService;
        }

        [FunctionName("getPayload")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string id = req.Query["id"];

            if (NotValidInput(id))
            {
                return SendFailure();
            }

            var dataFromStorage = await _storageServiceService.GetBlobAsync(id);
            if (dataFromStorage == String.Empty)
            {
                SendFailure();

            }
            return new OkObjectResult(dataFromStorage);
        }
        private NotFoundResult SendFailure()
        {
            return new NotFoundResult();
        }
        private bool NotValidInput(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return true;
            }
            return input.ToCharArray()
                .Any(c=>char.IsDigit(c)==false);
        }
    }
}



