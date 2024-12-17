using System.Text.Json.Serialization;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; }
    
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
}