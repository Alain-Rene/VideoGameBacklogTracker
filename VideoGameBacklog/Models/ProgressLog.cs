using System;
using System.Collections.Generic;

namespace VideoGameBacklog.Models;

public partial class ProgressLog
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public int? GameId { get; set; }

    public string? Status { get; set; }

    public int? PlayTime { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
