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
    // class used to connecting to MusicBrainz API
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

        // main run entry point
        public async Task<(string, string)[]> Run(string[] inputs)
        {
            var songResponse = new Task<string>[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                // API calls are made asynchronously, but app still have to sleep after making a call
                songResponse[i] = MakeApiCall(inputs[i]);
                // MusicBrainz requests that you don't make more than one request per second, therefore
                // you have to artificially slow down program not to get blacklisted by said API
                Thread.Sleep(sleepTime);
            }
            // only after making all calls they are synchronized and parsed
            var results = await Task.WhenAll(songResponse);

            return ParseApiResponse(results);
        }

        //make single API call
        private Task<string> MakeApiCall(string name)
        {
            string url = $"{_url}{name}&limit=1&fmt=json";
            try
            {
                return client.GetStringAsync(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // parse MusicBrainz API response to expected string format
        private (string, string)[] ParseApiResponse(string[] responses)
        {
            var output = new (string, string)[responses.Length];
            for(int i = 0; i < responses.Length; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                try
                {
                    dict = JsonSerializer.Deserialize<Dictionary<string, object>>(responses[i]);
                }
                catch (Exception)
                {
                    output[i] = (null, null);
                    continue;
                }
                if (dict["count"].ToString() == "0")
                {
                    output[i] = (null, null);
                    continue;
                }
                try
                {
                    var control = new char[] { '[', ']' };
                    JsonElement releases = (JsonElement)dict["releases"];
                    var rel = JsonSerializer.Deserialize<Dictionary<string, object>>((releases.ToString()).Trim(control));
                    var artist = JsonSerializer.Deserialize<Dictionary<string, object>>((rel["artist-credit"].ToString()).Trim(control));
                    output[i] = (rel["title"].ToString(), (string)artist["name"].ToString());
                    continue;
                }
                catch (Exception)
                {
                    output[i] = (null, null);
                    continue;
                }
            }
            return output;
        }
    }
}
