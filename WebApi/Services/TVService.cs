using Common.DTOs;
using Common.Models;
using Newtonsoft.Json;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Movie_Verse.Services;

public class TVService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "e05a1c8af1f41b38b93a4e8c15514a98";
    private readonly string _baseUrl = "https://api.themoviedb.org/3";

    public TVService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TmdbTVShow>> GetPopularTVShowsAsync()
    {
        var url = $"{_baseUrl}/tv/popular?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching data: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<TMDBTVResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Results ?? new List<TmdbTVShow>();
    }


}