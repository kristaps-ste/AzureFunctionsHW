using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StorageService.Interfaces;
using StorageService.Models;

namespace StorageService.Services
{
    public class AzureTableStorage : IAzureTableStorage
    {
        private readonly CloudTable _table;
      
        public AzureTableStorage(CloudTable table)
        {

            _table = table;
        }

        public async Task SaveLogAsync(LogEntity log)
        {
            TableOperation insertOperation = TableOperation.Insert(log);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task<List<LogEntity>> GetRecordsWithinIntervalAsync(DateTime from, DateTime to)
        {
            var fromStr = from.Ticks.ToString();
            var toStr = to.Ticks.ToString();
            var filterKey = "RowKey";
            TableQuery<LogEntity> rangeQuery = new TableQuery<LogEntity>().Where(

                      TableQuery.CombineFilters(
                          TableQuery.GenerateFilterCondition(filterKey, QueryComparisons.GreaterThanOrEqual, fromStr),
                          TableOperators.And,
                          TableQuery.GenerateFilterCondition(filterKey, QueryComparisons.LessThanOrEqual, toStr)));
            TableContinuationToken continuationToken = null;
            List<LogEntity> toReturn = new List<LogEntity>();
            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(rangeQuery, continuationToken);
                continuationToken = result.ContinuationToken;

                if (result.Results != null)
                {
                    toReturn.AddRange(result.Results);
                }
            } while (continuationToken != null);

            return toReturn;
        }

    }
}
