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

            List<TwitterStatus> data = new List<TwitterStatus>();

            if(option.raw != string.Empty)
            {
                using (StreamReader reader = new StreamReader(option.file))
                {
                    data = data.Concat((List<TwitterStatus>)JsonConvert.DeserializeObject(reader.ReadToEnd())).ToList();
                }
            }

            if(option.query != string.Empty)
            {
                data = data.Concat(TwitterApi.Search(option.query)).ToList();
            }

            Console.WriteLine("ツイートの数 : {0}", data.Count);

            //Console.WriteLine(@"{0}(@{1})さんの忍者ランド：{2}", data[0].User.Name, data[0].User.ScreenName, data[0].Text);

            List<MeCabResult> parsed = MeCab.Parse(data).ToList();
            Console.WriteLine("単語の数 : {0}", parsed.Count);

            //List<MeCabResult> result = MeCab.Parse(data[0].Text).ToList();

            MeCabFilter filter = MeCabFilter.GenerateFromFile(option.filterfile);

            if(option.filterfile != string.Empty)
            {
                filter.AddFilterFromFile(option.filterfile);
            }
            if(option.filterraw != string.Empty)
            {
                filter.AddFilterFromJson(option.filterraw);
            }

            List<MeCabResult> filtered = filter.Filtering(parsed);

            Dictionary<string, MeCabResultAggregate> aggregate = MeCab.Aggregate(filtered);

            Console.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
            if (option.output != string.Empty)
            {
                using (StreamWriter writer = new StreamWriter(option.output))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
                }
            }


            Console.ReadKey();
        }
    }
}