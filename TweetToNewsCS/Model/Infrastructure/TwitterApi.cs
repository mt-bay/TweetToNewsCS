using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetToNewsCS.Model.Domain;

using TweetSharp;

namespace TweetToNewsCS.Model.Infrastructure
{
    static class TwitterApi
    {
        private static TwitterService service = new TwitterService("Zm7v29Du4NvBPObSmCYaxhzof", "QzL6oY4LulEyWLEAT5lIlhTMcFpkKfJS41qBUj0RwkQGr264op", "856545703-j9pzCh3aD9HzPnPU9voQnRXebAlwaYDYoJJdXAEa", "HcRSpAgaUoSePUbcbyU7btGh8V94LYZy5J4jJ1Aps8M2X");

        static TwitterApi()
        {
            //service.AuthenticateWith();            
        }


        public static IEnumerable<TwitterStatus> Search(Options option)
        {
            DateTime? since, until;
            double? geoLatitude, geoLongtitude;
            long? sinceId, maxId;
            int? radius, count;

            if(string.IsNullOrEmpty(option.since))
            {
                since = null;
            }
            else
            {
                since = DateTime.Parse(option.since);
            }

            if(string.IsNullOrEmpty(option.until))
            {
                until = null;
            }
            else
            {
                until = DateTime.Parse(option.until);
            }

            if(string.IsNullOrEmpty(option.latitude))
            {
                geoLatitude = null;
            }
            else
            {
                geoLatitude = double.Parse(option.latitude);
            }

            if(string.IsNullOrEmpty(option.longtitude))
            {
                geoLongtitude = null;
            }
            else
            {
                geoLongtitude = double.Parse(option.longtitude);
            }

            if(string.IsNullOrEmpty(option.radius))
            {
                radius = null;
            }
            else
            {
                radius = int.Parse(option.radius);
            }

            if(string.IsNullOrEmpty(option.sinceid))
            {
                sinceId = null;
            }
            else
            {
                sinceId = long.Parse(option.sinceid);
            }

            if(string.IsNullOrEmpty(option.maxid))
            {
                maxId = null;
            }
            else
            {
                maxId = long.Parse(option.maxid);
            }

            if(string.IsNullOrEmpty(option.count))
            {
                count = null;
            }
            else
            {
                count = int.Parse(option.count);
            }

            return Search(option.query, since, until, option.lang, option.locale, true,
                          geoLatitude, geoLongtitude, radius,
                          sinceId, maxId, count);
        }

        public static IEnumerable<TwitterStatus> Search(string query = null, DateTime? since = null, DateTime? until = null, string lang = null, string locale = null, bool includeEntities = true,
                                                        double? geoLatitude = null, double? geoLongtitude = null, int? geoRadius = null,
                                                        long? sinceId = null, long? maxId = null, int? count = null)
        {
            try
            {
                //位置情報引数全てがnullでない場合は位置情報を生成
                TwitterGeoLocationSearch geo = (geoLatitude.HasValue && geoLongtitude.HasValue && geoRadius.HasValue) ?
                    new TwitterGeoLocationSearch(latitutde: geoLatitude.Value, longitude: geoLongtitude.Value, radius: geoRadius.Value, unitOfMeasurement: TwitterGeoLocationSearch.RadiusType.Km) :
                    null;

                string q = (string.IsNullOrEmpty(query) && since == null && until == null) ? null :
                    (query ?? string.Empty) + " -rt" + ((since == null) ? "" : " since:" + since.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST") + ((until == null) ? "" : " until:" + until.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST");

                SearchOptions options = new SearchOptions
                {
                    //Resulttype = TwitterSearchResultType.Recent,
                    Q = q,
                    Lang = string.IsNullOrEmpty(lang) ? null : lang,
                    Locale = string.IsNullOrEmpty(locale) ? null : locale,
                    //IncludeEntities = includeEntities,
                    Geocode = geo,
                    SinceId = sinceId,
                    MaxId = maxId,
                    Count = count.HasValue ? (count > 100 ? 100 : count) : null
                };

                TwitterSearchResult result = service.Search(options);
                if(result == null)
                {
                    return new List<TwitterStatus>();
                }
                IEnumerable<TwitterStatus> returns = result.Statuses;
                int last = (count ?? 0) - returns.Count();

                if(last > 0 && returns.Any())
                {
                    try
                    {
                        returns = returns.Concat(Search(query, since, until, lang, locale, includeEntities,
                                                        geoLatitude, geoLongtitude, geoRadius,
                                                        sinceId, returns.Last().Id, last));
                    }
                    catch(Exception e)
                    {
                        Console.Error.WriteLine("Twitter検索の際に例外が発生しました(メッセージ : {0})", e.Message);
                    }
                }

                return returns;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
