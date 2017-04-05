using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;

namespace TweetToNewsCS.Model.Application
{
    class Logic
    {
        public static IEnumerable<MeCabResult> FilteredParse(string target)
        {
            return MeCab.Parse(target);
        }
    }
}
