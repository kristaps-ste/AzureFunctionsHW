using System;
using System.Globalization;
using Newtonsoft.Json;

namespace AzureFunctions.Core.Models
{
    public class GetLogResponseModel
    {
        [JsonProperty("id")]
        public long RowKey { get; set; }
        
        [JsonProperty("wasSuccess")]
        public bool Success { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp
            => new DateTime(RowKey).ToString(CultureInfo.InvariantCulture);
    }
}
