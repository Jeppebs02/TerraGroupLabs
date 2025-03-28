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
        
        return Page();
    }
    
    
    public IActionResult OnPost(string name, int price)
    {
        // Create a new potion object
        Potion newPotion = new Potion
        {
            Name = name,
            Price = price
        };
        
        // Convert the potion object to JSON
        string jsonPotion = JsonConvert.SerializeObject(newPotion);
        
        // Send the POST request to the API
        _apiRequester.PostAsync("potion", jsonPotion).Wait();
    
        // Handle form submission here
        // You can access the submitted data using Potions property
        // For example, you can save the data to the database or perform other actions
        
        
        
        return RedirectToPage(Potions); // Redirect to the same page after processing
    }
}