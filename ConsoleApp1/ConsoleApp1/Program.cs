using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var songAnalysys = new songAnalysys(band:"kazik", song: "12 groszy");
            Console.ReadLine();
        }
    }

    class songAnalysys
    {
        public songAnalysys(string band, string song)
        {
            var browser = new WebClient();
            var url = "https://api.lyrics.ovh/v1/" + band + "/" + song;
            var json = browser.DownloadString(url);
            var lyrics = JsonConvert.DeserializeObject<LyricovhAnwser>(json);
            Console.WriteLine(lyrics.lyrics);
           
        }
    }

    public class LyricovhAnwser
    {
        public string lyrics;
        public string error;
    }
}
