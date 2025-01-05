using System.Text.Json.Serialization;

namespace Common.DTOs;

public class TVShow
{
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; }
    public double VoteAverage { get; set; }
}