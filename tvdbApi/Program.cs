using System;
using System.Globalization;

namespace tvdbApi
{
    class Program
    {
        public static void Main()
        {
            var tvdb = new Tvdb("F9D98CE470B5ABAE");
            var series = tvdb.Search("NCIS");
            var sect = 0;
            foreach (var ser in series)
            {
                Console.WriteLine(sect++ + ".");
                Console.WriteLine("### " + ser.SeriesName + " ###");
                Console.WriteLine(ser.Overview);
                Console.WriteLine("---");
            }
            sect = int.Parse(Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture));
            var details = series[sect].GetDetailedInformation();
            Console.WriteLine(details.Episodes[0].Director);
        }
    }
}
