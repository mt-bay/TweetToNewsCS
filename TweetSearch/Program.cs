using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSearch.Model.Application;
using TweetSharp;
using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;

namespace TweetSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            TweetSearchOption option = TweetSearchOption.Parse(args);

            List<TwitterStatus> data = Logic.Search(option).ToList();

            Console.WriteLine("{0}件のツイートを取得しました", data.Count());

            if (!Directory.Exists(option.OutputFile))
            {
                Directory.CreateDirectory(option.OutputFile);
            }

            using (StreamWriter writer = new StreamWriter(option.OutputFile))
            {
                writer.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
            }

            return;
        }
    }
}
