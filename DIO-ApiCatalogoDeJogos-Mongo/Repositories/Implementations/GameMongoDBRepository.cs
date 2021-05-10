using DIO_ApiCatalogoDeJogos_Mongo.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Repositories.Implementations
{
    public class GameMongoDBRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> gamesCollection;
        public GameMongoDBRepository(IConfiguration configuration)
        {

            var connection = new MongoClient(configuration.GetConnectionString("Default"));
            var database = connection.GetDatabase("test");
            MapClasses();
            gamesCollection = database.GetCollection<Game>(nameof(Game).ToLower());
        }

        private void MapClasses()
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            if (!BsonClassMap.IsClassMapRegistered(typeof(Game)))
            {
                BsonClassMap.RegisterClassMap<Game>(i =>
                {
                    i.AutoMap();
                    i.MapIdMember(c => c.Id);
                    i.SetIgnoreExtraElements(true);
                });
            }
        }

        public async Task Create(Game game)
        {
            await gamesCollection.InsertOneAsync(game);
        }

        public async Task Delete(ObjectId id)
        {
            await gamesCollection.DeleteOneAsync(g => g.Id == id);
        }

        public async Task<List<Game>> Get(int page, int recordsPerPage)
        {
            var games = new List<Game>();

            await gamesCollection
                .Find(FilterDefinition<Game>.Empty)
                .Sort("{_id: 1}")
                .Skip(page > 0 ? ((page - 1) * recordsPerPage) : 0)
                .Limit(recordsPerPage)
                .ForEachAsync(game => games.Add(game));

            return games;
        }

        public async Task<Game> Get(ObjectId id)
        {
            return await gamesCollection.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Game>> Get(string name, string producer)
        {
            var games = new List<Game>();

            await gamesCollection.Find(g => g.Name == name && g.Producer == producer).ForEachAsync(g => games.Add(g));

            return games;
        }

        public async Task Update(Game game)
        {
            await gamesCollection.FindOneAndReplaceAsync(g => g.Id == game.Id, game);
        }

    }
}
