using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Services;
using VideoGameBacklog.DTOs;
using VideoGameBacklog.Models;

namespace Services
{
   
    public class UserService
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();
        VideoGameDetailsService _vgbService;

        public UserService(VideoGameBacklogDbContext context, VideoGameDetailsService vgbService)
        {
            dbContext = context;
            _vgbService = vgbService;
        }

        public async Task<int> UpdateUserExperience(int userId)
        {
            List<RetrieveBackLogDTO> completedGames = GetCompletedGames(userId);

            User user = dbContext.Users.Find(userId);

            if (user != null && completedGames.Count > 0)
            {
                int finalXP = 0;
                
                foreach (RetrieveBackLogDTO game in completedGames)
                {
                    int XP = 100;
                    
                    foreach (Genre g in game.Game.genres)
                    {
                        int genreBonus = 0;
                        switch (g.name)
                        {
                            case "Pinball": genreBonus += 10; break;
                            case "Adventure": genreBonus += 40; break;
                            case "Indie": genreBonus += 30; break;
                            case "Arcade": genreBonus += 25; break;
                            case "Visual Novel": genreBonus += 20; break;
                            case "Card & Board Game": genreBonus += 15; break;
                            case "MOBA": genreBonus += 45; break;
                            case "Point-and-click": genreBonus += 30; break;
                            case "Fighting": genreBonus += 35; break;
                            case "Shooter": genreBonus += 40; break;
                            case "Music": genreBonus += 25; break;
                            case "Platform": genreBonus += 30; break;
                            case "Puzzle": genreBonus += 25; break;
                            case "Racing": genreBonus += 20; break;
                            case "Real Time Strategy (RTS)": genreBonus += 50; break;
                            case "Role-playing (RPG)": genreBonus += 50; break;
                            case "Simulator": genreBonus += 40; break;
                            case "Sport": genreBonus += 25; break;
                            case "Strategy": genreBonus += 45; break;
                            case "Turn-based strategy (TBS)": genreBonus += 50; break;
                            case "Tactical": genreBonus += 40; break;
                            case "Hack and slash/Beat 'em up": genreBonus += 35; break;
                            case "Quiz/Trivia": genreBonus += 15; break;
                            default: break; // Optional: Handle unrecognized genres if needed
                        }
                        System.Console.WriteLine("This is the genre bonus: " + genreBonus);
                        XP += genreBonus;
                    }
                    

                    CompletionTime time = await _vgbService.GetTimeToBeat(game.Game.id);
                    int timeBonus = 0;
                    if(time != null && time.Normally > 0)
                    {
                        long hours = time.Normally / 3600;
                        if (hours >= 1 && hours <= 10)
                        {
                            timeBonus = 20;
                        }
                        else if(hours >= 11 && hours <= 50)
                        {
                            timeBonus = 35;
                        }
                        else if (hours >= 51 && hours <= 100)
                        {
                            timeBonus = 50;
                        }
                        else if(hours > 100)
                        {
                            timeBonus = 65;
                        }

                        XP += timeBonus;
                    }
                    else 
                    {
                        if(game.PlayTime >= 1 && game.PlayTime <= 20)
                        {
                            timeBonus = 20;
                        }
                        else if(game.PlayTime >= 21 && game.PlayTime <= 50)
                        {
                            timeBonus = 35;
                        }
                        else if(game.PlayTime >= 51 && game.PlayTime <= 100)
                        {
                            timeBonus = 50;
                        }
                        else if(game.PlayTime > 100)
                        {
                            timeBonus = 65;
                        }
                        XP += timeBonus;
                    }
                    finalXP += XP;
                }

                System.Console.WriteLine("This is the final EXP: " + finalXP);
                user.TotalXp = finalXP;
                int level = CalculateCurrentLevel(finalXP);
                user.Level = level;
                dbContext.SaveChanges();
                return finalXP;
            }
            return -1;
            
        }

    public int CalculateXpForLevel(int level)
    {

        return (int)(500 * Math.Pow(1.5, level - 1));
    
        
    }

    public int CalculateCurrentLevel(int totalXP)
    {
        if(totalXP == 0)
        {
            return 0;
        }
        int level = 1;
        while (totalXP >= CalculateXpForLevel(level ))
        {
            level++;
        }
        if (level != 1)
        {
            return level - 1;
        }
        else
        {
            return level;
        }
        
    }

    public List<RetrieveBackLogDTO> GetCompletedGames(int userId)
        {
            List<GameApi> games = _vgbService.GetGamesInBacklog(userId).Result; // Call your method to get games in backlog
            List<ProgressLog> result = dbContext.ProgressLogs
                .Where(l => l.UserId == userId && l.Status == "Complete")
                .ToList();

            List<RetrieveBackLogDTO> gameList = result.Select(l => new RetrieveBackLogDTO
            {
                Status = l.Status,
                PlayTime = l.PlayTime,
                Game = games.FirstOrDefault(g => g.id == l.GameId),
            }).ToList();

            return gameList;
        }
    }

}