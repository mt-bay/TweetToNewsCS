using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TweetToNewsCS.Model.Domain;

using NMeCab;

namespace TweetToNewsCS.Model.Infrastructure
{
    static class MeCab
    {
        /// <summary>
        /// 受け取った文字列を形態素解析し、その結果を返す
        /// </summary>
        /// <param name="target">解析対象の文字列</param>
        /// <returns>解析結果(List形式)</returns>
        public static IEnumerable<MeCabResult> Parse(string target)
        {
            MeCabTagger tagger = Create();

            MeCabNode node = MeCab.Create().ParseToNode(target);

            return node.ToMeCabResultEnumerable();
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