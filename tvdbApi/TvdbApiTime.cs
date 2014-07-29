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

        public TvdbApiTime TvdbServerTime()
        {
            return TvdbApiRequest.PerformApiRequestAndDeserialize<TvdbApiTime>(TvdbApiMethods.Updates());
        }



    }
}
