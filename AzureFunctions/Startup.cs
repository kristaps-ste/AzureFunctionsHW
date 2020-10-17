using System;
using AzureFunctions.Exceptions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using AzureFunctions;
using AzureFunctions.Functions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Interfaces;
using StorageService.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public  override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            string containerName = Environment.GetEnvironmentVariable("BlobContainerName");
            string tableName = Environment.GetEnvironmentVariable("TableName");
            string dateFormat = Environment.GetEnvironmentVariable("RequestDateFormat");
            var container = new List<string>
            {
                connectionString,
                containerName,
                tableName,
                dateFormat
            };
            ThrowExceptionIfEmptyOrNull(container);
            builder.Services.AddScoped<IAzureTableStorage,AzureTableStorage>();
            builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorage>();

            //blob container client init for blob operations
            builder.Services.AddScoped((s) => new BlobContainerClient(connectionString, containerName));

            //table client  init for azure table operations
            builder.Services.AddScoped((s) =>
            {
                CloudStorageAccount storageAccount;
                CloudStorageAccount.TryParse(connectionString, out storageAccount);
                CloudTableClient tableClient =
                    storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                
                CloudTable table=tableClient.GetTableReference(tableName);
                table.CreateIfNotExists();
              return table;
            });
            
        }

        private void ThrowExceptionIfEmptyOrNull(IList<string> variables)
        {
            if (variables.Any(String.IsNullOrEmpty))
            {
                throw new InvalidConfigurationException();
            }
        }
    }
}



        
