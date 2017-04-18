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
        public string since { get; set; }
        public string until { get; set; }
        public string lang { get; set; }
        public string locale { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }
        public string radius { get; set; }
        public string sinceid { get; set; }
        public string maxid { get; set; }
        public string count { get; set; }

        public string file { get; set; }
        public string raw { get; set; }
        public string filterfile { get; set; }
        public string filterraw { get; set; }
        public string acceptfile { get; set; }
        public string output { get; set; }
    }
}
