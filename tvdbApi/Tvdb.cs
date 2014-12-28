using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Class to help with the usage of the various TVDB Api classes.
    /// Not strictly required but useful.
    /// </summary>
    public class Tvdb
    {
        /// <summary>
        /// Store the API key in an appropriate location
        /// </summary>
        protected string ApiKey
        {
            set { TvdbDetailedSeries.ApiKey = value; }
        }

        private TvdbApiTime _serverTime;

        /// <summary>
        /// Server time for API caching.
        /// </summary>
        public TvdbApiTime ServerTime
        {
            get
            {
                var path = Path.Combine(PersistentCacheLocation, "time.xml");
                if (_serverTime != null || !File.Exists(path)) return _serverTime;
                var reader = new StreamReader(path);
                var ser = new XmlSerializer(typeof(TvdbApiTime));
                _serverTime = (TvdbApiTime)ser.Deserialize(reader);
                reader.Close();
                return _serverTime;
            }
            protected set
            {
                _serverTime = value;
                if (CacheType == TvdbCacheType.None) return;
                var path = Path.Combine(PersistentCacheLocation, "time.xml");
                var writer = new StreamWriter(path);
                var ser = new XmlSerializer(typeof(TvdbApiTime));
                ser.Serialize(writer, value);
                writer.Close();
            }
        }

        /// <summary>
        /// Sets the type of cache that will be used for each consecutive lookup.
        /// </summary>
        public TvdbCacheType CacheType
        {
            get { return TvdbApiRequest.CacheType; }
            set { TvdbApiRequest.CacheType = value; }
        }

        /// <summary>
        /// Sets the persistent cache storage location. If this is not specified the
        /// persistent cache will be in the same directory as the executable.
        /// </summary>
        public string PersistentCacheLocation
        {
            get { return TvdbApiRequest.PersistentCacheLocation; }
            set { TvdbApiRequest.PersistentCacheLocation = value; }
        }

        /// <summary>
        /// Instantiates a new copy of the TVDB API helper class
        /// </summary>
        /// <param name="apiKey">API Key to use for lookups</param>
        /// <param name="cacheType">Type of cache to use. Defaults to memory and persistent.</param>
        public Tvdb(string apiKey, TvdbCacheType cacheType = TvdbCacheType.PersistentMemory)
        {
            // Set the key.
            ApiKey = apiKey;
            // Must have the server time as a part of the API spec.
            ServerTime = TvdbApiTime.TvdbServerTime(cacheType, ServerTime == null ? 0 : ServerTime.Time);
            // Set the type of cache that will be used.
            CacheType = cacheType;
        }

        /// <summary>
        /// Search for a series by name.
        /// </summary>
        /// <param name="series">Series to search for.</param>
        /// <returns>An array of undetailed series information. Null if failure.</returns>
        public TvdbSeries[] Search(string series)
        {
            return TvdbSeries.GetTvdbSeriesSearch(series);
        }

        /// <summary>
        /// Retreive a series by its TVDB id.
        /// </summary>
        /// <param name="id">ID to lookup</param>
        /// <returns>Detailed series information or null if failure.</returns>
        public TvdbDetailedSeries LookupId(uint id)
        {
            var index = ServerTime.Episodes.BinarySearch(id);
            return TvdbDetailedSeries.GetDetailedSeries(id, index >= 0 && index < ServerTime.Episodes.Count);
        }

        /// <summary>
        /// Retreive a specific episode of a tvdb series.
        /// WARNING:
        /// Will retreive all series information and episodes when used
        /// so that API usage is minimised. DO NOT USE if you plan to
        /// do this multiple times AND CacheType is set to None.
        /// </summary>
        /// <param name="id">Series TVDB id.</param>
        /// <param name="season">Season number.</param>
        /// <param name="episode">Episode number.</param>
        /// <returns>The episode or null if not found.</returns>
        public TvdbEpisode LookupEpisode(uint id, uint season, uint episode)
        {
            try
            {
                var series = LookupId(id);
                return series.GetEpisode(season, episode);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
