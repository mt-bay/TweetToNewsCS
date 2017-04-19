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
            Options option = OptionAnalysis.Analysis(args);

            List<TwitterStatus> data = Logic.GetData(option).ToList();

            Console.WriteLine("ツイートの数 : {0}", data.Count);

            List< IEnumerable<MeCabResult> > parsed = MeCab.Parse(data).Filtering(option).ToList();
            Console.WriteLine("単語の数 : {0}", parsed.Count);

            List<MeCabResultAggregate> aggregate = MeCab.AggregateAll(parsed).OrderByDescending(a => a.Num).ToList();

            if(!Directory.Exists("単語/"))
            {
                Directory.CreateDirectory("単語/");
            }

            string outPath = "単語/" + (option.output) + "_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".json";
            using (StreamWriter writer = new StreamWriter(outPath))
            {
                writer.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
            }
        }
    }
}