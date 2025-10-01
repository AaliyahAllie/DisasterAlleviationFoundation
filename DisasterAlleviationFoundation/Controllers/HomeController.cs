using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DisasterAlleviationFoundation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get volunteers with their assignments
            var assignments = _context.VolunteerAssignments
                .Include(a => a.Volunteer)   // Include volunteer info
                .Include(a => a.Disaster)    // Include disaster info
                .ToList();

            // Optional: pass Admin login URL to view
            ViewData["AdminLoginUrl"] = Url.Action("Login", "Admin");
           // Stats
            ViewData["TotalVolunteers"] = _context.Volunteers.Count();
            ViewData["TotalDisasters"] = _context.Disasters.Count();
            ViewData["TotalDonations"] = _context.Donations.Count();
            // Pass assignments list to the view
            return View(assignments);
        }


        public IActionResult About()
        {
            ViewData["Title"] = "About Disaster Alleviation";

            ViewData["Mission"] = "Our mission is to assist communities affected by disasters by providing a secure and user-friendly platform for reporting incidents, managing donations, and coordinating volunteers.";

            ViewData["Features"] = new List<string>
            {
                "Disaster Incident Reporting: Users can submit reports of natural disasters or emergencies in their area.",
                "Resource Donation Management: Enables individuals and organizations to donate essential resources like food, clothing, and medical supplies.",
                "Volunteer Coordination: Volunteers can register and receive task assignments managed by Admin.",
                "Secure User Authentication: Safe registration and login using ASP.NET Identity and Azure SQL backend.",
                "Data Tracking & Transparency: All incidents, donations, and volunteer contributions are recorded for accountability."
            };

            ViewData["Benefits"] = new List<string>
            {
                "Rapid response to emergencies through organized reporting and coordination.",
                "Improved distribution of resources to affected areas.",
                "Engagement of volunteers in structured and trackable relief efforts.",
                "Secure platform protecting user information and data integrity.",
                "Supports disaster preparedness, recovery, and community resilience."
            };

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(model);
        }
    }

    
}
