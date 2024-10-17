using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using VideoGameBacklog.Models;

namespace VideoGameBacklog.DTOs
{
    public class BackLogDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int GameId { get; set; }

        public string? Status { get; set; }

        public int? PlayTime { get; set; }
    }

    public class RetrieveBackLogDTO
    {
        public string? Status { get; set; }

        public int? PlayTime { get; set; }
        
        [JsonIgnore]
        public GameApi? Game {  get; set; }
    }
}
