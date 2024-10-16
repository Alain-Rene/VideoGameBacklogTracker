using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VideoGameBacklog.Models;

namespace Services
{
    public class VideoGameDetailsService
    {
        VideoGameBacklogDbContext dbContext = new VideoGameBacklogDbContext();
        private readonly HttpClient _httpClient;
        public VideoGameDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _httpClient.DefaultRequestHeaders.Add("Client-ID", ClientID);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Authorization);
        }
        private const string ClientID = "ff0ca20shq346e24m52cp9ozizz6ua"; // Twitch client ID
        private const string Authorization = "ll77khnf1la36uuwzx8z7e0sfzsfjm"; // Twitch access token for IGDB API
        public async Task<List<GameApi>> GetGames(int offset, int limit = 10) //not sure if offset and limit are necessary. Limit is default 10 and prolly don't need to offset that. -DB
        {
            string endpoint = "games";

            string requestBody = "fields name, genres.name, summary, total_rating, " +
                     "platforms.name, franchise, involved_companies.company.name, cover.url, videos; " +
                     $"sort total_rating desc; where themes != (42) & total_rating_count >= 50; limit {limit}; offset {offset};";
            // string requestBody = "fields name, genres.name, summary, total_rating," +
            //          "platforms.name, release_dates.human, similar_games, involved_companies.company.name, cover.url;" +
            //          $"sort total_rating desc; limit {limit}; offset {offset}; where category = (0, 8, 9) & aggregated_rating != null & total_rating_count > 50;";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };
            
            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);


            string jsonResponse = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine();

            System.Console.WriteLine(jsonResponse);
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

           
            return games;
        }

<<<<<<< HEAD
        public async Task<List<GameApi>> GetFilteredGames(int offset, int limit = 10, string? name = null, string? genre = null, int? total_rating = null, string? companyName = null, string? platform = null, string? releaseYear = null)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, total_rating, involved_companies.company.name, platforms.name,release_dates.human, cover.url; where themes != (42) & total_rating_count >= 1; limit {limit}; offset {offset};";
=======
        public async Task<List<GameApi>> GetFilteredGames(int offset, int limit = 10, string? name = null, string? genre = null, int? rating = null, string? companyName = null, string? platform = null, string? releaseYear = null)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, total_rating, involved_companies.company.name, platforms.name,release_dates.human, cover.url; limit {limit}; offset {offset};";
