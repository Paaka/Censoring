using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            var censor = new Censor();
            Console.ReadLine();
        }
    }

    internal class Censor
    {
        public Censor()
        {
            var profanities = File.ReadAllText("profanities.txt");
     
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
