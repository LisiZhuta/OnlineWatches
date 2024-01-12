using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineWatches.Data;
using OnlineWatches.Models;
using System.Diagnostics;
using System.Linq;

namespace OnlineWatches.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineWatchesDbContext _db;

        public HomeController(ILogger<HomeController> logger, OnlineWatchesDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            // Retrieve all watches from the database
            var watches = _db.Watches.ToList();

            // Pass the watches to the view
            return View(watches);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
