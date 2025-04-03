using Microsoft.AspNetCore.Mvc;
using Models.APIRequester;
using Newtonsoft.Json;

namespace WebServerMVC.Controllers;

public class Potion : Controller
{
    
    
    [BindProperty]
    public List<Potion> Potions { get; set; }

    private ApiRequester _apiRequester;
    
    public Potion()
    {
        // Load baseurl from JSON manually
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        _apiRequester = new ApiRequester(config);
    }
    
    
    // GET INDEX
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var potions =  await GetPotionsAsync();
        return View(potions);
    }
    
    private async Task<List<Potion>> GetPotionsAsync()
    {
        
        try
        {
            string jsonResponse = await _apiRequester.GetAsync("potion/AllPotions");
            Console.WriteLine(jsonResponse);
            Potions = JsonConvert.DeserializeObject<List<Potion>>(jsonResponse);
        }
        catch (Exception ex)
        {
            // Handle exception (log it, show error message, etc.)
            Console.WriteLine($"API call failed: {ex.Message}");
            Potions = new List<Potion>(); // Initialize to an empty list on error
        }
        
        return Potions;
    }
    
    
}