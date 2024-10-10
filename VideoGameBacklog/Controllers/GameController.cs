using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using VideoGameBacklog.Models;

namespace VideoGameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();

        private readonly VideoGameDetailsService _videoGameDetailsService;

        public GameController(VideoGameDetailsService videoGameDetailsService)
        {
            _videoGameDetailsService = videoGameDetailsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            List<GameApi> result = await _videoGameDetailsService.GetGames(0, 5);

            return Ok(result);
        }

        [HttpGet("/adventure")]
        public async Task<IActionResult> GetGamesAdventure()
        {
            List<GameApi> result = await _videoGameDetailsService.GetGamesAdventure(0, 5);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGamesById(int id)
        {
            GameApi result = await _videoGameDetailsService.GetGameById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody]GameApi game)
        {
            if(game == null)
            {
                return BadRequest("Could not find game.");
            }

            GameApi gameToAdd = await _videoGameDetailsService.GetGameById(game.id);
            if(gameToAdd != null)
            {
                gameToAdd.name = game.name;
                gameToAdd.platforms = game.platforms;
                gameToAdd.cover = game.cover;
                gameToAdd.involved_companies = game.involved_companies;
                gameToAdd.rating = game.rating;
                gameToAdd.summary = game.summary;
                gameToAdd.genres = game.genres;
                gameToAdd.release_dates = game.release_dates;
            }
            gameToAdd.id = 0;
            dbContext.Games.Add(gameToAdd);
            await dbContext.SaveChangesAsync();
            return Created($"games/{gameToAdd.id}", gameToAdd);
        }
    }
}