using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;

using TweetSharp;

namespace TweetToNewsCS.Model.Infrastructure
{
    public static class TwitterApi
    {
        private static TwitterService service = new TwitterService("Zm7v29Du4NvBPObSmCYaxhzof", "QzL6oY4LulEyWLEAT5lIlhTMcFpkKfJS41qBUj0RwkQGr264op", "856545703-j9pzCh3aD9HzPnPU9voQnRXebAlwaYDYoJJdXAEa", "HcRSpAgaUoSePUbcbyU7btGh8V94LYZy5J4jJ1Aps8M2X");

        static TwitterApi()
        {
            //service.AuthenticateWith();            
        }


        public static IEnumerable<TwitterStatus> Search(TweetSearchOption option)
        {
            SearchOptions options = new SearchOptions
            {
                //Resulttype = TwitterSearchResultType.Recent,
                Q = GenerateQueryString(option.Query, DateTime.Parse(option.Since), DateTime.Parse(option.Until)),
                Lang = option.Lang,
                Locale = option.Locale,
                //IncludeEntities = includeEntities,
                Geocode =  GeoValueToGeoData(option.Latitude, option.Longtitude, option.Radius),
                SinceId = option.SinceId,
                MaxId = option.MaxId,
                Count = option.Count
            };

            return Search(options);
        }

        public static IEnumerable<TwitterStatus> Search(string query = null, DateTime? since = null, DateTime? until = null, string lang = null, string locale = null, bool includeEntities = true,
                                                        double? geoLatitude = null, double? geoLongtitude = null, int? geoRadius = null,
                                                        long? sinceId = null, long? maxId = null, int? count = null)
        {
            SearchOptions options = new SearchOptions
            {
                //Resulttype = TwitterSearchResultType.Recent,
                Q = GenerateQueryString(query, since, until),
                Lang = string.IsNullOrEmpty(lang) ? null : lang,
                Locale = string.IsNullOrEmpty(locale) ? null : locale,
                //IncludeEntities = includeEntities,
                Geocode = GeoValueToGeoData(geoLatitude, geoLongtitude, geoRadius),
                SinceId = sinceId,
                MaxId = maxId,
                Count = count.HasValue ? (count > 100 ? 100 : count) : null
            };

            return Search(options);
        }


        private static IEnumerable<TwitterStatus> Search(SearchOptions option)
        {
            TwitterSearchResult result = service.Search(option);

            if (result == null)
            {
                return new List<TwitterStatus>();
            }
            IEnumerable<TwitterStatus> returns = result.Statuses;

            if (returns == null || !returns.Any())
            {
                return returns;
            }

            int last = (option.Count ?? 0) - returns.Count();
            Console.WriteLine("取得したツイート数 : {0, 3}, 先頭のID : {1}, 末尾のID : {2}, ほしいツイートの残り数 : {3}", returns.Count(), returns.First().Id, returns.Last().Id, last);

            if (last > 0)
            {
                try
                {
                    SearchOptions next = option;
                    next.MaxId = returns.Last().Id - 1;
                    next.Count = last;
                    returns = returns.Concat(Search(next));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Twitter検索の際に例外が発生しました(メッセージ : {0})", e.Message);
                }
            }

            return returns;
        }



        public static IEnumerable<TwitterUser> FollowList(long? userId = null, string scrrenName = null)
        {
            if (userId == null && scrrenName == null)
            {
                return new List<TwitterUser>();
            }

            FollowUserOptions option = new FollowUserOptions
            {
                
            };


            var follows = service.FollowUser(option);

            /*
            while (follows?.NextCursor != null)
            {
                option.Cursor = followers.NextCursor;

                followers = service.ListFollowers(option);

                if (followers != null)
                {
                    returns = returns.Concat(followers.ToList());
                }
            }

            return returns;
            */
            return new List<TwitterUser>();
        }


        public static IEnumerable<TwitterUser> FollowersList(long? userId = null, string scrrenName = null)
        {
            if(userId == null && scrrenName == null)
            {
                return new List<TwitterUser>();
            }

            TweetSharp.ListFollowersOptions option = new ListFollowersOptions
            {
                UserId = userId,
                ScreenName = scrrenName,
            };

            TwitterCursorList<TwitterUser> followers = service.ListFollowers(option);

            IEnumerable<TwitterUser> returns = followers?.ToList();
            while (followers?.NextCursor != null)
            {
                option.Cursor = followers.NextCursor;

                followers = service.ListFollowers(option);

                if(followers != null)
                {
                    returns = returns.Concat(followers.ToList());
                }
            }

            return returns;
        }


        private static string GenerateQueryString(string query = null, DateTime? since = null, DateTime? until = null)
        {
            return (string.IsNullOrEmpty(query) && since == null && until == null) ? null :
                (query ?? string.Empty) + " -rt" + ((since == null) ? "" : " since:" + since.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST") + ((until == null) ? "" : " until:" + until.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST");
        }


        private static TwitterGeoLocationSearch GeoValueToGeoData(double? geoLatitude, double? geoLongtitude, int? geoRadius)
        {
            //位置情報引数全てがnullでない場合は位置情報を生成
            return (geoLatitude.HasValue && geoLongtitude.HasValue && geoRadius.HasValue) ?
                new TwitterGeoLocationSearch(latitutde: geoLatitude.Value, longitude: geoLongtitude.Value, radius: geoRadius.Value, unitOfMeasurement: TwitterGeoLocationSearch.RadiusType.Km) :
                null;
        }
    }
}
