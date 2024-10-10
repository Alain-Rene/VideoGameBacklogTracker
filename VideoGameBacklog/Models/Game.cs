using System;
using System.Collections.Generic;

namespace VideoGameBacklog.Models;

//public partial class Game
//{
//    public int GameId { get; set; }

//    public string? Title { get; set; }

//    public string? Genre { get; set; }

//    public string? Description { get; set; }

//    public DateOnly? ReleaseDate { get; set; }

//    public decimal? Rating { get; set; }

//    public string? Platform { get; set; }

//    public string? Franchise { get; set; }

//    public string? InvolvedCompanies { get; set; }

//    public string? ImageUrl { get; set; }

//    public int? HltbmainTime { get; set; }

//    public int? Hltbextras { get; set; }

//    public int? Hltbcompletionist { get; set; }

//    public virtual ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();
//}


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