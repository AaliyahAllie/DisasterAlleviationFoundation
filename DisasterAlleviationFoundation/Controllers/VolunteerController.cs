using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VolunteersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var volunteers = _context.Volunteers.ToList();
            return View(volunteers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Volunteer volunteer)
        {
            volunteer.UserId = _userManager.GetUserId(User);
            _context.Add(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
