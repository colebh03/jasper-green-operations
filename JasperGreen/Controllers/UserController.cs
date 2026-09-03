using JasperGreen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserManager<User> userManager;

        public UserController(UserManager<User> userMngr)
        {
            userManager = userMngr;
        }

        public IActionResult Index()
        {
            List<User> users =
                userManager.Users.ToList();

            UserViewModel model =
                new UserViewModel
                {
                    Users = users
                };

            return View("List", model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";

            UserAddEditViewModel model =
                new UserAddEditViewModel();

            return View("AddEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User? user =
                await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserAddEditViewModel model =
                new UserAddEditViewModel
                {
                    Id = user.Id,
                    Username = user.UserName ?? ""
                };

            ViewBag.Action = "Edit";

            return View("AddEdit", model);
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
    UserAddEditViewModel model)
        {
            bool isAdd = string.IsNullOrEmpty(model.Id);

            // Password fields are required only when creating a new user
            if (isAdd &&
                string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(
                    "Password",
                    "Please enter a password.");
            }

            if (isAdd &&
                string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ModelState.AddModelError(
                    "ConfirmPassword",
                    "Please confirm the password.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Action = isAdd ? "Add" : "Edit";

                return View("AddEdit", model);
            }

            if (isAdd)
            {
                // Create the new Identity user using the supplied username and password
                User user = new User
                {
                    UserName = model.Username
                };

                IdentityResult result =
                    await userManager.CreateAsync(
                        user,
                        model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] =
                        model.Username +
                        " was added.";

                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                ViewBag.Action = "Add";

                return View("AddEdit", model);
            }
            else
            {
                // Existing user edits only update the username
                User? user =
                    await userManager.FindByIdAsync(model.Id!);

                if (user == null)
                {
                    return NotFound();
                }

                IdentityResult result =
                    await userManager.SetUserNameAsync(
                        user,
                        model.Username);

                if (result.Succeeded)
                {
                    TempData["message"] =
                        model.Username +
                        " was updated.";

                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                ViewBag.Action = "Edit";

                return View("AddEdit", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            string currentUserId =
                userManager.GetUserId(User) ?? "";

            // Prevent an authenticated user from deleting the account they are currently using
            if (id == currentUserId)
            {
                TempData["Error"] =
                    "You cannot delete your own account.";

                return RedirectToAction("Index");
            }

            User? user =
                await userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] =
                    "The selected user could not be found.";

                return RedirectToAction("Index");
            }

            IdentityResult result =
                await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["message"] =
                    user.UserName +
                    " was deleted.";
            }
            else
            {
                string errorMessage = "";

                foreach (IdentityError error in result.Errors)
                {
                    errorMessage +=
                        error.Description + " | ";
                }

                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("Index");
        }
    }
}