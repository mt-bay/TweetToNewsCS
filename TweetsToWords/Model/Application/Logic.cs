using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;
using TweetToNewsCS.Model.Infrastructure;

namespace TweetsToWords.Model.Application
{
    public static class Logic
    {
        public static IEnumerable<MeCabResult> Filtering(this IEnumerable<MeCabResult> filteringTarget, TweetsToWordsOption option)
        {
            MeCabFilter filter = new MeCabFilter();

            if (option.AcceptFilePath != string.Empty)
            {
                filter.AddAcceptFilterFromFile(option.AcceptFilePath);
            }

            if (option.FilterFilePath != string.Empty)
            {
                filter.AddFilterFromFile(option.FilterFilePath);
            }
            
            //if (option.filterraw != string.Empty)
            //{
            //    filter.AddFilterFromJson(option.filterraw);
            //}

            return filter.Filtering(filteringTarget);
        }


        public static IEnumerable<IEnumerable<MeCabResult>> Filtering(this IEnumerable<IEnumerable<MeCabResult>> filteringTarget, TweetsToWordsOption option)
        {
            MeCabFilter filter = new MeCabFilter();

            if (option.AcceptFilePath != string.Empty)
            {
                filter.AddAcceptFilterFromFile(option.AcceptFilePath);
            }

            if (option.FilterFilePath != string.Empty)
            {
                filter.AddFilterFromFile(option.FilterFilePath);
            }
            //if (option.filterraw != string.Empty)
            //{
            //    filter.AddFilterFromJson(option.filterraw);
            //}

            return filteringTarget.Select(f => filter.Filtering(f));
        }
    }
}
