using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Domain
{
    public class TweetsToWordsOption
    {
        public static TweetsToWordsOption Parse(string[] args)
        {
            TweetsToWordsOption returns = new TweetsToWordsOption();
            Parser.Default.ParseArguments<TweetsToWordsOption>(args).
                MapResult((TweetsToWordsOption opt) =>
                {
                    returns = opt;
                    return 0;
                },
                err => 1
                );
            return returns;
        }

        [Option("inputfile", Default = null)]
        public string InputFile { get; set; }
        [Option("inputdir", Default = null)]
        public string InputDirectory { get; set; }
        [Option('o', "out", Default = "out.json")]
        public string OutputFile { get; set; }
        [Option('f', "filter", Default = null)]
        public string FilterFilePath { get; set; }
        //public string filterraw { get; set; }
        [Option('a', "accept", Default = null)]
        public string AcceptFilePath { get; set; }
    }
}
