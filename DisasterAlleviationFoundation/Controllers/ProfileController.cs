using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DisasterAlleviationFoundation;
public class ProfileController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ProfileController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Index", "Home");

        var model = new ProfileViewModel
        {
            Email = user.Email,
            UserName = user.UserName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Index", "Home");

        user.Email = model.Email;
        user.UserName = model.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View("Index", model);
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Index", model);
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        ViewBag.StatusMessage = "Profile updated successfully!";
        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        // Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            // Optionally display errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var model = new ProfileViewModel
            {
                Email = user.Email,
                UserName = user.UserName
            };
            return View("Index", model);
        }

        // Log out the user after deleting
        await _signInManager.SignOutAsync();

        // Redirect to home page
        return RedirectToAction("Index", "Home");
    }
}
