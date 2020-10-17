using System.Threading.Tasks;
using AzureFunctions.Functions;
using Xunit;

namespace AzureFunctionsTests
{
    public class ScheduledRequestFunctionTest
    {
        private readonly ScheduledRequestFunction _testInstance;
      public   ScheduledRequestFunctionTest()
        {
            _testInstance =new ScheduledRequestFunction(GenerateMocks.AzureTableStorage(),GenerateMocks.AzureBlobStorage("test"));
        }

        [Theory]
        [InlineData("https://api.publicapis.org/")]
        [InlineData(null)]
        [InlineData("invalid-url")]
        [InlineData("")]
        public async Task ScheduledRequestFunction_ShouldNotThrowExceptionWithDifferentUrl(string url)
        {
            _testInstance.ApiUrl =url ;
            var exception = await Record.ExceptionAsync(() => _testInstance.Run(null)); 
           Assert.Null(exception);
        }
    }
}
