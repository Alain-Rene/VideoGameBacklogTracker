using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            // _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Authorization);
        }
        private const string ClientID = "ff0ca20shq346e24m52cp9ozizz6ua"; // Twitch client ID
        private const string Authorization = "ll77khnf1la36uuwzx8z7e0sfzsfjm"; // Twitch access token for IGDB API
        public async Task<List<GameApi>> GetGames(int offset, int limit = 5) //not sure if offset and limit are necessary. Limit is default 10 and prolly don't need to offset that. -DB
        {
            string endpoint = "games";

            string requestBody = "fields name, genres.name, summary, rating, " +
                     "platforms.name, franchise, involved_companies.company.name, cover.url; " +
                     $"sort rating desc; limit {limit}; offset {offset};";

            // string JsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            // HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
         
            // HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);


            // HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>(endpoint,requestBody);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };
            
            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);


            string jsonResponse = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponse);
            //List<GameApi> games = JsonSerializer.Deserialize<List<GameApi>>(await response.Content.ReadAsStringAsync());
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

            return games;
        }

        // public async Task<Game> GetFilteredGames()
        // {
        //     // Specifies the endpoint
        //     string endpoint = "games";

        //     string requestBody = "fields name, genres, summary, release_dates.date, " +
        //      "rating, platforms.name, franchise, involved_companies.company.name, cover.url";

        //     HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);

        //     requestMessage.Headers.Add("Client-ID", ClientID);
        //     requestMessage.Headers.Add("Authorization", "Bearer " + Authorization);

        //     List<string> filters = new List<string>();



        //     requestMessage.Content = new StringContent()
        // }
        public async Task<List<GameApi>> GetFilteredGames(int offset, int limit = 5, string? name = null, string? genre = null, int? rating = null, string? companyName = null, string? platform = null, string? releaseYear = null)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, rating, involved_companies.company.name, platforms.name,release_dates.human, cover.url; limit {limit}; offset {offset};";

            // string JsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            // HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);

            // HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
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
            if(rating.HasValue)
            {
                if(counter == 0)
                {
                    filters += $"rating >= {rating}";
                }
                else
                {
                    filters += $" & rating >= {rating}";
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
                    filters += $" & involved_companies.company.name = {companyID}";
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


            // HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>(endpoint,requestBody);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);


            string jsonResponse = await response.Content.ReadAsStringAsync();

            // System.Console.WriteLine(jsonResponse);
            //List<GameApi> games = JsonSerializer.Deserialize<List<GameApi>>(await response.Content.ReadAsStringAsync());
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

            string requestBody = $"fields name, genres.name, summary, rating, involved_companies.company.name, franchise, platforms.name, release_dates.human, cover.url; where id = {id};";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);



            string jsonResponse = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine(jsonResponse);
            //List<GameApi> games = JsonSerializer.Deserialize<List<GameApi>>(await response.Content.ReadAsStringAsync());
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

            return games.FirstOrDefault();
        }
        public async Task<List<GameApi>> GetGamesInBacklog(int id)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, rating, involved_companies.company.name, franchise, platforms.name,release_dates.human, cover.url;"; 

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

            // System.Console.WriteLine(jsonResponse);
            //List<GameApi> games = JsonSerializer.Deserialize<List<GameApi>>(await response.Content.ReadAsStringAsync());
            List<GameApi> games = await response.Content.ReadFromJsonAsync<List<GameApi>>();

            return games;
        }

        public async Task<int> GetGameId(GameApi game)
        {
            return game.id;
        }
        
    }
}