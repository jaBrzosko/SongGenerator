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
        public ConsoleClient(int lowerBoundary, int upperBoundary)
        {
            lower = lowerBoundary;
            upper = upperBoundary;
        }

        public async Task Run()
        {
            int input = ManageInput();
            var generator = new RandomWordClient();
            var results = (await generator.GetRandomWord(input)).OrderBy(x => x).ToArray();
            

            var songRequest = new SongClient();
            var songInfo = new (string, string)[results.Length];
            for(int i = 0; i < results.Length; i++)
            {
                songInfo[i] = await songRequest.Run(results[i]);
                Thread.Sleep(1000);
            }
            PrintOuput(results, songInfo);
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
                Console.Write("Random word is: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(results[i]);
                if (songInfo[i].artist != null)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Found title ");
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
                    Console.WriteLine(" No recording found!");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
