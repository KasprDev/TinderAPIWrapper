using Newtonsoft.Json;
using SharpTinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderClient.Tinder.Custom
{
    public class FastMatches
    {
        [JsonProperty("data")]
        public Result Data { get; set; }
    }

    public class FastData
    {
        [JsonProperty("results")]
        public IList<TinderRecommendedUser> Results { get; set; }
    }

    public class FastUser
    {
        [JsonProperty("is_fast_match")]
        public bool IsFastMatch { get; set; }

        [JsonProperty("s_number")]
        public long SNumber { get; set; }
    }
}
