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
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new RandomWordClient();
            var results = await generator.GetRandomWord(5);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
