using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var results = await generator.GetRandomWord(input);
            PrintOuput(results);
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

        private void PrintOuput(string[] results)
        {
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
