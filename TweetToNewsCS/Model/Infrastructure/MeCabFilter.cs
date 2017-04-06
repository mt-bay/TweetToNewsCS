using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;

namespace TweetToNewsCS.Model.Infrastructure
{
    class MeCabFilter
    {
        private List<MeCabResult> filter;


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


        public static MeCabFilter GenerateFromJson(string json)
        {
            return new MeCabFilter(JsonConvert.DeserializeObject<List<MeCabResult>>(json));
        }


        public bool IsPassable(MeCabResult target)
        {
            foreach(MeCabResult f in filter)
            {
            }
            return true;
        }
    }
}
