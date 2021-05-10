using DIO_ApiCatalogoDeJogos_Mongo.Exceptions;
using DIO_ApiCatalogoDeJogos_Mongo.InputModel;
using DIO_ApiCatalogoDeJogos_Mongo.Services;
using DIO_ApiCatalogoDeJogos_Mongo.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DIO_ApiCatalogoDeJogos_Mongo.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        /// <summary>
        /// Retorna uma lista contendo todos os jogos cadastrados de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Número da página desejada</param>
        /// <param name="quantidade">Quantidade de jogos para serem exibidos em cada página</param>
        /// <returns>Retorna status Ok e a lista dos jogos cadastrados</returns>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Não há jogos cadastrados</response>        
        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> Get([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var games = await gameService.Get(pagina, quantidade);
            if (games.Count == 0)
            {
                return NoContent();
            }

            return Ok(games);
        }

        /// <summary>
        /// Retorna um jogo baseado no id fornecido
        /// </summary>
        /// <param name="id">Identificador do jogo a ser pesquisado</param>
        /// <returns>Retorna status Ok e dados do jogo</returns>
        /// <response code="200">Retorna o jogo encontrado</response>
        /// <response code="204">Não há jogo cadastrado com o Id fornecido</response>        
        [HttpGet("{id}")]
        public async Task<ActionResult<GameViewModel>> Get(string id) 
        {
            if (!ObjectId.TryParse(id, out ObjectId gameId))
            {
                return NotFound("Jogo não encontrado");
            }

            var game = await gameService.Get(gameId);
            if (game == null)
            {
                return NotFound("Jogo não encontrado");
            }

            return Ok(game);
        }

        /// <summary>
        /// Cadastra um novo jogo
        /// </summary>
        /// <param name="game">Dados do jogo para cadastrar</param>
        /// <returns>Retorna status Created e dados do jogo</returns>
        /// <response code="201">Jogo criado com sucesso</response>
        /// <response code="422">Não foi possível cadastrar o jogo com os dados fornecidos</response>        
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> Create([FromBody] GameInputModel game)
        {
            try
            {
                var savedGame = await gameService.Create(game);
                return Ok(savedGame);
            }
            catch (GameAlreadyExistException)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        /// <summary>
        /// Atualiza os dados de um jogo já cadastrado
        /// </summary>
        /// <param name="id">Identificador do jogo a ser alterado</param>
        /// <param name="game">Novos dados do jogo</param>
        /// <returns>Retorna status Ok</returns>
        /// <response code="200">Jogo alterado com sucesso</response>
        /// <response code="404">Não foi encontrado o jogo com identificador fornecido</response>        
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] GameInputModel game)
        {
            if (!ObjectId.TryParse(id, out ObjectId gameId))
            {
                return NotFound("Jogo não encontrado");
            }

            try
            {
                await gameService.Update(gameId, game);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("Jogo não encontrado");
            }
        }

        /// <summary>
        /// Altera o valor de venda de um jogo
        /// </summary>
        /// <param name="id">Identificador do jogo a ser alterado</param>
        /// <param name="price">Novo valor de venda</param>
        /// <response code="200">Jogo alterado com sucesso</response>
        /// <response code="404">Não foi encontrado o jogo com identificador fornecido</response>        
        [HttpPatch("{id}/preco/{price:double}")]
        public async Task<ActionResult> UpdatePrice([FromRoute] string id, [FromRoute] double price)
        {
            if (!ObjectId.TryParse(id, out ObjectId gameId))
            {
                return NotFound("Jogo não encontrado");
            }

            try
            {
                await gameService.Update(gameId, price);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("Jogo não encontrado");
            }
        }

        /// <summary>
        /// Exclui um jogo cadastrado
        /// </summary>
        /// <param name="id">Identificador do jogo a ser excluído</param>
        /// <response code="200">Jogo excluído com sucesso</response>
        /// <response code="404">Não foi encontrado o jogo com identificador fornecido</response>        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId gameId))
            {
                return NotFound("Jogo não encontrado");
            }

            try
            {
                await gameService.Delete(gameId);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("Jogo não encontrado");
            }
        }

    }
}
