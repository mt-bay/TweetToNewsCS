using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using TweetsToWords.Model.Application;
using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;

namespace TweetsToWords
{
    class Program
    {
        static void Main(string[] args)
        {
            TweetsToWordsOption option = TweetsToWordsOption.Parse(args);

            if(option.InputFile == null && option.InputDirectory == null)
            {
                Console.Error.WriteLine();
            }
            IEnumerable<TwitterStatus> tweets = new List<TwitterStatus>();
            if(option.InputFile != null)
            {
                using (StreamReader reader = new StreamReader(option.InputFile))
                {
                    tweets = tweets.Concat(JsonConvert.DeserializeObject<IEnumerable<TwitterStatus>>(reader.ReadToEnd()));
                }
            }

            if (option.InputDirectory != null)
            {
                foreach(string f in Directory.GetFiles(option.InputDirectory))
                {
                    using (StreamReader reader = new StreamReader(f))
                    {
                        JsonConvert.DeserializeObject<IEnumerable<TwitterStatus>>(reader.ReadToEnd());
                    }
                }
            }

            List<IEnumerable<MeCabResult>> parsed = MeCab.Parse(tweets).Filtering(option).ToList();
            Console.WriteLine("単語の数 : {0}", parsed.Sum(e => e.Count()));

            List<MeCabResultAggregate> aggregate = MeCab.AggregateAll(parsed).OrderByDescending(a => a.Num).ToList();

            if (!Directory.Exists(option.OutputFile))
            {
                Directory.CreateDirectory(option.OutputFile);
            }

            using (StreamWriter writer = new StreamWriter(option.OutputFile))
            {
                writer.WriteLine(JsonConvert.SerializeObject(aggregate, Formatting.Indented));
            }
        }
    }
}
