using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tekst = "Programing is fucking awesome";
            var songAnalysys = new songAnalysys(band:"kazik", song: "12 groszy");
            var censor = new Censor();
            Console.WriteLine(censor.Fix(tekst));
            Console.ReadLine();
            
        }
    }

     class Censor
    {
        string[] badWords;
        public Censor()
        {
            var profanities = File.ReadAllText("profanities.txt");
            profanities = profanities.Replace("*", "");
            profanities = profanities.Replace("(", "");
            profanities = profanities.Replace(")", "");
            profanities = profanities.Replace("\"", "");

            badWords = profanities.Split(',');

            
        }

        public string Fix(string tekst)
        {
            foreach (var word in badWords)
            {
                tekst = RepalaceBadWords(tekst, word);
            }

            return tekst;
        }

        private static string RepalaceBadWords(string tekst, string word)
        {
            var pattern = "\\b" + word + "\\b";
             return Regex.Replace(tekst, pattern, "_____", RegexOptions.IgnoreCase);
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
