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
    }
}