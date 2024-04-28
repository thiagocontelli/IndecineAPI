using Indecine.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Indecine.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    public class Result
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = string.Empty;
    }

    public class Root
    {
        [JsonPropertyName("results")]
        public List<Result> Results { get; set; } = [];
    }

    [HttpGet]
    public async Task<List<Movie>> GetMovies()
    {
        var httpClient = new HttpClient();

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.themoviedb.org/3/movie/popular");

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "");

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        var contentStream = await response.Content.ReadAsStreamAsync();

        var root = await JsonSerializer.DeserializeAsync<Root>(contentStream);

        return root.Results.Select(item => new Movie { Id = item.Id, Poster = $"https://image.tmdb.org/t/p/original/{item.PosterPath}" }).ToList();
    }
}
