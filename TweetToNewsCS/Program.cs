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
            //args = args.Concat(new string[] { "" }).ToArray();

            //string[] options = new string[] { "-query", "-file", "-raw", "-filter" };
            Options option = OptionAnalysis.Analysis(args);

            string rawJson = "";
            using (StreamReader reader = new StreamReader(option.file))
            {
                rawJson = reader.ReadToEnd();
            }

            List<TwitterStatus> search = TwitterApi.Search(option.query).ToList();
            Console.WriteLine(@"{0}(@{1})さんの忍者ランド：{2}", search[0].User.Name, search[0].User.ScreenName, search[0].Text);

            List<MeCabResult> result = MeCab.Parse(search[0].Text).ToList();

            MeCabFilter filter = MeCabFilter.GenerateFromFile(option.filterfile);

            List<MeCabResult> filtered = filter.Filtering(result);

            Console.WriteLine(JsonConvert.SerializeObject(filtered, Formatting.Indented));

            Console.ReadKey();
        }
    }
}
