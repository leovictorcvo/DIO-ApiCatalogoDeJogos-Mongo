using System;

namespace DIO_ApiCatalogoDeJogos_Mongo.Exceptions
{
    public class GameAlreadyExistException: Exception
    {
        public GameAlreadyExistException(): base("Esse jogo já está cadastrado")
        {

        }
    }
}
