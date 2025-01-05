namespace Common.Models;

public class TMDBTVResponse
{
    public int Page { get; set; }
    public List<TmdbTVShow> Results { get; set; }
}