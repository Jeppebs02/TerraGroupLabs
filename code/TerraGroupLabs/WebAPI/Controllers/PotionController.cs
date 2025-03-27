using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogicLayer;

namespace WebAPI.Controllers
{
    [Route("api/potion")]
    [ApiController]
    public class PotionController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        
        public PotionController(IConfiguration configuration)
        {
            _configuration = configuration;
            Console.WriteLine($"Potion web API controller created");
        }
        
        [HttpGet]
        public IActionResult CheckHealth()
        {
            Console.WriteLine($"Potion web API health check");
            return Ok("Potion Controller is up and running");
        }   
        
        
        [HttpGet("CheckHealth2")]
        public IActionResult CheckHealth2()
        {
            Console.WriteLine($"Potion web API health check 2");
            return Ok("Potion Controller is up and running and supports route parameters test deployyyyyy");
        }
        
                
        [HttpGet("AllPotions")]
        public IActionResult GetAllPotions()
        {
            Console.WriteLine($"Request to get all potions");
            
            var potionLogic = new PotionLogic(_configuration);
            var potions = potionLogic.GetAllPotions();
            
            if(potions.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(potions);
            }

        }
        
        
        
        
    }
}
