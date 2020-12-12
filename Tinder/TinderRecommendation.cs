using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharpTinder
{

    public class TinderRecommendation
    {

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("data")]
        public Result Data { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
