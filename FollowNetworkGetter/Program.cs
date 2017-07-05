using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using TweetToNewsCS.Model.Infrastructure;

namespace FollowNetworkGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("test.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(TwitterApi.FollowersList(scrrenName: "Poncho_Mt_Bay").Select(e => e.ScreenName), Formatting.Indented));

            }
        }
    }
}
