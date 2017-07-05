using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Domain
{
    /// <summary>
    /// MeCabの解析結果の集計用構造体
    /// </summary>
    public struct MeCabResultAggregate
    {
        public MeCabResult Result { get; set; }
        public uint        Num    { get; set; }

        public void NumIncrement()
        {
            Num++;
        }
    }
}
