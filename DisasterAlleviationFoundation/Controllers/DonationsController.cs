using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DonationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            var donations = await _context.Donations
                .OrderByDescending(d => d.DateDonated)
                .ToListAsync();
            return View(donations);
        }

        // GET: Donations/Create
        public IActionResult Create() => View();

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation model)
        {
            // Avoid UserId required error
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            model.UserId = user?.Id;
            model.DateDonated = DateTime.Now;

            _context.Donations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Donations/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                donation.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
