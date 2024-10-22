using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoGameBacklog.DTOs;
using VideoGameBacklog.Models;
using Services;
using Microsoft.EntityFrameworkCore;

namespace VideoGameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            List<User> result = dbContext.Users.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            User result = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (result == null)
            {
                return NotFound("This user cannot be found! ");
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User u)
        {
            if(dbContext.Users.Any(x => x.GoogleId == u.GoogleId))
            {
                return NoContent();
            }
            u.Id = 0;
            dbContext.Users.Add(u);
            dbContext.SaveChanges();
            return Created("Not implemented", u);
        }

        [HttpDelete()]
        public IActionResult DeleteUser(int id)
        {
            User result = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if(result == null) { return NotFound("This user cannot be found"); }
            else { dbContext.Users.Remove(result); dbContext.SaveChanges(); return NoContent(); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (user.Id != id) { return BadRequest("Ids dont match"); }
            if (dbContext.Users.Any(c => c.Id == id) == false) { return NotFound("No matching ids"); }

            dbContext.Users.Update(user);
            dbContext.SaveChanges();
            return Ok(user);
        }

        [HttpPut("/XP/{id}")]
        public async Task<IActionResult> UpdateXP(int id)
        {
            int result = await _userService.UpdateUserExperience(id);

            return Ok(result);
        }

        [HttpGet("/Level/{id}")]
        public async Task<IActionResult> GetLevel(int id)
        {
            User result = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (result == null)
            {
                return NotFound("This user cannot be found! ");
            }

            int currentLevel = _userService.CalculateCurrentLevel((int)result.TotalXp);

            return Ok(currentLevel);
        }

        [HttpGet("/friends/{id}")]
        public async Task<IActionResult> GetFriends(int id)
        {
            User result = dbContext.Users.Include(u => u.Friends).FirstOrDefault(u => u.Id == id);

            if(result == null)
            {
                return NotFound("This user cannot be found!");
            }

            return Ok(result.Friends);
        }

        [HttpPost("/friends/{userId}/{friendId}")]
        public async Task<IActionResult> AddFriend(int userId, int friendId)
        {
            if (userId == friendId)
            {
                return BadRequest("You cannot add yourself to your friendslist, weirdo");
            }

            User result = dbContext.Users.Include(u => u.Friends).FirstOrDefault(u => u.Id == userId);

            // Incomplete method
            return Ok("Friend added");
        }
    }
}