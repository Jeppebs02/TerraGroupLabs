using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    public class PotionController : Controller
    {
        
        //Potion API requests will be handled here
        
        
        
        // GET: PotionController
        public ActionResult Index()
        {
            return View();
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
