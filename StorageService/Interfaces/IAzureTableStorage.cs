using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StorageService.Models;

namespace StorageService.Interfaces
{
    public interface IAzureTableStorage
    {
        Task SaveLogAsync(LogEntity log);
        Task<List<LogEntity>> GetRecordsWithinIntervalAsync(DateTime from, DateTime to);
    }
}