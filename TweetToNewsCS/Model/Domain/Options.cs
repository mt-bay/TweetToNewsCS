using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Domain
{
    class Options
    {
        public string query { get; set; }
        public string file { get; set; }
        public string raw { get; set; }
        public string filterfile { get; set; }
        public string filterraw { get; set; }
        public string output { get; set; }
    }
}
