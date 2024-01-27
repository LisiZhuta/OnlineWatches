using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using OnlineWatches.Data;
using OnlineWatches.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OnlineWatches.Controllers
{
    public class WatchController : Controller
    {
        public OnlineWatchesDbContext _db;
        public WatchController(OnlineWatchesDbContext db)//initializes db
        {
            _db = db;
        }


        public IActionResult AllWatches()
        {
            // Retrieve watches for the current user
            

            IEnumerable<Watch> watches = _db.Watches;

              
                

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


        public IActionResult Hublot()
        {
            // Retrieve watches that contain "Hublot" in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains("Hublot")).ToList();

            return View(watches);
        }

        public IActionResult Cartier()
        {
            // Retrieve watches that contain "Cartier" in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains("Cartier")).ToList();

            return View(watches);
        }



        //displays the create view where you can add a watch
        [Authorize(Roles = "Admin")]
        [HttpGet]
      public IActionResult Create()
        {
            return View();
        }


        //deals with the creation of the watch
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Watch obj)
        {
            if (ModelState.IsValid)//checks if the model inputted is correct
            {  // Creates a watch 
               
                _db.Watches.Add(obj);//adds the watch to the db
                _db.SaveChanges();//saves the db changes
                return RedirectToAction("AllWatches");
            }
            return View(obj);
        }

        //deals with the search functionality
        public IActionResult Search(string searchTerm)//takes a string that compares it to the name of the watches in db
        {
            // Retrieve watches that contain the search term in their name
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Name.Contains(searchTerm)).ToList();//for example the string is rolex and it returns all watches containing that string
            

            return View("Search", watches); 
        }

        //deals with the removal of watches
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
            else if (sourceView == "Cartier")
            {
                return RedirectToAction("Cartier");

            }
            else if (sourceView == "Hublot")
            {
                return RedirectToAction("Hublot");
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
            

            // Default redirect if sourceView is not recognized
            return RedirectToAction("AllWatches");
        }


        

       //displays the watches with the status new arrival
        public IActionResult NewArrival()
        {
            IEnumerable<Watch> watches=_db.Watches.Where(w=>w.Status=="New Arrival").ToList();
            return View(watches);
        }

        //displays the watches with the status sale
        public IActionResult Sale()
        {
            IEnumerable<Watch> watches = _db.Watches.Where(w => w.Status == "Sale").ToList();
            return View(watches);
        }

        //this is displaying the view of the watch that we selected to edit
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var watch = _db.Watches.Find(id);//selects the watch that we clicked on editting
            if (watch == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();//deals with the imgpath

            return View(watch);
           
        }
        //this is dealing with the editing of watches (name price status imgpath)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Watch updatedWatch, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var watch = _db.Watches.Find(updatedWatch.Id);//selects the watch for editting
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

                _db.Watches.Update(watch);//updates the watch info with new values
                _db.SaveChanges();

                return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("AllWatches");
                
            }
            return View(updatedWatch);
        }

        //displays a separate view of the watch we selected for more detailed view
        public IActionResult Details(int id)
        {
            var watch = _db.Watches.FirstOrDefault(w => w.Id == id);
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }

        public IActionResult FilterByPrice(decimal? minPrice, decimal? maxPrice, string viewName)
        {
            IEnumerable<Watch> watches;

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                // Filter watches based on the provided price range
                watches = _db.Watches.Where(w => w.Price >= minPrice && w.Price <= maxPrice).ToList();
            }
            else
            {
                // If no price range is provided, get all watches
                watches = _db.Watches.ToList();
            }

            // If a specific view is specified, further filter watches based on the view
            if (!string.IsNullOrEmpty(viewName))
            {
                if (viewName == "NewArrival")
                {
                    watches = watches.Where(w => w.Status == "New Arrival").ToList();
                }
                else if (viewName == "Sale")
                {
                    watches = watches.Where(w => w.Status == "Sale").ToList();
                }
                else if (viewName == "AllWatches")
                {
                    watches = watches.ToList();
                }

                else
                {
                    watches = watches.Where(w => w.Name.Contains(viewName)).ToList();
                }
            }

            // Pass the filtered watches to the specified view
            return View(viewName, watches);
        }






    }
}
