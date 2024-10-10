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

    [JsonIgnore]
    public virtual ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();
}
