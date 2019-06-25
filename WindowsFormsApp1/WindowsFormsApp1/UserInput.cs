using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    internal class UserInput:Censor
    {
        private string userText;

        public Dictionary<string, int> swearsFromUser = new Dictionary<string, int>();
        public UserInput(string userText)
        {
            this.userText = userText;


            foreach (var word in badWords)
            {
                var times = CountOccurances(word, userText);

                if (times > 0)
                {
                    if (!swearsFromUser.ContainsKey(word))
                        swearsFromUser.Add(word, 0);
                    swearsFromUser[word] += times;
                }
            }
        }

        public int CountOccurances(string word, string lyrics)
        {
            var pattern = "\\b" + word + "\\b";
            return Regex.Matches(lyrics, pattern, RegexOptions.IgnoreCase).Count;

        }

       
    }
}