using System.Collections.Generic;
using System.Threading.Tasks;
using AzureFunctions.Functions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace AzureFunctionsTests
{
   public class GetPayloadByIdFunctionTests
   {
       private GetPayloadByIdFunction _testFunction;
      
        public GetPayloadByIdFunctionTests()
        {
            _testFunction =new GetPayloadByIdFunction(GenerateMocks.AzureBlobStorage("testContent"));
        }

        [Theory]
        [InlineData("12345675", 200)]
        [InlineData("12345675a", 404)]
        [InlineData("a234$%#5a", 404)]
        [InlineData("", 404)]

        public async Task GetLogsFunction_ShouldPassWithThisData(string id, int expectedCode )
        {
            var query= new Dictionary<string,StringValues>();
            query.TryAdd("id",id );
            var request = GenerateMocks.HttpRequest(query);

            var response = await  _testFunction.Run(request);

            var resultCode = response as IStatusCodeActionResult;
            
            int actual = resultCode?.StatusCode??0;

             Assert.Equal(expectedCode,actual);
        }
    }
}
