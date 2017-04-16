using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;
using Microsoft.VisualBasic;



namespace TweetToNewsCS.Model.Application
{
    static class Logic
    {
        public static IEnumerable<TwitterStatus> GetData(Options option)
        {
            List<TwitterStatus> returns = new List<TwitterStatus>();

            if (option.raw != string.Empty)
            {
                returns = returns.Concat((List<TwitterStatus>)JsonConvert.DeserializeObject(option.raw)).ToList();
            }

            if (option.file != string.Empty)
            {
                using (StreamReader reader = new StreamReader(option.file))
                {
                    returns = returns.Concat((IEnumerable<TwitterStatus>)JsonConvert.DeserializeObject(reader.ReadToEnd())).ToList();
                }
            }

            if (option.query != string.Empty)
            {
                IEnumerable<TwitterStatus> searchResult = TwitterApi.Search(option.query);
                char[] invalidChara = Path.GetInvalidFileNameChars();
                string queryFileName = option.query + "_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".json";
                foreach(char c in invalidChara)
                {
                    queryFileName = queryFileName.Replace(c, Strings.StrConv(c.ToString(), VbStrConv.Wide)[0]);
                }

                using (StreamWriter writer = new StreamWriter(queryFileName))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(searchResult, Formatting.Indented));
                }
                returns = returns.Concat(searchResult).ToList();
            }

            return returns;
        }


        public static IEnumerable<MeCabResult> Filtering(this IEnumerable<MeCabResult> filteringTarget, Options option)
        {
            MeCabFilter filter = new MeCabFilter();

            if (option.filterfile != string.Empty)
            {
                filter.AddFilterFromFile(option.filterfile);
            }
            if (option.filterraw != string.Empty)
            {
                filter.AddFilterFromJson(option.filterraw);
            }

            return filter.Filtering(filteringTarget);
        }


        public static IEnumerable<MeCabResult> FilteredParse(string target)
        {
            return MeCab.Parse(target);
        }
    }
}
