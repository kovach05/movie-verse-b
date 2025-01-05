using System.Text.Json.Serialization;

namespace Common.Models;

public class TmdbTVShowResult
{
    [JsonPropertyName("results")]
    public List<TmdbTVShow> Results { get; set; }
}