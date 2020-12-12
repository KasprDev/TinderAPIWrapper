using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TinderClient.Tinder;

namespace SharpTinder
{
    public class Result
    {
        [JsonProperty("results")]
        public IList<TinderRecommendedUser> Results { get; set; }
    }

    public class TinderRecommendedUser
    { 
        [JsonProperty("user")]
        public TinderUser User { get; set; }

        [JsonProperty("distance_mi")]
        public int Distance { get; set; }

        [JsonProperty("is_fast_match")]
        public bool IsFastMatch { get; set; }

        [JsonProperty("s_number")]
        public long SNumber { get; set; }
    }
}

