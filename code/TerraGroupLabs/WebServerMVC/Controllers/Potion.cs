using Microsoft.AspNetCore.Mvc;
using Models.APIRequester;
using Newtonsoft.Json;
//Namespace alias, we refer to the full namespace Models.Model.Potion and then the class name Potion
using PotionModels = Models.Model.Potion.Potion;

namespace WebServerMVC.Controllers;

public class Potion : Controller
{
    
    
    [BindProperty]
    public List<PotionModels> Potions { get; set; }

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
    
    private async Task<List<PotionModels>> GetPotionsAsync()
    {
        
        try
        {
            string jsonResponse = await _apiRequester.GetAsync("potion/AllPotions");
            Console.WriteLine(jsonResponse);
            Potions = JsonConvert.DeserializeObject<List<PotionModels>>(jsonResponse);
        }
        catch (Exception ex)
        {
            // Handle exception (log it, show error message, etc.)
            Console.WriteLine($"API call failed: {ex.Message}");
            Potions = new List<PotionModels>(); // Initialize to an empty list on error
        }
        
        return Potions;
    }
    
    
}