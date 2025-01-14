using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Common.DTOs;
using Common.Models;

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
        var result = JsonSerializer.Deserialize<TMDBTVResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Results ?? new List<TmdbTVShow>();
    }

    public async Task<TmdbTVShow> GetTvShowByIdAsync(int id)
    {
        var url = $"{_baseUrl}/tv/{id}?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching TV show details: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var tvShow = JsonSerializer.Deserialize<TmdbTVShow>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return tvShow;
    }
    
    public async Task<string> GetTvShowTrailerAsync(int id)
    {
        var url = $"{_baseUrl}/tv/{id}/videos?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching TV show trailer: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TMDBVideoResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Знаходимо офіційний трейлер
        var trailer = result?.Results?.Find(video => video.Type == "Trailer" && video.Site == "YouTube");
        return trailer?.Key; // Повертаємо ключ для YouTube
    }

    public async Task<List<CastMember>> GetTvShowCreditsAsync(int id)
    {
        var url = $"{_baseUrl}/tv/{id}/credits?api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching TV show credits: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TMDbCreditsResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Cast ?? new List<CastMember>();
    }

}