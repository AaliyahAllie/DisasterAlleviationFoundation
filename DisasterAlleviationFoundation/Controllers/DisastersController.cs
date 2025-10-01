using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public DisastersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: Disasters
        public async Task<IActionResult> Index(string searchLocation, string searchSeverity)
        {
            var disasters = _context.Disasters.AsQueryable();

            if (!string.IsNullOrEmpty(searchLocation))
            {
                disasters = disasters.Where(d => d.Location.Contains(searchLocation));
            }

            if (!string.IsNullOrEmpty(searchSeverity))
            {
                disasters = disasters.Where(d => d.Severity == searchSeverity);
            }

            return View(await disasters.ToListAsync());
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var disaster = await _context.Disasters.FirstOrDefaultAsync(d => d.DisasterId == id);
            if (disaster == null) return NotFound();
            return View(disaster);
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disaster disaster, IFormFile Evidence)
        {
            if (!ModelState.IsValid) return View(disaster);

            disaster.CreatedBy = _userManager.GetUserId(User);

            if (Evidence != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(Evidence.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Evidence.CopyToAsync(stream);
                }

                disaster.EvidenceFileName = fileName;
            }

            _context.Add(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Disasters/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null) return NotFound();
            return View(disaster);
        }

        // POST: Disasters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disaster disaster, IFormFile Evidence)
        {
            if (id != disaster.DisasterId) return NotFound();
            if (!ModelState.IsValid) return View(disaster);

            var existingDisaster = await _context.Disasters.FindAsync(id);
            if (existingDisaster == null) return NotFound();

            existingDisaster.Name = disaster.Name;
            existingDisaster.Location = disaster.Location;
            existingDisaster.Description = disaster.Description;
            existingDisaster.Severity = disaster.Severity;
            existingDisaster.Status = disaster.Status;

            if (Evidence != null)
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(existingDisaster.EvidenceFileName))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "uploads", existingDisaster.EvidenceFileName);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                // Save new file
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(Evidence.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Evidence.CopyToAsync(stream);
                }

                existingDisaster.EvidenceFileName = fileName;
            }

            _context.Update(existingDisaster);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Disasters/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null) return NotFound();
            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster != null)
            {
                // Delete file if exists
                if (!string.IsNullOrEmpty(disaster.EvidenceFileName))
                {
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", disaster.EvidenceFileName);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Disasters.Remove(disaster);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
