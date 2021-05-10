using DIO_ApiCatalogoDeJogos_Mongo.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> Get(int page, int recordsPerPage);
        Task<Game> Get(ObjectId id);
        Task<List<Game>> Get(string name, string producer);
        Task Create(Game game);
        Task Update(Game game);
        Task Delete(ObjectId id);

    }
}
