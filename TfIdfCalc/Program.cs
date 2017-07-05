using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfIdfCalc.Model.Infrastructure;
using TweetToNewsCS.Model.Domain;


namespace TfIdfCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, IEnumerable<MeCabResultAggregate>> aggregate = new Dictionary<string, IEnumerable<MeCabResultAggregate>>();

            if(args.Length == 0)
            {
                Console.Error.WriteLine("ディレクトリを指定してください");
                return;
            }

            foreach(string a in args)
            {
                foreach(string f in Directory.EnumerateFiles(a, "*.json", SearchOption.TopDirectoryOnly))
                {
                    using (StreamReader reader = new StreamReader(f))
                    {
                        aggregate[f] = JsonConvert.DeserializeObject<IEnumerable<MeCabResultAggregate>>(reader.ReadToEnd());
                    }
                    Console.WriteLine("{0}内の単語数 : {1}", f, aggregate[f].Count());
                }
            }

            Console.WriteLine("対象のファイル数 : {0}", aggregate.Count);

            Dictionary<string, Dictionary<string, double>> tfIdf = TfIdf.GetAllTfIdf(aggregate);

            foreach(KeyValuePair<string, Dictionary<string, double> > t in tfIdf)
            {
                string dist = "計算結果/" + t.Key;
                if(!Directory.Exists(Path.GetDirectoryName(dist)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dist));
                }

                using (StreamWriter writer = new StreamWriter(dist))
                {
                    writer.Write(JsonConvert.SerializeObject(t.Value, Formatting.Indented));
                }
            }

            foreach(KeyValuePair<string, Dictionary<string, double> > n in TfIdf.Normalize(tfIdf))
            {
                string dist = "計算結果_正規化/" + n.Key;
                if (!Directory.Exists(Path.GetDirectoryName(dist)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dist));
                }

                using (StreamWriter writer = new StreamWriter(dist))
                {
                    writer.Write(JsonConvert.SerializeObject(n.Value, Formatting.Indented));
                }
            }

            return;
        }
    }
}
