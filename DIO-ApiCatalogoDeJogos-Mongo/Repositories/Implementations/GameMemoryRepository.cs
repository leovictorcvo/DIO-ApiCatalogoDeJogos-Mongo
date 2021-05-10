using DIO_ApiCatalogoDeJogos_Mongo.Entities;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Repositories.Implementations
{
    public class GameMemoryRepository : IGameRepository
    {
        private static readonly Dictionary<MongoDB.Bson.ObjectId, Game> games = new()
        {
            { ObjectId.Parse("58469c732adc9f5370e50c9c"), new Game { Id = ObjectId.Parse("58469c732adc9f5370e50c9c"), Name = "Fifa 21", Producer = "EA", Price = 200 } },
            { ObjectId.Parse("58469c732adc9f5370e50c9d"), new Game { Id = ObjectId.Parse("58469c732adc9f5370e50c9d"), Name = "Fifa 20", Producer = "EA", Price = 199 } },
        };

        public Task Create(Game game)
        {
            games.Add(game.Id, game);
            return Task.CompletedTask;
        }

        public Task Delete(ObjectId id)
        {
            games.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Close database connection
        }

        public Task<List<Game>> Get(int page, int recordsPerPage)
        {
            return Task.FromResult(games.Values.Skip((page - 1) * recordsPerPage).Take(recordsPerPage).ToList());
        }

        public Task<Game> Get(ObjectId id)
        {
            if (!games.ContainsKey(id))
                return Task.FromResult<Game>(null);

            return Task.FromResult(games[id]);
        }

        public Task<List<Game>> Get(string name, string producer)
        {
            return Task.FromResult(games.Values.Where(g => g.Name.Equals(name) && g.Producer.Equals(producer)).ToList());
        }

        public Task Update(Game game)
        {
            games[game.Id] = game;
            return Task.CompletedTask;
        }
    }
}
