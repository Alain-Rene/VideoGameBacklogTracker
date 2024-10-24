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
            List<GameApi> result = await _videoGameDetailsService.GetGames(0, 10);

            return Ok(result);
        }
        [HttpGet("/test")]
        public async Task<IActionResult> GetTest()
        {
            CompletionTime result = await _videoGameDetailsService.GetTimeToBeat(94873);

            return Ok(result);
        }

        [HttpGet("/filter")]
        public async Task<IActionResult> GetFilteredGames(string? name = null, string? genre = null, int? rating = null, string? companyName = null, string? platform = null, string? releaseYear = null)
        {
            List<GameApi> result = await _videoGameDetailsService.GetFilteredGames(0, 10, name, genre, rating, companyName, platform, releaseYear);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGamesById(int id)
        {
            GameApi result = await _videoGameDetailsService.GetGameById(id);
            return Ok(result);
        }
        [HttpGet("/similar/{id}")]
        public async Task<IActionResult> GetSimilarGames(int id)
        {
            List<GameApi> result = await _videoGameDetailsService.GetSimilarGamesById(id);

            return Ok(result);
        }

        [HttpGet("backlog/{id}")]
        public async Task<IActionResult> GetBacklogGames(int id)
        {

            List<GameApi> result = await _videoGameDetailsService.GetGamesInBacklog(id);
            return Ok(result);
        }

        [HttpGet("game_videos/{id}")]
        public async Task<IActionResult> GetVideos(int id)
        {
            List<GameVideo> result = await _videoGameDetailsService.GetGameVideosAsync(id);
            return Ok(result);
        }
    }
}