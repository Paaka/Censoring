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
            var eminemSwearStats = new Rapper("Hollywood undead");
            eminemSwearStats.AddSong("undead");

            var twoPacStats = new Rapper("2pac");
            twoPacStats.AddSong("changes");

            var rappers = new List<Rapper>();
            rappers.Add(eminemSwearStats);
            rappers.Add(twoPacStats);

            var unknownSong = new Song(band: "Hollywood undead", songName: "Christmas in Hollywood");

            var tinder = new RapperTinder(rappers, unknownSong);
            Console.ReadKey();
        }
    }

    internal class RapperTinder
    {
        List<Rapper> rappers;
        Song unknownSong;

        public RapperTinder(List<Rapper> rappers, Song unknownSong)
        {
            this.rappers = rappers;
            this.unknownSong = unknownSong;

            var songSwearStats = new SwearStats();
            songSwearStats.AddSwearFrom(unknownSong);

            foreach(var rapper in rappers)
            {
                var score = rapper.CompareWith(songSwearStats);
                Console.WriteLine(rapper.name + ": " + score + " points");
            }
        }
    }

    public class SwearStats:Censor
    {
        Dictionary<string, int> swears = new Dictionary<string, int>();
        public void AddSwearFrom(Song song)
        {
            foreach(var word in badWords)
            {
                var occurances = song.CountOccurances(word);
                if(occurances > 0)
                {
                    if (!swears.ContainsKey(word))
                        swears.Add(word, 0);
                    swears[word] += occurances;
                }
                
            }
        }

        public void ShowSummary()
        {
            foreach (var item in swears)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
        }

        public int CompareWith(SwearStats eminemSwearStats)
        {
            int score = 0;
            foreach (var myWords in swears)
            {
                if (eminemSwearStats.swears.ContainsKey(myWords.Key))
                {
                    score++;
                }
                else
                {
                    score--;
                }
            }
            return score;
        }
    }

    public class Song
    {
        public string title;
        public string artist;
        public string lyrics;
        public Song(string band, string songName)
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

    public class Rapper : SwearStats
    {
        public string name;

        public Rapper(string name)
        {
            this.name = name;
        }

        public void AddSong(string title)
        {
            var song = new Song(band: name, songName: title);
            AddSwearFrom(song);
        }
    }

    public class LyricovhAnwser
    {
        public string lyrics;
        public string error;
    }
}
