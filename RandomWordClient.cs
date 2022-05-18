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
    class RandomWordClient
    {
        private readonly HttpClient client;
        public RandomWordClient()
        {
            client = new HttpClient();
        }

        public async Task<string[]> GetRandomDistinctWord(int n)
        {
            int count = n;
            List<string> words = new List<string>();
            while(count > 0)
            {
                var results = await GetRandomWord(count);
                var trimmed = GetOnlyDifferentWords(results);
                count -= trimmed.Count;
                words.AddRange(trimmed);
            }
            return words.ToArray();
        }

        private List<string> GetOnlyDifferentWords(string[] table)
        {
            List<string> words = new List<string>();
            foreach (string word in table)
            {
                if (words.Contains(word) || word.StartsWith("\""))
                    continue;
                words.Add(word);
            }
            return words;
        }

        private async Task<string[]> GetRandomWord(int n)
        {
            Task<string>[] taskList = new Task<string>[n];
            for(int i = 0; i < n; i++)
            {
                taskList[i] = client.GetStringAsync("https://random-words-api.vercel.app/word");
            }
            var results = await Task.WhenAll(taskList);
            ExtractOnlyWord(ref results);
            return results;
        }
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
                catch (Exception _)
                {
                    continue;
                }
                rawData[i] = dict["word"];
                dict.Clear();
            }
        }
    }
}
