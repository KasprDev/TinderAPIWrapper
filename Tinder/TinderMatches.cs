using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderClient.Tinder
{
    public class TinderMatches
    {
        [JsonProperty("data")]
        public MatchData Data { get; set; }
    }
}
