namespace Common.Models;

public class TMDBVideoResponse
{
    public int Id { get; set; }
    public List<VideoResult>? Results { get; set; }
}