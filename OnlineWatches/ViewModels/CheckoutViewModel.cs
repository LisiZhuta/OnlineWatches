namespace OnlineWatches.ViewModels;

using OnlineWatches.Models;

public class CheckoutViewModel
{
    public ShoppingCart ShoppingCart { get; set; }

    // Customer's personal information
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // Credit Card Information
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVC { get; set; }

    // Other properties as needed
}