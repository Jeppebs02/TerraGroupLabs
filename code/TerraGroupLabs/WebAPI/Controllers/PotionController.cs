using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using WebAPI.BusinessLogicLayer;

namespace WebAPI.Controllers
{
    [Route("api/potion")]
    [ApiController]
    public class PotionController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private PotionLogic _potionLogic;
        
        public PotionController(IConfiguration configuration)
        {
            _configuration = configuration;
            Console.WriteLine($"Potion web API controller created");
            _potionLogic = new PotionLogic(_configuration);
            Console.WriteLine($"Potion web API controller created with logic layer");
        }
        
        [HttpGet]
        public IActionResult CheckHealth()
        {
            Console.WriteLine($"Potion web API health check");
            return Ok("Potion Controller is up and running");
        }  
        
        [HttpPost]
        public IActionResult CreatePotion([FromBody] Potion potion)
        {
            Console.WriteLine($"Potion web API create potion");
            
            if (potion == null)
            {
                return BadRequest("Potion data is null");
            }
            
            if(_potionLogic.AddPotion(potion))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to create potion");
            }
            
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeletePotion(int id)
        {
            Console.WriteLine($"Potion web API delete potion with id: {id}");
            
            if (_potionLogic.DeletePotion(id))
            {
                return Ok();
            }
            else
            {
                return NotFound("Potion not found");
            }
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
