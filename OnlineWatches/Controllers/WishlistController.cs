﻿using Microsoft.AspNetCore.Mvc;
using OnlineWatches.Data; 
using System.Security.Claims; // For user identification
using Microsoft.EntityFrameworkCore;
using OnlineWatches.Models;
using Microsoft.AspNetCore.Authorization; 

public class WishlistController : Controller
{

    //initializes db
    private readonly OnlineWatchesDbContext _context;

    public WishlistController(OnlineWatchesDbContext context)
    {
        _context = context;
    }

    // Display Wishlist
    
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID
        var wishlistItems = _context.WishlistItems
                                    .Include(w => w.Watch) // Include Watch details
                                    .Where(w => w.UserId == userId)
                                    .ToList();

        return View(wishlistItems);
    }

    // Add to Wishlist
    [HttpPost]
    [Authorize]
    public IActionResult AddToWishlist(int watchId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Check if item already exists in the wishlist
        if (!_context.WishlistItems.Any(w => w.WatchId == watchId && w.UserId == userId))
        {
            var wishlistItem = new WishlistItem { WatchId = watchId, UserId = userId };//creates new wishlistitem
            _context.WishlistItems.Add(wishlistItem);//adds the item to wishlist
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // Remove from Wishlist
    [HttpPost]
    [Authorize]
    public IActionResult RemoveFromWishlist(int wishlistItemId)
    {
        var wishlistItem = _context.WishlistItems.Find(wishlistItemId);//selects the item we want to remove
        if (wishlistItem != null)
        {
            _context.WishlistItems.Remove(wishlistItem);//removes item
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
