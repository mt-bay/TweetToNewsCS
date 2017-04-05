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
        public IEnumerable<MeCabResult> filter;

        public IEnumerable<MeCabResult> GenerateFromFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return JsonConvert.DeserializeObject<List<MeCabResult>>(reader.ReadToEnd());
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    }
}
