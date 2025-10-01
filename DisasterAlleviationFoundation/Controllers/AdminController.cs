using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string AdminUser = "admin";
        private const string AdminPassword = "adminPassKey12@";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin login
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (model.Username == AdminUser && model.Password == AdminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        // Admin Dashboard: show volunteers and their current assignments
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var volunteers = _context.Volunteers.ToList();
            var assignments = _context.VolunteerAssignments
                                      .Include(a => a.Disaster)
                                      .ToList();

            var model = volunteers.Select(v => new VolunteerAdminViewModel
            {
                Volunteer = v,
                Assignment = assignments.FirstOrDefault(a => a.VolunteerId == v.VolunteerId)
            }).ToList();

            ViewData["Tasks"] = _context.VolunteerTasks.ToList();
            ViewData["Disasters"] = _context.Disasters.ToList();

            return View("~/Views/VolunteerTasks/Dashboard.cshtml", model);
        }


        // Delete volunteer
        [HttpPost]
        public IActionResult DeleteVolunteer(int volunteerId)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var volunteer = _context.Volunteers.Find(volunteerId);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // Edit or Assign volunteer task
        public IActionResult AssignVolunteer(int volunteerId)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var volunteer = _context.Volunteers.Find(volunteerId);
            if (volunteer == null) return NotFound();

            var assignment = _context.VolunteerAssignments
                .FirstOrDefault(a => a.VolunteerId == volunteerId) ?? new VolunteerAssignment
                {
                    VolunteerId = volunteerId
                };

            ViewData["Tasks"] = _context.VolunteerTasks.ToList();
            ViewData["Disasters"] = _context.Disasters.ToList();

            // Point to the VolunteerTasks EditAssignment view
            return View("~/Views/VolunteerTasks/EditAssignment.cshtml", assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssignment(VolunteerAssignment model)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var assignment = _context.VolunteerAssignments
                .FirstOrDefault(a => a.VolunteerId == model.VolunteerId);

            if (assignment == null)
            {
                _context.VolunteerAssignments.Add(model);
            }
            else
            {
                assignment.TaskDescription = model.TaskDescription; // <-- updated
                assignment.DisasterId = model.DisasterId;
                assignment.Location = model.Location;
                assignment.DateAssigned = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

    }
}
