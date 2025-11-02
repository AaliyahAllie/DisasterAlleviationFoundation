using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

public class VolunteersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public VolunteersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Display all volunteers
    public IActionResult Index()
    {
        var volunteers = _context.Volunteers.ToList();
        return View(volunteers);
    }

    // GET: Display the registration form
    public IActionResult Register()
    {
        return View();
    }

    // POST: Handle form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Volunteer model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Get the logged-in user, if any
        var user = await _userManager.GetUserAsync(User);

        // Safe assignment: UserId is null if no user is logged in
        model.UserId = user?.Id;

        _context.Volunteers.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