>>>>>>> 95fde43bb46d2493bd3f961246fbc40b90ecddd0

            int counter = 0;
            string filters = "";
            if(!name.IsNullOrEmpty())
            {
                // Search for name
                requestBody += $" search \"{name}\";";
            }
            if(!genre.IsNullOrEmpty())
            {
                if(counter == 0)
                {
                    filters += $"genres.name ~ *\"{genre}\"* ";
                }
                else
                {
                    filters += $" & genres.name ~ *\"{genre}\"* ";
                }
                counter++;
            }
            if(total_rating.HasValue)
            {
                if(counter == 0)
                {
<<<<<<< HEAD
                    filters += $"rating >= {total_rating}";
                }
                else
                {
                    filters += $" & rating >= {total_rating}";
=======
                    filters += $"total_rating >= {rating}";
                }
                else
                {
                    filters += $" & total_rating >= {rating}";
>>>>>>> 95fde43bb46d2493bd3f961246fbc40b90ecddd0
                }
                counter++;
            }
            if(!companyName.IsNullOrEmpty())
            {
                string companyEndpoint = "companies";
                string companyBody = $"fields name; where name ~ \"{companyName}\";";
                HttpRequestMessage companyRequest = new HttpRequestMessage(HttpMethod.Post, companyEndpoint)
                {
                    Content = new StringContent(companyBody, Encoding.UTF8, "text/plain")
                };

                HttpResponseMessage companyResponse = await _httpClient.SendAsync(companyRequest);
                string jsonResponseCompany = await companyResponse.Content.ReadAsStringAsync();

                System.Console.WriteLine(jsonResponseCompany);
                List<Involved_Companies> id = await companyResponse.Content.ReadFromJsonAsync<List<Involved_Companies>>();

                int companyID = id.FirstOrDefault().id;
                if(counter == 0)
                {
                    filters += $"involved_companies.company = {companyID}";
                }
                else
                {
                    filters += $" & involved_companies.company = {companyID}";
                }
                counter++;
            }

            if(!platform.IsNullOrEmpty())
            {
                if(counter == 0)
                {
                    filters += $"platforms.name ~ *\"{platform}\"*";
                }
                else
                {
                    filters += $" & platforms.name ~ *\"{platform}\"*";
                }
                counter++;
            }

            if(!releaseYear.IsNullOrEmpty())
            {
                if(counter == 0)
                {
                    filters += $"release_dates.human ~ *\"{releaseYear}\"";
                }
                else
                {
                    filters += $" & release_dates.human ~ *\"{releaseYear}\"";
                }
                counter++;
            }

            if(counter > 0)
            {
                requestBody += $" where {filters};";
            }
            System.Console.WriteLine(filters);
            System.Console.WriteLine(requestBody);


            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);


            string jsonResponse = await response.Content.ReadAsStringAsync();

            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();
            //  if(!companyName.IsNullOrEmpty())
            // {
            //     List<GameApi> testgames = games.Where(game => game.involved_companies.Any(c => c.company.name.Contains(companyName))).ToList();
            //     return testgames;
            // }

            return games;
        }

        public async Task<GameApi> GetGameById(int id)
        {
            string endpoint = "games";

<<<<<<< HEAD
            string requestBody = $"fields name, genres.name, summary, total_rating, involved_companies.company.name, franchise, platforms.name, release_dates.human, cover.url; where id = {id};";
=======
            string requestBody = $"fields name, genres.name, summary, total_rating, similar_games, involved_companies.company.name, franchise, platforms.name, release_dates.human, cover.url; where id = {id};";
>>>>>>> 95fde43bb46d2493bd3f961246fbc40b90ecddd0

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);



            string jsonResponse = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponse);
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

            return games.FirstOrDefault();
        }
         public async Task<List<GameApi>> GetSimilarGamesById(int id)
        {
            string endpoint = "games";

            string requestBody = $"fields similar_games; where id = {id};";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponse);
            //List<GameApi> games = JsonSerializer.Deserialize<List<GameApi>>(await response.Content.ReadAsStringAsync());
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();
            
            int[] similarGameIDs = games.FirstOrDefault().similar_games;
            string similarGames = "";
            int counter = 1;
            foreach (int i in similarGameIDs)
            {
                similarGames += i.ToString();
                 if(counter <= similarGameIDs.Count())
                {
                    similarGames += ",";
                }
            }

            similarGames = similarGames.Substring(0, similarGames.Length - 1);
            System.Console.WriteLine(similarGames);

            string requestBodySimilar = $"fields name, genres.name, total_rating, cover.url; where id = ({similarGames});";
             HttpRequestMessage requestMessageSimilar = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBodySimilar, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage responseNew = await _httpClient.SendAsync(requestMessageSimilar);

             string jsonResponseNew = await responseNew.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponseNew);
            List<GameApi> allGames = await responseNew.Content.ReadFromJsonAsync<List<GameApi>>();

            return allGames;

        }
        public async Task<List<GameApi>> GetGamesInBacklog(int id)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, total_rating, involved_companies.company.name, franchise, platforms.name,release_dates.human, cover.url;"; 

            User u = dbContext.Users.FirstOrDefault(x => x.Id == id);

            List<ProgressLog> logs = dbContext.ProgressLogs.Where(x => x.UserId == u.Id).ToList();

            int counter = 1;
            string userGames = "";
            foreach(ProgressLog l in logs)
            {
                userGames += l.GameId;
                if(counter <= logs.Count())
                {
                    userGames += ",";
                }
                counter++;
            }

            System.Console.WriteLine(userGames);

            userGames = userGames.Substring(0, userGames.Length - 1);

            

            requestBody += $"where id = ({userGames});";

            System.Console.WriteLine(userGames);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);



            string jsonResponse = await response.Content.ReadAsStringAsync();

            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

            return games;
        }

        //public async Task<List<GameVideo>> GetGameVideosAsync(int[] videoIds)
        //{
        //    int counter = 1;
        //    string ids = "";
        //    foreach (int i in videoIds)
        //    {
        //        ids += i;
        //        if (counter <= videoIds.Count())
        //        {
        //            ids += ",";
        //        }
        //        counter++;
        //    }
        //    System.Console.WriteLine(ids);
        //    ids = ids.Substring(0, ids.Length - 1);
        //    string GameVideosEndpoint = "game_videos";
        //    string GameVideosBody = $"fields *; where id = (\"{ids}\");";

        //    HttpRequestMessage GameVideosRequest = new HttpRequestMessage(HttpMethod.Post, GameVideosEndpoint)
        //    {
        //        Content = new StringContent(GameVideosBody, Encoding.UTF8, "text/plain")
        //    };

        //    HttpResponseMessage GameVideosResponse = await _httpClient.SendAsync(GameVideosRequest);
        //    string jsonResponseGameVideos = await GameVideosResponse.Content.ReadAsStringAsync();

        //    System.Console.WriteLine(jsonResponseGameVideos);
        //    List<GameVideo> videos = await GameVideosResponse.Content.ReadFromJsonAsync<List<GameVideo>>();
        //    return videos;
        //}

        public async Task<List<GameVideo>> GetGameVideosAsync(int id)
        {
            string GameVideosEndpoint = "game_videos";
            string GameVideosBody = $"fields *; where game = ({id});";

            HttpRequestMessage GameVideosRequest = new HttpRequestMessage(HttpMethod.Post, GameVideosEndpoint)
            {
                Content = new StringContent(GameVideosBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage GameVideosResponse = await _httpClient.SendAsync(GameVideosRequest);
            string jsonResponseGameVideos = await GameVideosResponse.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponseGameVideos);
            List<GameVideo> videos = await GameVideosResponse.Content.ReadFromJsonAsync<List<GameVideo>>();
            return videos;
        }

    }
}