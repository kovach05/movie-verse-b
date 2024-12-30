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
    
    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        var url = $"{_baseUrl}/movie/{id}?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching movie details: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var movie = JsonSerializer.Deserialize<Movie>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return movie;
    }

    public async Task<string> GetMovieTrailerKeyAsync(int movieId)
    {
        var url = $"{_baseUrl}/movie/{movieId}/videos?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);
    
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching trailer: {response.StatusCode}");
        }
    
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TMDBVideoResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    
        // Перевірка на відсутність трейлерів
        var trailer = result?.Results?.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
        if (trailer == null)
        {
            throw new Exception("No trailers found for this movie.");
        }
    
        return trailer.Key;
    }

    public async Task<string> GetTrailerUrl(int movieId)
    {
        var url = $"{_baseUrl}/movie/{movieId}/videos?api_key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TMDBVideoResponse>();
            var trailer = data?.Results?.FirstOrDefault(v => v.Type == "Trailer");
            return trailer?.Key; // Return the trailer key
        }
        return null;
    }
    
    public async Task<TMDBCrewResponse> GetMovieCreditsAsync(int movieId)
    {
        var url = $"{_baseUrl}/movie/{movieId}/credits?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching credits: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TMDBCrewResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }


}