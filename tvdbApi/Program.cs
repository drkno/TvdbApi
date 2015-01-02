using System;
using System.IO;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

namespace tvdbApi
{
    class Program
    {
        public static void Main()
        {
            /*var stream = new FileStream(@"C:\Users\Matthew\Documents\Development\tvdbApi\tvdbApi\bin\Debug\detailed.xml", FileMode.Open);
            var seri = new XmlSerializer(typeof(TvdbDetailedSeries));
            var deserialized = (TvdbDetailedSeries)seri.Deserialize(stream);
            stream.Close();

            Console.WriteLine(deserialized);

            return;*/
            // API Key for this program and the program it was
            // designed to be used as a part of (TitleCleaner) ONLY
            // If you use this program, get your own API key.
            var tvdb = new Tvdb("F9D98CE470B5ABAE");

            var start = DateTime.Now;

            // Just a random test prog
            TvdbSeries[] series = tvdb.Search("top gear");
            int sect = 0;
            foreach (var ser in series)
            {
                Console.WriteLine(sect++ + ".");
                Console.WriteLine("### " + ser.SeriesName + " ###");
                Console.WriteLine(ser.Description);
                Console.WriteLine("---");
            }
            var details = series[0].GetDetailedInformation(ref tvdb._tvdbApiRequest);
            Console.WriteLine(string.Join(",", details.Episodes[0].Directors));
            Console.WriteLine(details.Episodes[0].EpisodeName);

            DateTime time1 = DateTime.Now;

            series = tvdb.Search("top gear");
            sect = 0;
            foreach (var ser in series)
            {
                Console.WriteLine(sect++ + ".");
                Console.WriteLine("### " + ser.SeriesName + " ###");
                Console.WriteLine(ser.Description);
                Console.WriteLine("---");
            }
            details = series[0].GetDetailedInformation(ref tvdb._tvdbApiRequest);
            Console.WriteLine(string.Join(",", details.Episodes[0].Directors));
            Console.WriteLine(details.Episodes[0].EpisodeName);

            DateTime time2 = DateTime.Now;

            Console.WriteLine("Lookup 1 [un-cached]: " + time1.Subtract(start).TotalSeconds + "sec.");
            Console.WriteLine("Lookup 2 [cached]   : " + time2.Subtract(time1).TotalSeconds + "sec.");
            Console.WriteLine("Total    [both]     : " + time2.Subtract(start).TotalSeconds + "sec.");
            Console.ReadKey();
        }
    }
}
