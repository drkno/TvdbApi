using System.Xml.Serialization;

namespace tvdbApi
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbSeries : TvdbSeriesCommon
    {
        protected TvdbSeries(){}

        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Data")]
        protected class SeriesSearch
        {
            [XmlArray("Series"), XmlArrayItem(ElementName = "Series", Type = typeof(TvdbSeries))]
            public TvdbSeries[] Series;
        }

        public static TvdbSeries[] GetTvdbSeriesSearch(string series, TvdbApiTime serverTime)
        {
            series = series.ToLower().Trim();
            var seriesSearch = TvdbApiRequest.PerformApiRequestAndDeserialize<SeriesSearch>(GetSeriesUrl(series));
            return seriesSearch.Series;
        }

        private static string GetSeriesUrl(string seriesName)
        {
            return "GetSeries.php?seriesname=" + seriesName;
        }

        public TvdbDetailedSeries GetDetailedInformation()
        {
            return TvdbDetailedSeries.GetDetailedSeries(Id);
        }
    }
}
