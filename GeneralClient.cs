using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongGenerator
{
    class GeneralClient
    {
        private RandomWordClient _randomWordClient;
        private SongClient _songClient;
        public GeneralClient(string wordLink, string songLink, int sleep)
        {
            _randomWordClient = new RandomWordClient(wordLink);
            _songClient = new SongClient(songLink, sleep);
        }
        public async Task<(string[], (string, string)[])> Run(int n)
        {
            var words = (await _randomWordClient.GetRandomDistinctWord(n)).OrderBy(x => x).ToArray();
            
            var songs = await _songClient.Run(words);

            return (words, songs);
        }
    }
}
