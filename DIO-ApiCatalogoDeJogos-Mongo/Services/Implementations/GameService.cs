using DIO_ApiCatalogoDeJogos_Mongo.Entities;
using DIO_ApiCatalogoDeJogos_Mongo.Exceptions;
using DIO_ApiCatalogoDeJogos_Mongo.InputModel;
using DIO_ApiCatalogoDeJogos_Mongo.Repositories;
using DIO_ApiCatalogoDeJogos_Mongo.ViewModel;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<GameViewModel> Create(GameInputModel game)
        {
            var gamesWithNameAndProducer = await gameRepository.Get(game.Name, game.Producer);

            if (gamesWithNameAndProducer.Count > 0)
                throw new GameAlreadyExistException();

            var newGame = new Game(game);

            await gameRepository.Create(newGame);

            return newGame.ToGameViewModel();
        }

        public async Task Delete(ObjectId id)
        {
            var savedGame = await gameRepository.Get(id);

            if (savedGame == null)
            {
                throw new GameNotFoundException();
            }

            await gameRepository.Delete(id);
        }

        public void Dispose()
        {
        }

        public async Task<List<GameViewModel>> Get(int page, int recordsPerPage)
        {
            var games = await gameRepository.Get(page, recordsPerPage);

            return games.Select(g => g.ToGameViewModel()).ToList();
        }

        public async Task<GameViewModel> Get(ObjectId id)
        {
            var game = await gameRepository.Get(id);

            return game?.ToGameViewModel();
        }

        public async Task Update(ObjectId id, GameInputModel game)
        {
            var savedGame = await gameRepository.Get(id);

            if (savedGame == null)
            {
                throw new GameNotFoundException();
            }

            savedGame.UpdateFromGameInputModel(game);

            await gameRepository.Update(savedGame);
        }

        public async Task Update(ObjectId id, double price)
        {
            var savedGame = await gameRepository.Get(id);

            if (savedGame == null)
            {
                throw new GameNotFoundException();
            }

            savedGame.Price = price;

            await gameRepository.Update(savedGame);
        }
    }
}
