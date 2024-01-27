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
using Microsoft.AspNetCore.Authorization;

namespace OnlineWatches.Controllers
{
	public class AccountController : Controller
	{
		//initializing db so that we can retrieve data, userManager, ignInManager so that we can show different things for user or admin,RoleManager
		private OnlineWatchesDbContext _dbContext;
		UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;


		//constructor for the above mentioned properties
		public AccountController(OnlineWatchesDbContext dbContext, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		//displays the Login view 
		public IActionResult Login()
		{
			return View();
		}
		

		//Deals with the loggin process
		[HttpPost]
		public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
		{
			//checks if model is valid, if all information are in correct form
			if (!ModelState.IsValid)
				return View(loginViewModel);
			//checks if the information inputted are the same with the login information stored in db
			var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

			//if the login is a success, it sends the user to the homepage 
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("", "Invalid Login");

			return View();
		}

		//logs off the user
		[HttpPost]
		public async Task<IActionResult> LogOff()
		{
			await _signInManager.SignOutAsync();//signs the user out
			return RedirectToAction("Login", "Account");//redirects to login
		}

		//displays the register view
		public IActionResult Register()
		{
			return View();
		}


		//Deals with the process of registering 
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();

			}
			//if the inputted information are unique(dont exist in the database)
			if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
			{
				await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
				await _roleManager.CreateAsync(new IdentityRole(Helper.User));
			}
			//creates a new user and assigns values to its properties
			var user = new ApplicationUser()
			{
				UserName = model.Email,
				Email = model.Email,
				Name = model.Name,
				
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			//if its a success it redirects user to homepage
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.RoleName); //assigns the role for the user 
				await _signInManager.SignInAsync(user, isPersistent: false);//logs the user 
				RedirectToAction("Index", "Home");//redirects him to homepage
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}


		//deals with the display of user info
		[Authorize]
        public async Task<IActionResult> UserInfo()
        {
			//gets the userinfo
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
			//gets the role 
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault(); // assuming one role per user

			//creates a new model and assigns values of the user
            var model = new UserInfoViewModel
            {
                Name = user.Name,
                Email = user.Email,
                RoleName = roleName
            };
			//displays the information of the user 
            ViewBag.ApplicationUser = user; // passes the ApplicationUser object to the view
            return View(model);
        }

		[Authorize]
		//deals with the display of all accounts(manage all account you can say)
        public async Task<IActionResult> AllAccounts()
        {
			//check if its not admin its not authorized(only admin accounts can have authorization)
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
			//gets the list of all users
            var users = _userManager.Users.ToList();
            return View(users);
        }


		//deals with deleting a user (also only an admin can delete another user)
		[Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
			//check if its not admin its not authorized
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
			//by clicking on the remove button in the allaccounts view it selects the user for deletion
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {//deletes the user
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("AllAccounts");
        }





    }
}
