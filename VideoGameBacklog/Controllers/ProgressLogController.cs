using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using VideoGameBacklog.DTOs;
using VideoGameBacklog.Models;

namespace VideoGameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressLogController : ControllerBase
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();
        //VideoGameDetailsService vgbService = new VideoGameDetailsService();
        private readonly VideoGameDetailsService _vgbService;
        public ProgressLogController(VideoGameDetailsService vgbService)
        {
            _vgbService = vgbService;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            List<ProgressLog> result = dbContext.ProgressLogs.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetLogsById(int id)
        {
            ProgressLog result = dbContext.ProgressLogs.FirstOrDefault(p => p.LogId == id);
            if(result == null)
            {
                return NotFound("This progress log cannot be found!");
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddLog([FromBody] ProgressLog newLog)
        {
            if(dbContext.ProgressLogs.Any(x => x.GameId == newLog.GameId))
            {
                return NoContent();
            }
            newLog.LogId = 0;
            dbContext.ProgressLogs.Add(newLog);
            dbContext.SaveChanges();
            return Created("Not implemented", newLog);
        }

        [HttpPost("/DTO")]
        public IActionResult AddDTOLog([FromBody] BackLogDTO newLog)
        {
            if (dbContext.ProgressLogs.Any(x => x.GameId == newLog.GameId))
            {
                return NoContent();
            }
            ProgressLog p = new ProgressLog
            {
                LogId = 0,
                UserId = newLog.UserId,
                GameId = newLog.GameId,
                Status = newLog.Status,
                PlayTime = newLog.PlayTime
            };
            dbContext.ProgressLogs.Add(p);
            dbContext.SaveChanges();
            return Created("Not implemented", p);
        }

        [HttpGet("/DTO/{id}")]
        public async Task<IActionResult> GetDTOLogById(int id)
        {
            ProgressLog result = dbContext.ProgressLogs.FirstOrDefault(p => p.LogId == id);
            if (result == null)
            {
                return NotFound("This progress log cannot be found!");
            }

            RetrieveBackLogDTO dto = new RetrieveBackLogDTO
            {
                Status = result.Status,
                PlayTime = result.PlayTime,
                //Game = await _vgbService.GetGameById((int)result.GameId),
                Game = await _vgbService.GetGameById(1942) //id for the Witcher 3. Uncomment ln 88 once we have some functionality.
            };
            return Ok(dto);
        }

    }
}