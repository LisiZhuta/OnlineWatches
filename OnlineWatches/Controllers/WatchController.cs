using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using OnlineWatches.Data;
using OnlineWatches.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineWatches.Controllers
{
    public class WatchController : Controller
    {
        public OnlineWatchesDbContext _db;
        public WatchController(OnlineWatchesDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            // Retrieve expenses for the current user
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Watch> watches = _db.Watches;
              //  .Where(e => e.CreatedUserId == userId);
                

            return View(watches);
        }

        public IActionResult Rolex()
        {
            // Retrieve watches that contain "Rolex" in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains("Rolex")).ToList();

            return View(watches);
        }
        public IActionResult Audemars()
        {
           
            // Retrieve watches that contain "Audemars" in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains("Audemars")).ToList();

            return View(watches);
        }
        
        
        
        public IActionResult Patek()
        {
            // Retrieve watches that contain "Patek" in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains("Patek")).ToList();

            return View(watches);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
      public IActionResult Create()
        {
            return View();
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Watch obj)
        {
            if (ModelState.IsValid)
            {  // Creates a watch 
               // obj.CreatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _db.Watches.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Search(string searchTerm)
        {
            // Retrieve watches that contain the search term in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains(searchTerm)).ToList();
            

            return View("Search", watches); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveWatch(int watchId)
        {
            // Retrieve the watch from the database
            var watch = _db.Watches.Find(watchId);

            if (watch == null)
            {
                
                return NotFound();
            }

            // Remove the watch from the database
            _db.Watches.Remove(watch);
            _db.SaveChanges();

            
            return RedirectToAction("Index");
        }


    }
}
