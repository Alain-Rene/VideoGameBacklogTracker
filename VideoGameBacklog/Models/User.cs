using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VideoGameBacklog.Models;

public partial class User
{
    public int Id { get; set; }

    public string GoogleId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Pfp { get; set; }

    public int? TotalXp { get; set; }

    public int? Level { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();

    [JsonIgnore]
    public virtual ICollection<User> Friends { get; set; } = new List<User>();

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
