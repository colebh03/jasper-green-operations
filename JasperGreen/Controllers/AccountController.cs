using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using Microsoft.AspNetCore.Identity;

namespace JasperGreen.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager; private SignInManager<User> signInManager;
        public AccountController(UserManager<User> userMngr, SignInManager<User> signInMngr)
        {
            userManager = userMngr; signInManager = signInMngr;
        }

        // The Register(), LogIn(), and LogOut()methods go here }

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
                var user = new User { UserName = model.Username }; var result = await userManager.CreateAsync(user, model.Password); if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false); return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync(); return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnURL }; return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					User user = await userManager.FindByNameAsync(model.Username);

					if (await userManager.IsInRoleAsync(user, "Admin"))
					{
						return RedirectToAction("Index", "Admin");
					}

					return RedirectToAction("Index", "Home");
				}
			}
            ModelState.AddModelError("", "Invalid username/password."); return View(model);
        }

		public ViewResult AccessDenied()
		{
			return View();
		}
	}
}
