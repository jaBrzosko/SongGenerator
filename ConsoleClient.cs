using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SongGenerator
{
    class ConsoleClient
    {
        private readonly int lower;
        private readonly int upper;
        private readonly GeneralClient client;
        public ConsoleClient(int lowerBoundary, int upperBoundary, int sleep, string wordLink, string songLink)
        {
            lower = lowerBoundary;
            upper = upperBoundary;
            client = new GeneralClient(wordLink, songLink, sleep);
        }

        public async Task Run()
        {
            int input = ManageInput();
            var (words, songs) = await client.Run(input);
            
            PrintOuput(words, songs);
        }

        private int ManageInput()
        {
            Console.Write("Number of words to return: ");
            while (true)
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

        private void PrintOuput(string[] results, (string title, string artist)[] songInfo)
        {
            for(int i = 0; i < results.Length; i++)
            {
                Console.WriteLine();
                Console.Write("Random word is: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(results[i]);
                if (songInfo[i].artist != null)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Found title ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(songInfo[i].title);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" mady by ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(songInfo[i].artist);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No recording found!");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
