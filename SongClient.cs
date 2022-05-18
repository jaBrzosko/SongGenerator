using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;

namespace SongGenerator
{
    class SongClient
    {
        private readonly HttpClient client;
        private readonly string _url;
        private readonly int sleepTime;
        public SongClient(string link, int sleep)
        {
            sleepTime = sleep;
            _url = link;
            client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("SongRequest/1.0.1 (Warsaw)");
        }

        public async Task<(string, string)[]> Run(string[] inputs)
        {
            var songInfo = new (string, string)[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                songInfo[i] = await GetOne(inputs[i]);
                Thread.Sleep(sleepTime);
            }
            return songInfo;
        }
        private async Task<(string, string)> GetOne(string name)
        {
            string url = $"{_url}{name}&limit=1&fmt=json";
            string result;
            try
            {
                 result = await client.GetStringAsync(url);
            }
            catch (Exception)
            {
                return (null, null);
            }
            Dictionary<string, object> dict = new Dictionary<string, object>();

            try
            {
                dict = JsonSerializer.Deserialize<Dictionary<string, object>>(result);
            }
            catch (Exception)
            {
                return (null, null);
            }
            if(dict["count"].ToString() == "0")
            {
                return (null, null);
            }
            try
            {
                var control = new char[] { '[', ']' };
                JsonElement releases = (JsonElement)dict["releases"];
                var rel = JsonSerializer.Deserialize<Dictionary<string, object>>((releases.ToString()).Trim(control));
                var artist = JsonSerializer.Deserialize<Dictionary<string, object>>((rel["artist-credit"].ToString()).Trim(control));
                return (rel["title"].ToString(), (string)artist["name"].ToString());
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
    }
}
