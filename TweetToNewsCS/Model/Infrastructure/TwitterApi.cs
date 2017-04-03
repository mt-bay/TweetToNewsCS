using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                string q = (query == null && since == null && until == null) ? null :
                    (query ?? string.Empty) + ((since == null) ? "" : " since:" + since.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST") + ((until == null) ? "" : " until:" + until.Value.ToString("yyyy-MM-dd_HH:mm:ss") + "_JST");

                SearchOptions options = new SearchOptions
                {
                    //Resulttype = TwitterSearchResultType.Recent,
                    Q = q,
                    Lang = lang,
                    Locale = locale,
                    //IncludeEntities = includeEntities,
                    Geocode = geo,
                    SinceId = sinceId,
                    MaxId = maxId,
                    Count = count
                };

                TwitterSearchResult result = service.Search(options);

                return result.Statuses;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
