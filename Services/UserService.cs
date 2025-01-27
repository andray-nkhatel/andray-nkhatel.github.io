// UserService.cs
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UserService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiBaseUrl"];
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<User>>($"{_baseUrl}api/admin/Users");
    }

    public async Task<User> GetUserAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<User>($"{_baseUrl}api/admin/Users/{id}");
    }

    public async Task UpdateUserAsync(string id, User user)
    {
        await _httpClient.PutAsJsonAsync($"{_baseUrl}api/admin/Users/{id}", user);
    }

    public async Task DeleteUserAsync(string id)
    {
        await _httpClient.DeleteAsync($"{_baseUrl}api/admin/Users/{id}");
    }

    public async Task<UserCountDto> GetUserCountAsync()
    {
        return await _httpClient.GetFromJsonAsync<UserCountDto>($"{_baseUrl}api/admin/Users/count");
    }
}

public class UserCountDto
{
    public int TotalUsers { get; set; }
    public Dictionary<string, int> UsersByRole { get; set; }
}

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    // Add other properties as needed
}