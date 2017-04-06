using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Domain
{
    struct MeCabResult
    {
        /// <summary> 表層形 </summary>
        public string 表層形 { get; set; }
        /// <summary> 品詞 </summary>
        public string 品詞 { get; set; }
        /// <summary> 品詞細分類1 </summary>
        public string 品詞細分類1 { get; set; }
        /// <summary> 品詞細分類2 </summary>
        public string 品詞細分類2 { get; set; }
        /// <summary> 品詞細分類3 </summary>
        public string 品詞細分類3 { get; set; }
        /// <summary> 活用形 </summary>
        public string 活用形 { get; set; }
        /// <summary> 活用型 </summary>
        public string 活用型 { get; set; }
        /// <summary> 原型 </summary>
        public string 原形 { get; set; }
        /// <summary> 読み </summary>
        public string 読み { get; set; }
        /// <summary> 発音 </summary>
        public string 発音 { get; set; }
    }
}
