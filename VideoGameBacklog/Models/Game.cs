﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VideoGameBacklog.Models;

public partial class Game
{
    public int Id { get; set; }

    public int GameId { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();
}

public class GameApi
{
    public int id { get; set; }
    public Cover cover { get; set; }
    public Genre[] genres { get; set; }
    public Involved_Companies[] involved_companies { get; set; }
    public string name { get; set; }
    public Platform[] platforms { get; set; }
    public float rating { get; set; }
    public Release_Dates[] release_dates { get; set; }
    public string summary { get; set; }
}
public class Cover
{
    public int id { get; set; }
    public string url { get; set; }
}
public class Genre
{
    public int id { get; set; }
    public string name { get; set; }
}
public class Involved_Companies
{
    public int id { get; set; }
    public Company company { get; set; }
}
public class Company
{
    public int id { get; set; }
    public string name { get; set; }
}
public class Platform
{
    public int id { get; set; }
    public string name { get; set; }
}
public class Release_Dates
{
    public int id { get; set; }
    public string human { get; set; }
}
