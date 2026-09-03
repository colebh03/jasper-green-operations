using JasperGreen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JasperGreen.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(
            UserManager<User> userMngr,
            SignInManager<User> signInMngr)
        {
            userManager = userMngr;
            signInManager = signInMngr;
        }

        [HttpGet]
        public IActionResult LogIn(string returnUrl = "")
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Authenticate user without triggering account locked out on failure
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(
                        model.Username,
                        model.Password,
                        model.RememberMe,
                        false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            ModelState.AddModelError(
                "",
                "Invalid username or password.");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public ViewResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            // Resolve the currently authenticated user
            User? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("LogIn");
            }

            ChangePasswordViewModel model =
                new ChangePasswordViewModel
                {
                    Username = user.UserName ?? ""
                };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel model)
        {
            User? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("LogIn");
            }

            model.Username = user.UserName ?? "";

            if (ModelState.IsValid)
            {
                IdentityResult result =
                    await userManager.ChangePasswordAsync(
                        user,
                        model.OldPassword,
                        model.NewPassword);

                if (result.Succeeded)
                {
                    // Refresh authenticated session so user stays signed in after their password changes
                    await signInManager.RefreshSignInAsync(user);

                    TempData["message"] =
                        "Your password was changed successfully.";

                    // Redirect authenticated administrators back to operations portal                   
                    return RedirectToAction("Index", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }
            }

            return View(model);
        }
    }
}