using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace TweetToNewsCS.Model.Domain
{
    class Tweet
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("geo")]
        public string Geo { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt => DateTime.ParseExact(createdAt, "ddd MMM dd HH:mm:ss zzz yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
        [JsonProperty("created_at")]
        private string createdAt { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
