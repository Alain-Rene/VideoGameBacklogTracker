using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoGameBacklog.Models;

namespace Services
{
    public class VideoGameDetailsService
    {
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
        public async Task<List<GameApi>> GetGames(int offset, int limit = 5)
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
        public async Task<List<GameApi>> GetGamesAdventure(int offset, int limit = 5)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, rating, involved_companies.company.name, franchise, platforms.name,release_dates.human, cover.url; limit {limit}; offset {offset}; where genres.name = \"Adventure\";";

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

        public async Task<GameApi> GetGameById(int id)
        {
            string endpoint = "games";

            string requestBody = $"fields name, genres.name, summary, rating, involved_companies.company.name, franchise, platforms.name,release_dates.human, cover.url; where id = {id};";

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

        
    }
}