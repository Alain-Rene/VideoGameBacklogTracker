using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPut("{id}")]
        public IActionResult UpdateProgressLog(int id, [FromBody] ProgressLog p)
        {
            if (p.LogId != id) { return BadRequest("Ids dont match"); }
            if (dbContext.ProgressLogs.Any(c => c.LogId == id) == false) { return NotFound("No matching ids"); }

            dbContext.ProgressLogs.Update(p);
            dbContext.SaveChanges();
            return Ok(p);
        }

        [HttpPost("/DTO")]
        public IActionResult AddDTOLog([FromBody] BackLogDTO newLog)
        {
            if (dbContext.ProgressLogs.Any(x => (x.GameId == newLog.GameId) && (x.UserId == newLog.UserId)))
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
                Game = await _vgbService.GetGameById((int)result.GameId),

            };
            return Ok(dto);
        }
        [HttpGet("/DTO/userID/{id}")]

        public async Task<IActionResult> GetDTOByUserId(int id)
        {
            List<GameApi> games = await _vgbService.GetGamesInBacklog(id);
            // Select statement is automatically converting ProgressLogs into DTOs
            List<ProgressLog> result = await dbContext.ProgressLogs.Where(l => l.UserId.HasValue && l.UserId.Value == id).ToListAsync();

            List<RetrieveBackLogDTO> gameList = result.Select(l => new RetrieveBackLogDTO {
                Status = l.Status,
                PlayTime = l.PlayTime,
                Game = games.FirstOrDefault(g => l.GameId.HasValue && g.id == l.GameId.Value),
            }).ToList();
            
            return Ok(gameList);
        }
        [HttpPut("/DTO/{id}")]
        public async Task<IActionResult> UpdateProgressLogDTO(int id, [FromBody] BackLogDTO updatedLog)
        {
            ProgressLog p = dbContext.ProgressLogs.FirstOrDefault(p => p.UserId == id && p.GameId == updatedLog.GameId );

            p.Status = updatedLog.Status;
            p.PlayTime = updatedLog.PlayTime;

            dbContext.ProgressLogs.Update(p);
            dbContext.SaveChanges();

            return Ok(p);
        }
        [HttpDelete()]
        public IActionResult DeleteLog(int id)
        {
            ProgressLog result = dbContext.ProgressLogs.FirstOrDefault(u => u.LogId == id);

            if(result == null) { return NotFound("This user cannot be found"); }
            else { dbContext.ProgressLogs.Remove(result); dbContext.SaveChanges(); return NoContent(); }
        }

    }
}