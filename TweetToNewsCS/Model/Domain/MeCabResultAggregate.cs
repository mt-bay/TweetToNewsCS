using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Domain
{
    struct MeCabResultAggregate
    {
        public MeCabResult Result { get; set; }
        public uint        Num    { get; set; }

        public void NumIncrement()
        {
            Num++;
        }
    }
}
