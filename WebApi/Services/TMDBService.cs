using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;

public class TMDBService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "e05a1c8af1f41b38b93a4e8c15514a98";
    private readonly string _baseUrl = "https://api.themoviedb.org/3";

    public TMDBService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Movie>> GetPopularMoviesAsync()
    {
        var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching data: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TMDBResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Results ?? new List<Movie>();
    }
}