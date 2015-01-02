using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Keep track of API time to check for updates.
    /// </summary>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Items")]
    public class TvdbApiTime
    {
        /// <summary>
        /// This class should not be instantiated.
        /// </summary>
        protected TvdbApiTime() { }

        /// <summary>
        /// Unix epoch time of this API update.
        /// </summary>
        [XmlElement(ElementName = "Time")]
        public uint Time { get; set; }

        /// <summary>
        /// Episodes that have changed since the specified epoch time.
        /// </summary>
        [XmlElement(ElementName = "Episode")]
        public List<uint> Episodes { get; set; }

        /// <summary>
        /// Series that have changed since the specified epoch time.
        /// </summary>
        [XmlElement(ElementName = "Series")]
        public List<uint> Series { get; set; }

        public static TvdbApiTime TvdbServerTime(TvdbApiRequest request, uint previousTime)
        {
            Debug.WriteLine("-> TvdbApiTime::TvdbServerTime request=\"" + request + "\" previousTime=\"" + previousTime + " Called");
            var ut = request.CacheProvider.CacheType == TvdbCacheType.None || previousTime == 0 ? UpdateType.Time : UpdateType.All;
            var st = request.PerformApiRequestAndDeserialize<TvdbApiTime>(GetUpdateUrl(ut, previousTime), string.Empty, true, true);
            if (st.Series != null) st.Series.Sort();
            if (st.Episodes != null) st.Episodes.Sort();
            return st;
        }

        private static string GetUpdateUrl(UpdateType type, uint previousTime)
        {
            Debug.WriteLine("-> TvdbApiTime::GetUpdateUrl type=\"" + type + "\" previousTime=\"" + previousTime + " Called");
            string t;
            switch (type)
            {
                case UpdateType.All: t = "all&time=" + previousTime; break;
                case UpdateType.Time: t = "none"; break;
                case UpdateType.Episode: t = "episode&time=" + previousTime; break;
                case UpdateType.Series: t = "series&time=" + previousTime; break;
                default: goto case UpdateType.All;
            }
            return "Updates.php?type=" + t;
        }

        private enum UpdateType
        {
            Time,
            All,
            Episode,
            Series
        }
    }
}
