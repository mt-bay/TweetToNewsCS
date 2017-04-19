using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Infrastructure
{
    class TextFilter
    {
        public static string Filtering(string raw)
        {
            string returns = raw;

            returns = RemoveUrl(returns);
            returns = RemoveReplyTo(returns);
            returns = RemoveRetweetSign(returns);
            returns = RemoveNewLine(returns);

            return returns;
        }


        /// <summary>
        /// リプライの宛先表示を消す
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        private static string RemoveReplyTo(string raw)
        {
            return Regex.Replace(raw, @"@[\w:]+\s*", "");
        }


        /// <summary>
        /// RTを表す記号の削除
        /// </summary>
        /// <returns></returns>
        private static string RemoveRetweetSign(string raw)
        {
            return Regex.Replace(raw, @"^RT(\s)*", "");
        }


        /// <summary>
        /// URLの除去
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        private static string RemoveUrl(string raw)
        {
            return Regex.Replace(raw, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./_%&=]*)?", "");
        }


        private static string RemoveNewLine(string raw)
        {
            return Regex.Replace(raw, @"[\r\n]", "");
        }
    }
}
