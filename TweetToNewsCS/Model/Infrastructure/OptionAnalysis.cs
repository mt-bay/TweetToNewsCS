using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TweetToNewsCS.Model.Domain;
using Newtonsoft.Json;
using CommandLine;

namespace TweetToNewsCS.Model.Infrastructure
{
    public static class OptionAnalysis
    {
        public static T Parse<T>(string[] args)
            where T : IOption
        {
            T returns = default(T);
            int code = Parser.Default.ParseArguments<T>(args)
                .MapResult(
                    (T opt) =>
                    {
                        returns = opt;
                        return 0;
                    },
                    err => 1
                );
            switch (code)
            {
                case 0:
                    return returns;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
