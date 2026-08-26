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
                        " was added successfully.";

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
                        " was updated successfully.";

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

            if (id == currentUserId)
            {
                TempData["message"] =
                    "You cannot delete your own account.";

                return RedirectToAction("Index");
            }

            User? user =
                await userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["message"] =
                    "The selected user could not be found.";

                return RedirectToAction("Index");
            }

            IdentityResult result =
                await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["message"] =
                    user.UserName +
                    " was deleted successfully.";
            }
            else
            {
                string errorMessage = "";

                foreach (IdentityError error in result.Errors)
                {
                    errorMessage +=
                        error.Description + " | ";
                }

                TempData["message"] = errorMessage;
            }

            return RedirectToAction("Index");
        }
    }
}