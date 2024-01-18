using OnlineWatches.Models;
using OnlineWatches.Utility;
using OnlineWatches.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OnlineWatches.Data;
using OnlineWatches.Models;
using OnlineWatches.ViewModels;

namespace OnlineWatches.Controllers
{
	public class AccountController : Controller
	{

		private OnlineWatchesDbContext _dbContext;
		UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;

		public AccountController(OnlineWatchesDbContext dbContext, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
				return View(loginViewModel);

			var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("", "Invalid Login");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LogOff()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}


		public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();

			}

			if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
			{
				await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
				await _roleManager.CreateAsync(new IdentityRole(Helper.User));
			}

			var user = new ApplicationUser()
			{
				UserName = model.Email,
				Email = model.Email,
				Name = model.Name,
				
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.RoleName);
				await _signInManager.SignInAsync(user, isPersistent: false);
				RedirectToAction("Index", "Home");
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}


        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault(); // Assuming one role per user

            var model = new UserInfoViewModel
            {
                Name = user.Name,
                Email = user.Email,
                RoleName = roleName
            };

            ViewBag.ApplicationUser = user; // Pass the ApplicationUser object to the view
            return View(model);
        }



        public async Task<IActionResult> AllAccounts()
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var users = _userManager.Users.ToList();
            return View(users);
        }




        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("AllAccounts");
        }





    }
}
