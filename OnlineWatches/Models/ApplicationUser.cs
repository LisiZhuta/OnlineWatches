using Microsoft.AspNetCore.Identity;

namespace OnlineWatches.Models
{
	public class ApplicationUser:IdentityUser //User model
	{
		public string Name { get; set; }
		
	}
}
