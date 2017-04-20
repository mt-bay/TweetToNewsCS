using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;

namespace TfIdfCalc.Model.Infrastructure
{
    class TfIdf
    {
        public static Dictionary<string, Dictionary<string, double> > GetAllTfIdf(Dictionary<string, IEnumerable<MeCabResultAggregate>> documents)
        {
            List<Dictionary<string, double>> returnVals = new List<Dictionary<string, double>>();
            for(int i = 0; i < documents.Count(); ++i)
            {
                returnVals.Add(new Dictionary<string, double>());
            }

            List<string> words = GetAllWords(documents).ToList();

            foreach(string w in words)
            {
                List<uint> tf = GetTf(documents.Values, w).ToList();
                double idf = GetIdf(documents.Values, w);

                for(int i = 0; i < tf.Count() && i < documents.Count(); ++i)
                {
                    returnVals[i][w] = tf[i] * idf;
                }
            }

            Dictionary<string, Dictionary<string, double>> returns = new Dictionary<string, Dictionary<string, double>>();
            for (int i = 0; i < documents.Count() && i < returnVals.Count(); ++i)
            {
                returns[documents.Keys.ToList()[i]] = returnVals[i].OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            }

            return returns;
        }


        public static IEnumerable<uint> GetTf(IEnumerable< IEnumerable<MeCabResultAggregate> > documents, string targetWord)
        {
            return documents.Select(e => e.Where(f => f.Result.原形.Equals(targetWord)).Any() ? e.Where(f => f.Result.原形.Equals(targetWord)).FirstOrDefault().Num : 0u);
        }


        public static double GetIdf(IEnumerable<IEnumerable<MeCabResultAggregate>> documents, string targetWord)
        {
            return Math.Log10(documents.Count() / GetDf(documents, targetWord)) + 1;
        }


        private static double GetDf(IEnumerable< IEnumerable<MeCabResultAggregate> > documents, string targetWord)
        {
            return documents.Select(e => e.Where(f => f.Result.原形.Equals(targetWord)).Any() ? e.Where(f => f.Result.原形.Equals(targetWord)).FirstOrDefault().Num : 0u).Sum(e => e);
        }


        private static IEnumerable<string> GetAllWords(Dictionary<string, IEnumerable<MeCabResultAggregate>> documents)
        {
            List<string> returns = new List<string>();

            foreach (IEnumerable<MeCabResultAggregate> d in documents.Values)
            {
                foreach(MeCabResultAggregate m in d)
                {
                    if (!returns.Where(e => e.Equals(m.Result.原形)).Any())
                    {
                        returns.Add(m.Result.原形);
                    }
                }
            }

            return returns;
        }
    }
}
