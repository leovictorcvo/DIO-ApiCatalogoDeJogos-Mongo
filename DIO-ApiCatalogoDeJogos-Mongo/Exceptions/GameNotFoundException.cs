using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Exceptions
{
    public class GameNotFoundException:Exception
    {
        public GameNotFoundException():base("Esse jogo não está cadastrado")
        {

        }
    }
}
