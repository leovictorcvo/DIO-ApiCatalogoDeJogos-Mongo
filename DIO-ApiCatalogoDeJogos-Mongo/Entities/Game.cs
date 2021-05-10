using DIO_ApiCatalogoDeJogos_Mongo.InputModel;
using DIO_ApiCatalogoDeJogos_Mongo.ViewModel;

namespace DIO_ApiCatalogoDeJogos_Mongo.Entities
{
    public class Game
    {
        public Game()
        {

        }

        public Game(GameInputModel model)
        {
            this.Name = model.Name;
            this.Producer = model.Producer;
            this.Price = model.Price;
        }

        public MongoDB.Bson.ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public double Price { get; set; }

        public GameViewModel ToGameViewModel() => new()
        {
            Id = this.Id.ToString(),
            Name = this.Name,
            Producer = this.Producer,
            Price = this.Price
        };

        public void UpdateFromGameInputModel(GameInputModel game)
        {
            this.Name = game.Name;
            this.Producer = game.Producer;
            this.Price = game.Price;
        }
    }
}
