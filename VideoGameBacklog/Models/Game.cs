﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VideoGameBacklog.Models;

public class GameApi
{
    public int id { get; set; }
    public Cover cover { get; set; }
    public Genre[] genres { get; set; }
    public Involved_Companies[] involved_companies { get; set; }
    public string name { get; set; }
    public Platform[] platforms { get; set; }
    public double total_rating { get; set; }
    public Release_Dates[] release_dates { get; set; }
    public int[] similar_games { get; set; }
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

public class GameVideo
{
    public int id { get; set; }
    public int game { get; set; }
    public string name { get; set; }
    public string video_id { get; set; }
    public string checksum { get; set; }
}

public partial class CompletionTime
    {
        public long Id { get; set; }
        public long GameId { get; set; }
        public long Normally { get; set; }
        public long Count { get; set; }
    }