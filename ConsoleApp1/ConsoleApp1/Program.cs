using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var eminemSwearStats = new SwearStats();
            var song = new song(band:"Eminem", songName: "Stan");
            eminemSwearStats.AddSwearFrom(song);
            var censor = new Censor();
            Console.WriteLine(censor.Fix(song.lyrics));
            Console.ReadLine();
            
        }
    }

    class SwearStats:Censor
    {
        Dictionary<string, int> swears = new Dictionary<string, int>();
        public void AddSwearFrom(song song)
        {
            foreach(var word in badWords)
            {
                var occurances = song.CountOccurances(word);
            }
        }
    }

    class song
    {
        public string title;
        public string artist;
        public string lyrics;
        public song(string band, string songName)
        {
            var browser = new WebClient();
            var url = "https://api.lyrics.ovh/v1/" + band + "/" + songName;
            var json = browser.DownloadString(url);
            var lyricsData = JsonConvert.DeserializeObject<LyricovhAnwser>(json);
            

            title = songName;
            artist = band;
            lyrics = lyricsData.lyrics;
           
        }

        public int CountOccurances(string word)
        {
            var pattern = "\\b" + word + "\\b";
            return Regex.Matches(lyrics, pattern, RegexOptions.IgnoreCase).Count;
           
        }

        
     }

    public class LyricovhAnwser
    {
        public string lyrics;
        public string error;
    }
}
