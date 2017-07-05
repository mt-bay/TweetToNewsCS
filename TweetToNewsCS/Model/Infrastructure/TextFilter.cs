using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Infrastructure
{
    public class TextFilter
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
        /// <param name="raw">削除前の文字列</param>
        /// <returns>削除後の文字列</returns>
        private static string RemoveReplyTo(string raw)
        {
            return Regex.Replace(raw, @"@[\w:]+\s*", "");
        }


        /// <summary>
        /// RTを表す記号の削除
        /// </summary>
        /// <param name="raw">削除前の文字列</param>
        /// <returns>削除後の文字列</returns>
        private static string RemoveRetweetSign(string raw)
        {
            return Regex.Replace(raw, @"^RT(\s)*", "");
        }


        /// <summary>
        /// URLの除去
        /// </summary>
        /// <param name="raw">削除前の文字列</param>
        /// <returns>削除後の文字列</returns>
        private static string RemoveUrl(string raw)
        {
            return Regex.Replace(raw, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./_%&=]*)?", "");
        }

        /// <summary>
        /// 改行文字の削除
        /// </summary>
        /// <param name="raw">削除前の文字列</param>
        /// <returns>削除後の文字列</returns>
        private static string RemoveNewLine(string raw)
        {
            return Regex.Replace(raw, @"[\r\n]", "");
        }
    }
}
