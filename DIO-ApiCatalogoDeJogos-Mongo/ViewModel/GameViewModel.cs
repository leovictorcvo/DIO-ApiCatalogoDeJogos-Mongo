using System;

namespace DIO_ApiCatalogoDeJogos_Mongo.ViewModel
{
    public class GameViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public double  Price { get; set; }
    }
}
