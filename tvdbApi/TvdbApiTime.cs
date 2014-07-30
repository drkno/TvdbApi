using System.Xml.Serialization;

namespace tvdbApi
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Items")]
    public class TvdbApiTime
    {
        protected TvdbApiTime() { }

        [XmlElement("Time")]
        public uint Time { get; set; }
        [XmlArray("Series"), XmlArrayItem(ElementName = "Series", Type = typeof(uint))]
        public uint[] Series { get; set; }

        public static TvdbApiTime TvdbServerTime()
        {
            return TvdbApiRequest.PerformApiRequestAndDeserialize<TvdbApiTime>(GetUpdateUrl());
        }

        private static string GetUpdateUrl()
        {
            return "Updates.php?type=none";
        }
    }
}
