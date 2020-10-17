using System.Collections.Generic;
using System.Threading.Tasks;
using AzureFunctions.Functions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Xunit;


namespace AzureFunctionsTests
{
    public class GetLogsFunctionTests
    {
        private GetLogsFunction _testFunction;
        public GetLogsFunctionTests()
        {
            string dateFormat = "yyyy/MM/dd/HH:mm";
           
            _testFunction = new GetLogsFunction(GenerateMocks.AzureTableStorage());
            _testFunction.RequestDateFormat = dateFormat;

        }
        [Theory]
        [InlineData("2020/10/15/00:01","2020/10/16/00:01", 200)]
        [InlineData("1999/10/15/00:01","2020/10/16/00:01", 200)]
        [InlineData("2020/99/99/00:01","2020/10/16/00:01", 404)]
        [InlineData("2020/10/29/00:01","2020/10/16/00:01", 404)]
        [InlineData("2020/10/15/00:01","2020/10/1", 404)]
        [InlineData("202","2020/10/1", 404)]
        [InlineData("","", 404)]
        public async Task GetLogsFunction_ShouldPassWithThisInputData(string from, string to, int expected)
        {
            var query= new Dictionary<string,StringValues>();
            query.TryAdd("from",from );
            query.TryAdd("to", to);
            var request = GenerateMocks.HttpRequest(query);
            var res = await _testFunction.Run(request);
            var resultCode= (IStatusCodeActionResult)res;
            int actual = resultCode?.StatusCode??0;

            Assert.Equal(expected,actual);
        }
    }
}
