using System;
using System.Xml.Serialization;

namespace tvdbApi
{
    internal class Tvdb
    {
        public string ApiKey
        {
            get { return TvdbApiMethods.ApiKey; }
            protected set { TvdbApiMethods.ApiKey = value; }
        }

        public Tvdb(string apiKey)
        {
            ApiKey = apiKey;
        }

        public TvdbSeries[] Search(string series)
        {
            return TvdbSeries.GetTvdbSeriesSearch(series);
        }
    }
}
