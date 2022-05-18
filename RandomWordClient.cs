using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace SongGenerator
{
    // class is interface to connecting to random word api
    class RandomWordClient
    {
        private readonly string _url;
        private readonly HttpClient client;
        public RandomWordClient(string link)
        {
            client = new HttpClient();
            _url = link;
        }

        // function is main controll for the class
        public async Task<string[]> GetRandomDistinctWord(int n)
        {
            int count = n;
            List<string> words = new List<string>();
            while(count > 0)
            {
                var results = await GetRandomWord(count);
                // we eliminate repeating words or API miss callbacks
                var trimmed = GetOnlyDifferentWords(results, words);
                // count controlls how many words were not yet succesfully found
                count -= trimmed.Count;
                words.AddRange(trimmed);
            }
            return words.ToArray();
        }

        // returns string array where words are valid API call responses and do not repeat
        private List<string> GetOnlyDifferentWords(string[] table, List<string> existing)
        {
            List<string> words = new List<string>();
            foreach (string word in table)
            {
                if (words.Contains(word) || word.StartsWith("\"") || existing.Contains(word))
                    continue;
                words.Add(word);
            }
            return words;
        }

        // returns n random words from API calls
        private async Task<string[]> GetRandomWord(int n)
        {
            Task<string>[] taskList = new Task<string>[n];
            for(int i = 0; i < n; i++)
            {
                taskList[i] = client.GetStringAsync(_url);
            }
            var results = await Task.WhenAll(taskList);
            ExtractOnlyWord(ref results);
            return results;
        }

        // manages API calls to extract words and ommit pronounciation and definition
        private void ExtractOnlyWord(ref string[] rawData)
        {
            var control = new char[] { '[', ']' };
            Dictionary<string, string> dict = new Dictionary<string, string>(); 
            for (int i = 0; i < rawData.Length; i++)
            {
                rawData[i] = rawData[i].Trim(control);
                try
                {
                    dict = JsonSerializer.Deserialize<Dictionary<string, string>>(rawData[i]);
                }
                catch (Exception)
                {
                    continue;
                }
                rawData[i] = dict["word"];
                dict.Clear();
            }
        }
    }
}
