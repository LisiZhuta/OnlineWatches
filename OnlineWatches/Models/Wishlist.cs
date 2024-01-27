namespace OnlineWatches.Models
{
    public class WishlistItem //wishlist model
    {
        public int WishlistItemId { get; set; }
        public string UserId { get; set; } // User's identifier
        public int WatchId { get; set; }
        public virtual Watch Watch { get; set; } // Navigation property
    }
}
