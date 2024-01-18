using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using OnlineWatches.Data;
using OnlineWatches.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace OnlineWatches.Controllers
{
    public class WatchController : Controller
    {
        public OnlineWatchesDbContext _db;
        public WatchController(OnlineWatchesDbContext db)
        {
            _db = db;
        }


        public IActionResult AllWatches()
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
                return RedirectToAction("AllWatches");
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
        public IActionResult RemoveWatch(int watchId, string sourceView)
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

            // Determine the appropriate redirect based on the source view
            
            if (sourceView == "Rolex")
            {
                return RedirectToAction("Rolex");
            }
            else if (sourceView == "Patek")
            {
                return RedirectToAction("Patek");
            }
            else if (sourceView == "Audemars")
            {
                return RedirectToAction("Audemars");
            }
            else if (sourceView == "NewArrival")
            {
                return RedirectToAction("NewArrival");
            }
            else if (sourceView == "Search")
            {
                return RedirectToAction("Search");
            }
            else if (sourceView == "Sale")
            {
                return RedirectToAction("Sale");
            }
            // Add more conditions for other views if needed

            // Default redirect if sourceView is not recognized
            return RedirectToAction("AllWatches");
        }


        

       
        public IActionResult NewArrival()
        {
            IEnumerable<Watch> watches=_db.Watches.Where(w=>w.Status=="New Arrival").ToList();
            return View(watches);
        }

        public IActionResult Sale()
        {
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Status == "Sale").ToList();
            return View(watches);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var watch = _db.Watches.Find(id);
            if (watch == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();

            return View(watch);
           
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Watch updatedWatch, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var watch = _db.Watches.Find(updatedWatch.Id);
                if (watch == null)
                {
                    return NotFound();
                }

                // Update the properties of the watch
                watch.Name = updatedWatch.Name;
                watch.Price = updatedWatch.Price;
                watch.Year = updatedWatch.Year;
                watch.ImgPath = updatedWatch.ImgPath;
                watch.Status = updatedWatch.Status;
                watch.Discount = updatedWatch.Discount;

                _db.Watches.Update(watch);
                _db.SaveChanges();

                return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("AllWatches");
                
            }
            return View(updatedWatch);
        }


        public IActionResult Details(int id)
        {
            var watch = _db.Watches.FirstOrDefault(w => w.Id == id);
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }



    }
}
