using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace TweetToNewsCS.Model.Domain
{
    /// <summary>
    /// ツイートを保存するクラス
    /// </summary>
    class Tweet
    {
        /// <summary>
        /// ツイート記事内容
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 位置情報。なければnull
        /// </summary>
        [JsonProperty("geo")]
        public string Geo { get; set; }

        /// <summary>
        /// 投稿日時
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedAt => DateTime.ParseExact(createdAt, "ddd MMM dd HH:mm:ss zzz yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
        /// <summary>
        /// 投稿日時(APIで取得したそのままのデータ)
        /// </summary>
        [JsonProperty("created_at")]
        private string createdAt { get; set; }


        /// <summary>
        /// インスタンス情報を文字列化して返す
        /// </summary>
        /// <returns>文字列化したインスタンス情報</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
