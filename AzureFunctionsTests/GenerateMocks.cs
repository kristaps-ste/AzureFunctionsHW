using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;
using StorageService.Interfaces;
using StorageService.Models;

namespace AzureFunctionsTests
{
   public static  class GenerateMocks
    {
       public static HttpRequest HttpRequest(Dictionary<String, StringValues> query)
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Flush();
            return reqMock.Object;
        }

       public static IAzureTableStorage AzureTableStorage()
       {
           using var mock = AutoMock.GetLoose();
           mock.Mock<IAzureTableStorage>()
               .Setup(it => it.GetRecordsWithinIntervalAsync(It.IsAny<DateTime>(),It.IsAny<DateTime>() ))
               .Returns(Task.FromResult(new List<LogEntity>()));
             
           return mock.Create<IAzureTableStorage>(); 
       }

       public static IAzureBlobStorageService AzureBlobStorage(string mockContent)
       {
           using var mock = AutoMock.GetLoose();
           mock.Mock<IAzureBlobStorageService>()
               .Setup(it => it.GetBlobAsync(It.IsAny<string>() ))
               .Returns(Task.FromResult(mockContent));
             
           return mock.Create<IAzureBlobStorageService>();     
       }
    }
}
