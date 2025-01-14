using System.Text.Json.Serialization;

namespace Common.Models;

public class CastMember
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Character { get; set; }
    
    [JsonPropertyName("poster_path")]
    public string ProfilePath { get; set; }
}