using System.Text.Json.Serialization;

namespace Common.Models;

public class TmdbTVShow
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
}