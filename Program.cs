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
            int lower = 5, upper = 20;
            int input = ManageInput(lower, upper);
            var generator = new RandomWordClient();
            var results = await generator.GetRandomWord(input);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static int ManageInput(int lower, int upper)
        {
            Console.Write("Number of words to return: ");
            while(true)
            {
                if (!int.TryParse(Console.ReadLine(), out var input))
                {
                    Console.Write("Please, write a number: ");
                    continue;
                }
                if (input <= 0)
                {
                    Console.Write("Number must be positive: ");
                    continue;
                }
                if (input < lower)
                {
                    Console.Write($"Number must be greater than or equal to {lower}: ");
                    continue;
                }
                if (input > upper)
                {
                    Console.Write($"Number must be less than or equal to {upper}: ");
                    continue;
                }
                return input;
            }
        }
    }
}
