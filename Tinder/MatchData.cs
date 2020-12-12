using Newtonsoft.Json;
using SharpTinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderClient.Tinder
{
    public class MatchData
    {
        [JsonProperty("matches")]
        public IList<Match> Matches { get; set; }
    }
}
