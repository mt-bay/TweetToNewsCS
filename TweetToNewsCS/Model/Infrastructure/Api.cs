using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace TweetToNewsCS.Model.Infrastructure
{
    /// <summary>
    /// WebAPIを叩くクラス
    /// </summary>
    static class Api
    {
        /// <summary>
        /// GET
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="query">クエリ(hoge=piyoの形式で書いてください)</param>
        /// <returns>受け取った結果</returns>
        /// <exception cref="WebException">正常処理以外のステータスが帰ってきた場合</exception>
        public static string Get(string url, params string[] query)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + ArgsToQuery(query));
            request.Method = "Get";

            return Access(request);
        }


        /// <summary>
        /// 引数のHttpWebRequestを用いてAPIコールし、結果を取得する
        /// </summary>
        /// <param name="request">アクセス先のHttpWebRequest</param>
        /// <returns>受け取った結果</returns>
        /// /// <exception cref="WebException">正常処理以外のステータスが帰ってきた場合</exception>
        private static string Access(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (!IsSuccessStatusCode((int)response.StatusCode))
                {
                    throw new WebException(request.RequestUri.ToString() + "の取得に失敗しました(" + response.StatusDescription + ")");
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return 200 <= statusCode && statusCode < 300;
        }


        /// <summary>
        /// 与えられた引数をパラメータ化して返す
        /// </summary>
        /// <param name="query">パラメータ化したい内容</param>
        /// <returns>引数をパラメータ化したもの</returns>
        private static string ArgsToQuery(params string[] query)
        {
            return query.Any() ? "?" + string.Join("&", query) : string.Empty;
        }
    }
}
