using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Newtonsoft.Json;
using SearchingCurses;

using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

           
        }


        private void Button_Click(object sender, EventArgs e)
        {
      
            var userText = richTextBox1.Text;
            var uknownSong = new UserInput(userText);

            var eminemSwearStats = new Rapper("Eminem");
            eminemSwearStats.AddSong("stan");
            eminemSwearStats.AddSong("Lose Yourself");
            eminemSwearStats.AddSong("Venom");
            eminemSwearStats.AddSong("Lucky you");

            var twoPacStats = new Rapper("2pac");
            twoPacStats.AddSong("changes");
            twoPacStats.AddSong("Dear Mama");
            twoPacStats.AddSong("Hit Em Up");
            twoPacStats.AddSong("California Love");
            twoPacStats.AddSong("Ghetto Gospel");

            var huStats = new Rapper("Hollywood Undead");
            huStats.AddSong("undead");
            huStats.AddSong("Christmas in hollywood");
            huStats.AddSong("Comin' in Hot");
            huStats.AddSong("No. 5");
            huStats.AddSong("City");

            var snoopDoggStats = new Rapper("Snoop dogg");
            snoopDoggStats.AddSong("Bitch please");
            snoopDoggStats.AddSong("Who Am I");
            snoopDoggStats.AddSong("Vato");
            snoopDoggStats.AddSong("Gin and Juice");
            snoopDoggStats.AddSong("Lay Low");

            var DMXstats = new Rapper("DMX");
            DMXstats.AddSong("x gon give it to ya");
            DMXstats.AddSong("Party up");
            DMXstats.AddSong("Who we be");
            DMXstats.AddSong("No sunshine");
            DMXstats.AddSong("I miss you");


            var EminemScore = compareUserSongWithRappers(eminemSwearStats, uknownSong);
            var twoPacScore = compareUserSongWithRappers(twoPacStats, uknownSong);
            var hollywoodUndeadScore = compareUserSongWithRappers(huStats, uknownSong);
            var snoopDoggScore = compareUserSongWithRappers(snoopDoggStats, uknownSong);
            var DMXscore = compareUserSongWithRappers(DMXstats, uknownSong);

            List<MyData> myRappers = new List<MyData>();

            myRappers.Add(new MyData { score = EminemScore, rapper = "Eminem" });
            myRappers.Add(new MyData { score = twoPacScore, rapper = "2Pac" });
            myRappers.Add(new MyData { score = hollywoodUndeadScore, rapper = "Hollywood Undead" });
            myRappers.Add(new MyData { score = snoopDoggScore, rapper = "Snoop Dogg" });
            myRappers.Add(new MyData { score = DMXscore, rapper = "DMX" });

            var bubbleSortedList = czaryMary(myRappers);

            if(bubbleSortedList[4].score == 0)
            {
                label2.Text = "Without swears it's imposible to me what this song name is";
            }
            else
            {
                label2.Text = "That probably's " + bubbleSortedList[4].rapper + " (" + bubbleSortedList[4].score + ")";
                label3.Text = "2." + bubbleSortedList[3].rapper + " (" + bubbleSortedList[3].score + ")";
                label4.Text = "3." + bubbleSortedList[2].rapper + " (" + bubbleSortedList[2].score + ")";
                label5.Text = "4." + bubbleSortedList[1].rapper + " (" + bubbleSortedList[1].score + ")";
                label6.Text = "5." + bubbleSortedList[0].rapper + " (" + bubbleSortedList[0].score + ")";
            }
           
        }

        private static List<MyData> czaryMary(List<MyData> list)
        {
            int size = (list.Capacity) / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < (size - i); j++)
                {
                    if (list[j].score > list[j + 1].score)
                    {
                        int temp = list[j].score;
                        string idk = list[j].rapper;
                        list[j].score = list[j + 1].score;
                        list[j].rapper = list[j + 1].rapper;
                        list[j + 1].score = temp;
                        list[j + 1].rapper = idk;
                    }

                }
            }

            return list;
        }

        private static int compareUserSongWithRappers(Rapper eminemSwearStats, UserInput song)
        {

            int score = 0;
            foreach (var myWords in song.swearsFromUser)
            {

                if (eminemSwearStats.swears.ContainsKey(myWords.Key))
                {
                    score++;
                }
                /*else
                {
                    score--;
                }*/
            }
            return score;

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

                foreach (var rapper in rappers)
                {
                    var score = rapper.CompareWith(songSwearStats);
                    Console.WriteLine(rapper.name + ": " + score + " points");
                }
            }
        }

        public class SwearStats : Censor
        {
            public Dictionary<string, int> swears = new Dictionary<string, int>();
            public void AddSwearFrom(Song song)
            {
                foreach (var word in badWords)
                {
                    var occurances = song.CountOccurances(word);

                    if (occurances > 0)
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
                var json = WebCache.GetOrDownload(url);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    internal class MyData
    {
        public int score;
        public string rapper;
    }
}

