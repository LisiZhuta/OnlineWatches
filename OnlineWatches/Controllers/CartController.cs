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
        private readonly OnlineWatchesDbContext _db;//initializes db

        public CartController(OnlineWatchesDbContext db)
        {
            _db = db;
        }

        //deals with adding items to cart
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

        //displays the list of items in cart
      
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//gets the id of the logged in user

            // Retrieve cart items for the current user
            var cartItems = _db.CartItems.Include(c => c.Watch).Where(c => c.UserId == userId).ToList();

            var cart = new ShoppingCart //creates a shopping cart 
            {
                CartItems = cartItems
            };

            return View(cart);
        }


        //deals with the removal of items from shoppingcart
        [Authorize]
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//gets the id of the logged in user

            // Find the cart item
            var cartItem = _db.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId && c.UserId == userId);

            if (cartItem != null)//checks if cart is not empty
            {
                _db.CartItems.Remove(cartItem);//removes the item selected for removal
                _db.SaveChanges();
            }

            // Redirect to the cart page
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()//displays the checkout view (where you put information for purchase)
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Checkout(CheckoutViewModel model) //deals with the action of checking out 
        {   
            
           
                // Here you would normally process the payment
                // For now, we'll just pretend the payment is successful
                TempData["SuccessMessage"] = "Payment successful!";
                return RedirectToAction("Confirmation");
            

            
        }
        //after hitting the checkout button a little message saying that the payment is successful
        public IActionResult Confirmation() 
        {
            return View();
        }



    }
}
