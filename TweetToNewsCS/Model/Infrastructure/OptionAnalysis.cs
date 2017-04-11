using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TweetToNewsCS.Model.Domain;

namespace TweetToNewsCS.Model.Infrastructure
{
    class OptionAnalysis
    {

        public static Options Analysis(string[] args)
        {
            if(args == null)
            {
                return new Options();
            }

            Options returns = new Options();
            Type returnsType = returns.GetType();

            //MemberInfo[] member = returnsType.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] property = returnsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<string> optionsList = property.Select(hoge => hoge.Name).ToList();
            string[] bufArgs = args.Length % 2 == 0 ? args.ToArray() : args.Concat(new string[] { "" }).ToArray();
            Dictionary<string, string> input = bufArgs.ToDictionary(e => e.Substring(1).ToLower(), e => bufArgs.SkipWhile(a => a != e).Skip(1).FirstOrDefault());

            foreach(PropertyInfo p in returnsType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    p.SetValue(returns, input[p.Name] ?? string.Empty);
                }
                catch(KeyNotFoundException e)
                {
                    p.SetValue(returns, string.Empty);
                }
            }

            return returns;
        }
    }
}
