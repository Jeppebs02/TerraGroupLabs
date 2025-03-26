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
        }
        
        [HttpGet]
        public IActionResult CheckHealth()
        {
            return Ok("Potion Controller is up and running");
        }   
        
        
        [HttpGet("CheckHealth2")]
        public IActionResult CheckHealth2()
        {
            return Ok("Potion Controller is up and running and supports route parameters");
        }
        
                
        [HttpGet("AllPotions")]
        public IActionResult GetAllPotions()
        {
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
