using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderClient.Tinder
{
    class LikedUser
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("match")]
        public bool Matched { get; set; }

        [JsonProperty("likes_remaining")]
        public int LikesRemaining { get; set; }
    }
}
