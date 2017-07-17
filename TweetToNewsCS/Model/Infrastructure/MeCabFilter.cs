using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TweetToNewsCS.Model.Domain;

namespace TweetToNewsCS.Model.Infrastructure
{
    public class MeCabFilter
    {
        private List<MeCabResult> filter;
        private List<MeCabResult> acceptFilter;

        public MeCabFilter()
        {
            filter = new List<MeCabResult>();
            acceptFilter = new List<MeCabResult>();
        }

        private MeCabFilter(IEnumerable<MeCabResult> filterResults)
        {
            filter = filterResults.ToList();
        }


        public static MeCabFilter GenerateFromFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return new MeCabFilter(JsonConvert.DeserializeObject< List<MeCabResult> >(reader.ReadToEnd()));
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        

        public void AddFilterFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                filter = filter.Concat(JsonConvert.DeserializeObject<List<MeCabResult>>(reader.ReadToEnd())).ToList();
            }
        }

        public void AddAcceptFilterFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                acceptFilter = acceptFilter.Concat(JsonConvert.DeserializeObject<List<MeCabResult>>(reader.ReadToEnd())).ToList();
            }
        }


        public void AddFilterFromJson(string json)
        {
            filter = filter.Concat(JsonConvert.DeserializeObject<List<MeCabResult>>(json)).ToList();
        }


        public static MeCabFilter GenerateFromJson(string json)
        {
            return new MeCabFilter(JsonConvert.DeserializeObject<List<MeCabResult>>(json));
        }


        public IEnumerable<MeCabResult> Filtering(IEnumerable<MeCabResult> targetResult)
        {
            return targetResult.Where(t => IsPassable(t));
        }


        public bool IsPassable(MeCabResult target)
        {
            return (!acceptFilter.Any() || acceptFilter.Where(f => f.品詞 == target.品詞 &&
                    (f.品詞細分類1 == target.品詞細分類1 || f.品詞細分類1 == null) &&
                    (f.品詞細分類2 == target.品詞細分類2 || f.品詞細分類2 == null) &&
                    (f.品詞細分類3 == target.品詞細分類2 || f.品詞細分類3 == null)).Any()) &&
                !filter.Where(f => f.品詞 == target.品詞 &&
                    (f.品詞細分類1 == target.品詞細分類1 || f.品詞細分類1 == null) &&
                    (f.品詞細分類2 == target.品詞細分類2 || f.品詞細分類2 == null) &&
                    (f.品詞細分類3 == target.品詞細分類2 || f.品詞細分類3 == null)).Any();
        }
    }
}
