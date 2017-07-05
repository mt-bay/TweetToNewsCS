using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TweetSharp;
using TweetToNewsCS.Model.Infrastructure;
using TweetToNewsCS.Model.Domain;

namespace TweetSearch.Model.Application
{
    public static class Logic
    {
        public static IEnumerable<TwitterStatus> Search(TweetSearchOption option)
        {
            List<TwitterStatus> returns = new List<TwitterStatus>();

            IEnumerable<TwitterStatus> searchResult = TwitterApi.Search(option);
            char[] invalidChara = Path.GetInvalidFileNameChars();
            string queryFileName = option.Query + "_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".json";
            foreach (char c in invalidChara)
            {
                queryFileName = queryFileName.Replace(c, Strings.StrConv(c.ToString(), VbStrConv.Wide)[0]);
            }

            if (!Directory.Exists("検索結果/"))
            {
                Directory.CreateDirectory("検索結果");
            }

            using (StreamWriter writer = new StreamWriter("検索結果/" + queryFileName))
            {
                writer.WriteLine(JsonConvert.SerializeObject(searchResult, Formatting.Indented));
            }
            returns = returns.Concat(searchResult).ToList();

            return returns;
        }
    }
}