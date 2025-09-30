using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DisastersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var disasters = _context.Disasters.ToList();
            return View(disasters);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Disaster disaster)
        {
            disaster.UserId = _userManager.GetUserId(User);
            _context.Add(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
