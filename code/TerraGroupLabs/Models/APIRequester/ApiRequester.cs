using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Models.APIRequester;

public class ApiRequester
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiRequester(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _baseUrl = configuration["ApiSettings:BaseUrl"];
        Console.WriteLine($"Base URL: {_baseUrl}");
        
    }

    public async Task<string> GetAsync(string endpoint)
    {
        Console.WriteLine($"Request for url: {_baseUrl}/{endpoint}");
        var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> PostAsync(string endpoint, string json)
    {
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> PutAsync(string endpoint, string json)
    {
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
}
