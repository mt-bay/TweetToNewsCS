using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Reflection;

namespace TweetToNewsCS.Model.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class TweetSearchOption : IOption
    {
        public TweetSearchOption()
        {
        }


        public static TweetSearchOption Parse(string[] args)
        {
            TweetSearchOption returns = new TweetSearchOption();
            Parser.Default.ParseArguments<TweetSearchOption>(args).
                MapResult((TweetSearchOption opt) =>
                {
                    returns = opt;
                    return 0;
                },
                err => -1);
            return returns;
        }


        [Option('q', "query", Default = "")]
        public string Query { get; set; }

        [Option('s', "since", Default = null)]
        public string Since { get; set; }

        [Option('u', "until", Default = null)]
        public string Until { get; set; }

        [Option("lang", Default = null)]
        public string Lang { get; set; }

        [Option("locale", Default = null)]
        public string Locale { get; set; }

        [Option("latitude", Default = null)]
        public double? Latitude { get; set; }

        [Option("longtitude", Default = null)]
        public double? Longtitude { get; set; }

        [Option('r', "radius", Default = null)]
        public int? Radius { get; set; }

        [Option("sinceid", Default = null)]
        public int? SinceId { get; set; }

        [Option("maxid", Default = null)]
        public int? MaxId { get; set; }

        [Option('c', "count", Default = null)]
        public int? Count { get; set; }

        [Option('o', "output", Default = "out.json")]
        public string OutputFile { get; set; }
    }
}
