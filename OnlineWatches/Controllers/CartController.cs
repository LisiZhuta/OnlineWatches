using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWatches.Data;
using OnlineWatches.Models;
using OnlineWatches.ViewModels;
using System.Security.Claims;

namespace OnlineWatches.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineWatchesDbContext _db;

        public CartController(OnlineWatchesDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(int watchId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the item already exists in the cart
            var existingCartItem = _db.CartItems.FirstOrDefault(c => c.WatchId == watchId && c.UserId == userId);

            if (existingCartItem != null)
            {
                // If exists, just update the quantity
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // If not exists, create a new cart item
                var cartItem = new CartItem
                {
                    WatchId = watchId,
                    Quantity = quantity,
                    UserId = userId
                };

                _db.CartItems.Add(cartItem);
            }

            _db.SaveChanges();

            // Redirect to the cart page or wherever you want the user to go next
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve cart items for the current user
            var cartItems = _db.CartItems.Include(c => c.Watch).Where(c => c.UserId == userId).ToList();

            var cart = new ShoppingCart
            {
                CartItems = cartItems
            };

            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the cart item
            var cartItem = _db.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId && c.UserId == userId);

            if (cartItem != null)
            {
                _db.CartItems.Remove(cartItem);
                _db.SaveChanges();
            }

            // Redirect to the cart page
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Checkout(CheckoutViewModel model)
        {
           
                // Here you would normally process the payment
                // For now, we'll just pretend the payment is successful
                TempData["SuccessMessage"] = "Payment successful!";
                return RedirectToAction("Confirmation");
            

            
        }
        public IActionResult Confirmation() 
        {
            return View();
        }



    }
}
