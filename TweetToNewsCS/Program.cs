using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMeCab;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using TweetSharp;
using TweetToNewsCS.Model.Infrastructure;
using System.IO;
using TweetToNewsCS.Model.Domain;

namespace TweetToNewsCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string rawJson = "";
            using (StreamReader reader = new StreamReader("C:/src/TweetsToNewsCS/TweetToNewsCS/data/json/20160627_niigata.json"))
            {
                rawJson = reader.ReadToEnd();
            }

            List<TwitterStatus> search = TwitterApi.Search("\"ラドンもそうだそうだと言っています\"").ToList();
            Console.WriteLine(@"{0}(@{1})さんの忍者ランド：{2}", search[0].User.Name, search[0].User.ScreenName, search[0].Text);

            MeCabResult result = MeCab.Parse(search[0].Text);

            Console.WriteLine(JsonConvert.SerializeObject(result));

            Console.ReadKey();
        }
    }
}
