using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TweetToNewsCS.Model.Domain;

using NMeCab;
using TweetSharp;


namespace TweetToNewsCS.Model.Infrastructure
{
    static class MeCab
    {
        private static MeCabTagger tagger;


        static MeCab()
        {
            tagger = MeCab.Create();
        }


        public static IEnumerable<MeCabResult> ParseAndConcat(IEnumerable<TwitterStatus> target)
        {
            List<MeCabResult> returns = new List<MeCabResult>();
            foreach(TwitterStatus s in target)
            {
                returns = returns.Concat(Parse(s.Text)).ToList();
            }

            return returns;
        }


        public static IEnumerable< IEnumerable<MeCabResult> > Parse(IEnumerable<TwitterStatus> target)
        {
            return target.Select(t => Parse(TextFilter.Filtering(t.Text)));
        }


        /// <summary>
        /// 受け取った文字列を形態素解析し、その結果を返す
        /// </summary>
        /// <param name="target">解析対象の文字列</param>
        /// <returns>解析結果(List形式)</returns>
        public static IEnumerable<MeCabResult> Parse(string target)
        {
            MeCabNode node = tagger.ParseToNode(target);
            return node.ToMeCabResultEnumerable();
        }


        public static List<MeCabResultAggregate> Aggregate(IEnumerable<MeCabResult> target)
        {
            Dictionary<string, MeCabResultAggregate> returns = new Dictionary<string, MeCabResultAggregate>();

            foreach(MeCabResult m in target)
            {
                returns = returns.AddResult(m);
            }

            return returns.Values.ToList();
        }


        public static List<MeCabResultAggregate> AggregateAll(IEnumerable< IEnumerable<MeCabResult> > target)
        {
            Dictionary<string, MeCabResultAggregate> returns = new Dictionary<string, MeCabResultAggregate>();

            foreach(IEnumerable<MeCabResult> e in target)
            {
                foreach(MeCabResult m in e)
                {
                    returns = returns.AddResult(m);
                }
            }

            return returns.Values.ToList();
        }


        private static Dictionary<string, MeCabResultAggregate> AddResult(this Dictionary<string, MeCabResultAggregate> result, MeCabResult add)
        {
            MeCabResultAggregate buf = new MeCabResultAggregate();
            buf.Result       = add;
            buf.Num          = result.ContainsKey(add.原形) ? result[add.原形].Num + 1 : 1;
            result[add.原形] = buf;

            return result;
        }


        /// <summary>
        /// 受け取ったnodeから先をIEnumerable&gt;MeCabResult&lt;に変換する
        /// </summary>
        /// <param name="node">変換対象のnodeの先頭</param>
        /// <returns>nodeから先をIEnumerable&gt;MeCabResult&lt;に変換したもの</returns>
        internal static IEnumerable<MeCabResult> ToMeCabResultEnumerable(this MeCabNode node)
        {
            while (node != null)
            {
                //BOS/EOSを弾く
                if (node.CharType > 0)
                {
                    yield return node.ToMeCabResult();
                }
                node = node.Next;
            }
        }


        /// <summary>
        /// nodeをMeCabResultに変換
        /// </summary>
        /// <param name="node">変換対象のnode</param>
        /// <returns></returns>
        internal static MeCabResult ToMeCabResult(this MeCabNode node)
        {
            string[] feature = node.Feature.Split(',');
            int filter = 0;

            return new MeCabResult
            {
                表層形 = node.Surface,

                品詞 = feature.Length >= ++filter ? feature[filter - 1] : "",
                品詞細分類1 = feature.Length >= ++filter ? feature[filter - 1] : "",
                品詞細分類2 = feature.Length >= ++filter ? feature[filter - 1] : "",
                品詞細分類3 = feature.Length >= ++filter ? feature[filter - 1] : "",
                活用形 = feature.Length >= ++filter ? feature[filter - 1] : "",
                活用型 = feature.Length >= ++filter ? feature[filter - 1] : "",
                原形 = feature.Length >= ++filter ? feature[filter - 1] : "",
                読み = feature.Length >= ++filter ? feature[filter - 1] : "",
                発音 = feature.Length >= ++filter ? feature[filter - 1] : "",
            };
        }


        /// <summary>
        /// MeCabインスタンスの作成
        /// </summary>
        /// <returns>MeCabインスタンス</returns>
        private static MeCabTagger Create()
        {
            MeCabParam param = new MeCabParam();

            return MeCabTagger.Create(param);
        }
    }
}