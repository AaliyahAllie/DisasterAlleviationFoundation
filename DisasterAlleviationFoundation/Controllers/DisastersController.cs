using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var disasters = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .ToListAsync();
            return View(disasters);
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .FirstOrDefaultAsync(m => m.DisasterId == id);

            if (disaster == null) return NotFound();

            return View(disaster);
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            ViewData["SeverityOptions"] = new List<string> { "Low", "Mild", "High" };
            return View();
        }

        // POST: Disasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disaster model, IFormFile? Evidence)
        {
            if (!ModelState.IsValid)
            {
                ViewData["SeverityOptions"] = new List<string> { "Low", "Mild", "High" }; // re-populate
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            model.UserId = user?.Id;
            model.DateReported = DateTime.Now;

            if (model.DisasterFiles == null)
                model.DisasterFiles = new List<DisasterFile>();

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

                model.DisasterFiles.Add(new DisasterFile
                {
                    FileName = fileName,
                    FilePath = "/uploads/" + fileName
                });
            }

            _context.Disasters.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Disasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .FirstOrDefaultAsync(d => d.DisasterId == id);

            if (disaster == null) return NotFound();

            ViewData["SeverityOptions"] = new List<string> { "Low", "Mild", "High" };
            return View(disaster);
        }

        // POST: Disasters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disaster disaster, IFormFile? Evidence)
        {
            if (id != disaster.DisasterId) return NotFound();
            if (!ModelState.IsValid) return View(disaster);

            var existing = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .FirstOrDefaultAsync(d => d.DisasterId == id);

            if (existing == null) return NotFound();

            existing.Title = disaster.Title;
            existing.Description = disaster.Description;
            existing.Location = disaster.Location;
            existing.Severity = disaster.Severity;

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

                existing.DisasterFiles.Add(new DisasterFile
                {
                    FileName = fileName,
                    FilePath = "/uploads/" + fileName
                });
            }

            _context.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Disasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .FirstOrDefaultAsync(d => d.DisasterId == id);

            if (disaster == null) return NotFound();

            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disaster = await _context.Disasters
                .Include(d => d.DisasterFiles)
                .FirstOrDefaultAsync(d => d.DisasterId == id);

            if (disaster != null)
            {
                // delete files from disk
                foreach (var file in disaster.DisasterFiles)
                {
                    var path = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.Disasters.Remove(disaster);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
