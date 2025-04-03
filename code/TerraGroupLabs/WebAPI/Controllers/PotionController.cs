using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PotionModels = Models.Model.Potion.Potion;
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
        
        private static string? GetClientIp(HttpContext httpContext)
        {
            
            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress?.ToString();
            }

            return ip;
        }
        

        private void LogRequest()
        {
            Console.WriteLine($"Api request from: {GetClientIp(HttpContext)}");
        }   
        
        [HttpGet]
        public IActionResult CheckHealth()
        {
            Console.WriteLine($"Potion web API health check");
            LogRequest();
            return Ok("Potion Controller is up and running");
        }  
        
        [HttpPost]
        public IActionResult CreatePotion([FromBody] PotionModels potion)
        {
            LogRequest();
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
            LogRequest();
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
            LogRequest();
            Console.WriteLine($"Potion web API health check 2");
            return Ok("Potion Controller is up and running and supports route parameters test deployyyyyy");
        }


        [HttpGet("AllPotions")]
        public IActionResult GetAllPotions()
        {
            LogRequest();
            Console.WriteLine($"Request to get all potions");

            var potionLogic = new PotionLogic(_configuration);
            var potions = potionLogic.GetAllPotions();

            if (potions.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(potions);
            }
        }

        [HttpPut("{id}")]
            public IActionResult UpdatePotion(int id, [FromBody] PotionModels potion)
            {
                LogRequest();
                Console.WriteLine($"Potion web API update potion with id: {id}");

                if (potion == null || potion.Id != id)
                {
                    return BadRequest("Potion data is null or ID mismatch");
                }

                if (_potionLogic.UpdatePotion(potion))
                {
                    return Ok();
                }
                else
                {
                    return NotFound("Potion not found");
                }
            }

        
        
        
        
        
    }
}
