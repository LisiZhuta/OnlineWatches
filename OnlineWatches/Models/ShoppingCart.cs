namespace OnlineWatches.Models
{
    public class CartItem //cart model
    {
        public int CartItemId { get; set; }
        public int WatchId { get; set; }
        public Watch Watch { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; } // Assuming each user has their cart
    }

    public class ShoppingCart
    {
        public List<CartItem> CartItems { get; set; }
        
    }
}
