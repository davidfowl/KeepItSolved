using KeepItSolved.Models;
using KeepItSolved.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace KeepItSolved.Controllers.Web
{
	public class AppController : Controller
    {
		private ISolvedRepository _repository;
		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;

		public AppController(ISolvedRepository repository, 
							UserManager<IdentityUser> userManager, 
							SignInManager<IdentityUser> signInManager)
		{
			_repository = repository;
			_userManager = userManager;
			_signInManager = signInManager;
		}

        // GET: /<controller>/
        public IActionResult Index(string returnUrl = "")
        {
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Cards", "App");
			}
			var model = new LoginViewModel { ReturnUrl = returnUrl };
			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> Index(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false);
				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Cards", "App");
					}
				}

			}
			ModelState.AddModelError("", "Login or Password incorrect");
			return View();
		}

		[Authorize]
		public IActionResult Cards()
		{
			var cards = _repository.GetAllFlashcards();
			return View(cards);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser { UserName = model.Login, Email = model.Email };
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, false);
					return RedirectToAction("Cards", "App");
				}
				else
				{
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "App");
		}
    }
}
