using Microsoft.AspNetCore.Mvc;
using Models.APIRequester;
using Models.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebServer.Controllers
{
    public class PotionController : Controller
    {
        //Potion API requests will be handled here
        private readonly ApiRequester _apiRequester;

        public PotionController(ApiRequester apiRequester)
        {
            _apiRequester = apiRequester;
        }

        public PotionController(IConfiguration configuration = null)
        {
            if (configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("Webserver/Configs/appsettings.json", optional: false, reloadOnChange: true);
                configuration = builder.Build();
            }
            _apiRequester = new ApiRequester(configuration);
        }

        // GET: PotionController
        public async Task<ActionResult> Index()
        {
            try
            {
                string jsonResponse = await _apiRequester.GetAsync("potion/AllPotions");
                List<Potion> potions = JsonConvert.DeserializeObject<List<Potion>>(jsonResponse);
                return View("Index", potions); // Ensure the view name is correctly specified
            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
                return View("Index", new List<Potion>()); // Ensure the view name is correctly specified
            }
        }

        // GET: PotionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PotionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PotionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PotionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PotionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PotionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PotionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
