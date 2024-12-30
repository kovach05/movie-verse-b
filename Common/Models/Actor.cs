using System.Text.Json.Serialization;

namespace Common.Models;

public class Actor
{
    public string Name { get; set; }
    public string Character { get; set; }

    [JsonPropertyName("profile_path")]
    public string ProfilePath { get; set; }
}