using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    public class VolunteerTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard: list tasks and volunteers
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied. You do not have access to this resource.");

            var tasks = _context.VolunteerTasks
                .Include(t => t.AssignedVolunteer)
                .ToList();

            var volunteers = _context.Volunteers.ToList();
            ViewData["Volunteers"] = volunteers;

            return View(tasks);
        }

        // Assign or reassign a volunteer to a task
        [HttpPost]
        public IActionResult AssignTask(int taskId, int volunteerId)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var task = _context.VolunteerTasks.Find(taskId);
            if (task != null)
            {
                task.AssignedVolunteerId = volunteerId != 0 ? volunteerId : null;
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // Delete a task
        [HttpPost]
        public IActionResult DeleteTask(int taskId)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var task = _context.VolunteerTasks.Find(taskId);
            if (task != null)
            {
                _context.VolunteerTasks.Remove(task);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // Delete a volunteer
        [HttpPost]
        public IActionResult DeleteVolunteer(int volunteerId)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var volunteer = _context.Volunteers.Find(volunteerId);
            if (volunteer != null)
            {
                // Unassign all tasks from this volunteer
                var assignedTasks = _context.VolunteerTasks
                    .Where(t => t.AssignedVolunteerId == volunteerId)
                    .ToList();

                foreach (var t in assignedTasks)
                    t.AssignedVolunteerId = null;

                _context.Volunteers.Remove(volunteer);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // Optional: create a new task
        [HttpPost]
        public IActionResult CreateTask(string title, string description)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Content("Access denied.");

            var task = new VolunteerTask
            {
                Title = title,
                Description = description,
                DateTime = DateTime.Now
            };

            _context.VolunteerTasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}
