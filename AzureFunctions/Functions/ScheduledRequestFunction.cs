using System;
using System.Threading.Tasks;
using AzureFunctions.Core;
using Microsoft.Azure.WebJobs;
using Refit;
using StorageService.Interfaces;
using StorageService.Models;

namespace AzureFunctions.Functions
{
    public  class ScheduledRequestFunction
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IAzureBlobStorageService _blobStorage;
        public string ApiUrl { get; set; }
        public ScheduledRequestFunction(IAzureTableStorage tableStorage,
                                    IAzureBlobStorageService blobStorage)
        {
            _tableStorage = tableStorage;
            _blobStorage = blobStorage;
            ApiUrl= Environment.GetEnvironmentVariable("ApiUrl");
        }

        [FunctionName("ScheduledRequest")]
        public async  Task Run([TimerTrigger("%ScheduledTriggerInterval%")]TimerInfo timer)
        { 
            DateTime requestTime= DateTime.UtcNow;
           
            LogEntity logEntity = new LogEntity
            {
                PartitionKey = CreatePartitionKey(requestTime), 
                RowKey = requestTime.Ticks.ToString()
            };

            try
            {
                var randomApi = RestService.For<IRandomRequestApi>(ApiUrl);
                var payload = await randomApi.GetRandom();
                logEntity.Success = true;
                await LogSuccess(logEntity, payload);
            }
            catch 
            {
                logEntity.Success = false;
                await LogFailure(logEntity);
            }
        }

        private async   Task LogFailure(LogEntity logEntity)
        {
            await _tableStorage.SaveLogAsync(logEntity);
        }
        private async Task LogSuccess(LogEntity logEntity , string payload)
        {
            await _tableStorage.SaveLogAsync(logEntity);
            await _blobStorage.UploadJsonStreamAsync(payload,logEntity.RowKey);
        }
        private string CreatePartitionKey(DateTime logTime)
        {
            return  new DateTime(logTime.Year,logTime.Month,logTime.Day,logTime.Hour,0,0).Ticks.ToString();
        }
    }
}
