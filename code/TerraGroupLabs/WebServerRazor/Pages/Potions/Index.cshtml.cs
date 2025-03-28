using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Models.APIRequester;
using Models.Model;
using Newtonsoft.Json;

namespace WebServerRazor.Pages.Potions;

public class Index : PageModel
{
    [BindProperty]
    public List<Potion> Potions { get; set; }

    private ApiRequester _apiRequester;

    public Index()
    {
        // Load baseurl from JSON manually
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        _apiRequester = new ApiRequester(config);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        //Potions = await _apiRequester.GetAsync<List<Potion>>("api/potion/AllPotions");
        
        try
        {
            string jsonResponse = await _apiRequester.GetAsync("api/potion/AllPotions");
            Console.WriteLine(jsonResponse);
            Potions = JsonConvert.DeserializeObject<List<Potion>>(jsonResponse);
        }
        catch (Exception ex)
        {
            // Handle exception (log it, show error message, etc.)
            Console.WriteLine($"API call failed: {ex.Message}");
            Potions = new List<Potion>(); // Initialize to an empty list on error
        }
        
        return Page();
    }
}