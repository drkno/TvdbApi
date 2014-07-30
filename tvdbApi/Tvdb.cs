namespace tvdbApi
{
    internal class Tvdb
    {
        protected string ApiKey
        {
            set { TvdbDetailedSeries.ApiKey = value; }
        }

        public TvdbApiTime ServerTime { get; protected set; }

        public Tvdb(string apiKey)
        {
            ApiKey = apiKey;
            ServerTime = TvdbApiTime.TvdbServerTime();
        }

        public TvdbSeries[] Search(string series)
        {
            return TvdbSeries.GetTvdbSeriesSearch(series, ServerTime);
        }
    }
}
