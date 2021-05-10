using DIO_ApiCatalogoDeJogos_Mongo.InputModel;
using DIO_ApiCatalogoDeJogos_Mongo.ViewModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Services
{
    public interface IGameService: IDisposable
    {
        Task<List<GameViewModel>> Get(int page, int recordsPerPage);
        Task<GameViewModel> Get(ObjectId id);
        Task<GameViewModel> Create(GameInputModel game);
        Task Update(ObjectId id, GameInputModel game);
        Task Update(ObjectId id, double price);
        Task Delete(ObjectId id);
    }
}
