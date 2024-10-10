using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoGameBacklog.Models;

namespace VideoGameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressLogController : ControllerBase
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();

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

    
    }
}