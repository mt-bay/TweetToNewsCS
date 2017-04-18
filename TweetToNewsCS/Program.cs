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
using TweetToNewsCS.Model.Application;

namespace TweetToNewsCS
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = args.Concat(new string[] { "" }).ToArray();

            //string[] options = new string[] { "-query", "-file", "-raw", "-filter" };
            Options option = OptionAnalysis.Analysis(args);

            List<TwitterStatus> data = Logic.GetData(option).ToList();

            

            Console.WriteLine("ツイートの数 : {0}", data.Count);

            //Console.WriteLine(@"{0}(@{1})さんの忍者ランド：{2}", data[0].User.Name, data[0].User.ScreenName, data[0].Text);

            List< IEnumerable<MeCabResult> > parsed = MeCab.Parse(data).Filtering(option).ToList();
            Console.WriteLine("単語の数 : {0}", parsed.Count);

            //List<MeCabResult> result = MeCab.Parse(data[0].Text).ToList();

            Dictionary<string, MeCabResultAggregate> aggregate = MeCab.AggregateAll(parsed);

            Console.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
            if (option.output != string.Empty)
            {
                using (StreamWriter writer = new StreamWriter(option.output))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
                }
            }
        }
    }
}