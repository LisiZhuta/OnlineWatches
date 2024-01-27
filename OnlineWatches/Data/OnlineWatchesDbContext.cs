using OnlineWatches.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace OnlineWatches.Data
{
	public class OnlineWatchesDbContext:IdentityDbContext<ApplicationUser>	
	{
		public OnlineWatchesDbContext(DbContextOptions<OnlineWatchesDbContext> options)
			: base(options)
		{

		}


		public DbSet<Watch> Watches { get; set; }//adds watches

        public DbSet<CartItem> CartItems { get; set; }//adds cartitems

        public DbSet<WishlistItem> WishlistItems { get; set; }//adds wishlistitems


    }
}
