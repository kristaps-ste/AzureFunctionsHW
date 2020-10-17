using System.Threading.Tasks;
using Refit;

namespace AzureFunctions.Core
{ public interface IRandomRequestApi
    {
        //endpoint for random api
        [Get("/random?auth=null")]
        Task<string> GetRandom();
    }
}