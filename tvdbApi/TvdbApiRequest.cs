using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace tvdbApi
{
    public class TvdbApiMethods
    {
        public static string ApiKey { get; set; }
        public static string GetSeries(string seriesName)
        {
            return "GetSeries.php?seriesname=" + seriesName;
        }

        public static string Updates(uint time = 0)
        {
            return "Updates.php?type=" + ((time != 0) ? "series&time=" + time : "none");
        }

        public static string GetExtendedSeries(uint id)
        {
            return ApiKey + "/series/" + id + "/all/en.xml";
        }
    }

    public class TvdbApiRequest
    {
        public static bool UseCache { get; set; }
        private static string _cachePath = Assembly.GetExecutingAssembly().Location;
        public static string CachePath
        {
            get { return _cachePath; }
            set { _cachePath = value; }
        }

        private const string MirrorPath = "http://thetvdb.com/api/";
        private static string _userAgent = "TvdbRequestManager/2.1 (C#, part of TitleCleaner, like Gecko)";
        public static string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private static CookieContainer _cookieContainer = new CookieContainer();

        protected static Stream GetStreamForUrl(string url)
        {
            var webRequest = (HttpWebRequest) WebRequest.Create(MirrorPath + url);
            webRequest.UserAgent = UserAgent;
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate |
                                                DecompressionMethods.GZip |
                                                DecompressionMethods.None;
            webRequest.AllowAutoRedirect = true;
            webRequest.CookieContainer = _cookieContainer;

            var stream = new MemoryStream();
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                if (responseStream == null)
                {
                    throw new Exception("No data was returned from lookup.");
                }
                var reader = new StreamReader(responseStream);
                var text = reader.ReadToEnd();
                reader.Close();
                stream.Write(Encoding.UTF8.GetBytes(text), 0, text.Length);
            }
            _cookieContainer = webRequest.CookieContainer;
            stream.Position = 0;
            return stream;
        }

        protected static Stream GetStreamFromFile(string url)
        {
            return new FileStream(url, FileMode.Open);
        }

        public static T PerformApiRequestAndDeserialize<T>(string url)
        {
            var stream = PerformApiRequest(url);
            var ser = new XmlSerializer(typeof(T));
            var deserialised = (T)ser.Deserialize(stream);
            return deserialised;
        }

        public static Stream PerformApiRequest(string url)
        {
            
        }
    }
}
