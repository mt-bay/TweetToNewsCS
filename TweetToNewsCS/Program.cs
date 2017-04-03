using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMeCab;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using TweetSharp;

using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;
using System.IO;

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

            
            
                List<TwitterStatus> search = TwitterApi.Search("\"わいわい忍者ランド\"").ToList();
                Console.WriteLine(@"{0}(@{1})さんの忍者ランド：{2}", search[0].User.Name, search[0].User.ScreenName, search[0].Text);

            //IsoDateTimeConverter formatter = new IsoDateTimeConverter { DateTimeFormat = @"ddd MMM dd HH:mm:ss zzz yyyy" };

            //List<Tweet> result = JsonConvert.DeserializeObject<List<Tweet>>(rawJson, formatter);

            //Console.WriteLine("resultの長さ : {0}", result.Count);
            //Console.WriteLine("resultのサンプル(時間 : {0}) : {1}", result[0].CreatedAt, result[0]);
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
            
            Console.ReadKey();
        }
    }
}
