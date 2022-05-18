using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Configuration;

namespace SongGenerator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Loading app settings on startup
            var (lower, upper, sleep, word, song) = LoadConfig();
        
            // possibility of future implementation of GUI Client
            var client = new ConsoleClient(lower, upper, sleep, word, song);
            await client.Run();
        }

        // function loads settings
        private static (int, int, int, string, string) LoadConfig()
        {
            int lower, upper, sleep;
            if (!int.TryParse(ConfigurationManager.AppSettings.Get("lowerBound"), out lower))
                lower = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings.Get("upperBound"), out upper))
                upper = 5;
            if (!int.TryParse(ConfigurationManager.AppSettings.Get("sleepTime"), out sleep))
                sleep = 1000;
            string word = ConfigurationManager.AppSettings.Get("randomWordApiLink");
            string song = ConfigurationManager.AppSettings.Get("songApiPrefix");
            return (lower, upper, sleep, word, song);
        }
    }
}
