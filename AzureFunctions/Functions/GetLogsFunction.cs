using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using StorageService.Interfaces;
using StorageService.Models;

namespace AzureFunctions.Functions
{
    public class GetLogsFunction
    {
        private readonly IAzureTableStorage _azureTable;
        public string RequestDateFormat { get; set; }
        public GetLogsFunction(IAzureTableStorage storage)
        {
            _azureTable = storage;
            RequestDateFormat =  Environment.GetEnvironmentVariable("RequestDateFormat");
        }
        [FunctionName("getLogs")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string from = req.Query["from"];
            string to = req.Query["to"];
            
            CultureInfo provider = CultureInfo.InvariantCulture;
            
            var isParsedFrom = DateTime
                .TryParseExact(from, RequestDateFormat, provider, DateTimeStyles.AdjustToUniversal, out DateTime fromDt);
            var isParsedTo = DateTime
                .TryParseExact(to, RequestDateFormat, provider, DateTimeStyles.AdjustToUniversal, out DateTime toDt);

            if (!IsValidInputData(isParsedFrom,isParsedTo,fromDt,toDt, RequestDateFormat))
            {
                return new NotFoundResult();
            }
            
            var dataFromStorage = await _azureTable
                .GetRecordsWithinIntervalAsync(fromDt, toDt);
            List<GetLogResponseModel> mapped= dataFromStorage.Select(MapToOutput).ToList();
          
            var resultsContainer= new ContainerModel<GetLogResponseModel>();
           resultsContainer.Items = mapped;

            return new  OkObjectResult(resultsContainer);
        }

        private bool IsValidInputData(bool parsedFrom, bool parsedTo, DateTime from, DateTime to,string requestDateFormat )
        {
            if (string.IsNullOrEmpty(requestDateFormat))
            {
                return false;

            }
            return parsedFrom && parsedTo && from < to;
        }
        private GetLogResponseModel MapToOutput(LogEntity it)
        {
            var entity = new GetLogResponseModel();
            entity.RowKey = long.Parse(it.RowKey);
            entity.Success = it.Success;
            return entity;
        }
    }
}