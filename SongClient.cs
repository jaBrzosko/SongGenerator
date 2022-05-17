using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SongGenerator
{
    class SongClient
    {
        private readonly HttpClient client;
        public SongClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("SongRequest/0.0.1 (Warsaw)");
        }

        public async Task<(string, string)> Run(string name)
        {
            string url = $"https://musicbrainz.org/ws/2/release?query={name}&limit=1&fmt=json";
            var result = await client.GetStringAsync(url);
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
