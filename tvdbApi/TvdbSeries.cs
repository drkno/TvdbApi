using System;
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

        public static TvdbSeries[] GetTvdbSeriesSearch(string series)
        {
            series = series.ToLower().Trim();
            var ser = new XmlSerializer(typeof(SeriesSearch));
            var stream = TvdbApiRequest.PerformApiRequest(TvdbApiMethods.GetSeries(series));
            if (stream == null) throw new NullReferenceException("Series search request returned nothing.");
            var seriesSearch = (SeriesSearch)ser.Deserialize(stream);
            stream.Close();
            return seriesSearch.Series;
        }

        public TvdbDetailedSeries GetDetailedInformation()
        {
            return TvdbDetailedSeries.GetDetailedSeries(Id);
        }
    }
}
