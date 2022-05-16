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

        public async Task<string[]> GetRandomWord(int n)
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
            for (int i = 0; i < rawData.Length; i++)
            {
                rawData[i] = rawData[i].Trim(control);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(rawData[i]);
                rawData[i] = dict["word"];
            }
        }
    }
}
