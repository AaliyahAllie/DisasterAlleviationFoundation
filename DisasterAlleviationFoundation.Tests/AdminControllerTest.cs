using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Tests
{
    public class AdminControllerTests
    {
        // -------------------
        // Helper: InMemory DbContext
        // -------------------
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed Volunteers
            context.Volunteers.AddRange(
                new Volunteer { VolunteerId = 1, Name = "Alice" },
                new Volunteer { VolunteerId = 2, Name = "Bob" }
            );

            // Seed VolunteerAssignments
            context.VolunteerAssignments.Add(
                new VolunteerAssignment
                {
                    VolunteerId = 1,
                    TaskDescription = "Distribute Food",
                    DisasterId = 1,
                    Location = "City A",
                    DateAssigned = DateTime.Now
                }
            );

            // Seed VolunteerTasks
            context.VolunteerTasks.Add(new VolunteerTask
            {
                TaskId = 1,
                Title = "Distribute Food",
                Description = "Distribute food to flood victims",
                DateTime = DateTime.Now
            });

            // Seed Disasters
            context.Disasters.Add(new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flooding in city",
                Location = "City A",
                DateReported = DateTime.Now,
                Severity = "High"
            });

            context.SaveChanges();
            return context;
        }

        // -------------------
        // Helper: Create Controller with TestSession
        // -------------------
        private AdminController GetController(ApplicationDbContext context, bool isAdmin = false)
        {
            var controller = new AdminController(context);

            var session = new TestSession();
            if (isAdmin)
                session.Set("IsAdmin", System.Text.Encoding.UTF8.GetBytes("true"));

            var httpContext = new DefaultHttpContext
            {
                Session = session
            };

            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            return controller;
        }

        // -------------------
        // Login POST tests
        // -------------------
        [Fact]
        public void Login_Post_ValidCredentials_RedirectsToDashboard()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context);

            var model = new AdminLoginViewModel { Username = "admin", Password = "adminPassKey12@" };

            var result = controller.Login(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
        }

        [Fact]
        public void Login_Post_InvalidCredentials_ReturnsViewWithError()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context);

            var model = new AdminLoginViewModel { Username = "wrong", Password = "wrong" };

            var result = controller.Login(model) as ViewResult;

            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
        }

        // -------------------
        // Dashboard tests
        // -------------------
        [Fact]
        public void Dashboard_AdminAccess_ReturnsViewWithVolunteers()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var result = controller.Dashboard() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as List<VolunteerAdminViewModel>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Dashboard_NonAdminAccess_ReturnsAccessDenied()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: false);

            var result = controller.Dashboard() as ContentResult;

            Assert.NotNull(result);
            Assert.Equal("Access denied.", result.Content);
        }

        // -------------------
        // DeleteVolunteer tests
        // -------------------
        [Fact]
        public void DeleteVolunteer_AdminAccess_DeletesVolunteer()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var result = controller.DeleteVolunteer(1) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
            Assert.Null(context.Volunteers.FirstOrDefault(v => v.VolunteerId == 1));
        }

        [Fact]
        public void DeleteVolunteer_NonAdminAccess_ReturnsAccessDenied()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: false);

            var result = controller.DeleteVolunteer(1) as ContentResult;

            Assert.NotNull(result);
            Assert.Equal("Access denied.", result.Content);
        }

        // -------------------
        // AssignVolunteer tests
        // -------------------
        [Fact]
        public void AssignVolunteer_AdminAccess_ReturnsViewWithAssignment()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var result = controller.AssignVolunteer(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as VolunteerAssignment;
            Assert.NotNull(model);
            Assert.Equal(1, model.VolunteerId);
        }

        [Fact]
        public void AssignVolunteer_NonAdminAccess_ReturnsAccessDenied()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: false);

            var result = controller.AssignVolunteer(1) as ContentResult;

            Assert.NotNull(result);
            Assert.Equal("Access denied.", result.Content);
        }

        [Fact]
        public void AssignVolunteer_InvalidVolunteerId_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var result = controller.AssignVolunteer(999) as NotFoundResult;

            Assert.NotNull(result);
        }

        // -------------------
        // EditAssignment tests
        // -------------------
        [Fact]
        public void EditAssignment_AdminAccess_AddsNewAssignment()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var newAssignment = new VolunteerAssignment
            {
                VolunteerId = 2,
                TaskDescription = "Set up shelter",
                DisasterId = 1,
                Location = "City B"
            };

            var result = controller.EditAssignment(newAssignment) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
            Assert.NotNull(context.VolunteerAssignments.FirstOrDefault(a => a.VolunteerId == 2));
        }

        [Fact]
        public void EditAssignment_AdminAccess_UpdatesExistingAssignment()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: true);

            var updatedAssignment = new VolunteerAssignment
            {
                VolunteerId = 1,
                TaskDescription = "Updated Task",
                DisasterId = 1,
                Location = "City Updated"
            };

            var result = controller.EditAssignment(updatedAssignment) as RedirectToActionResult;

            Assert.NotNull(result);
            var assignment = context.VolunteerAssignments.First(a => a.VolunteerId == 1);
            Assert.Equal("Updated Task", assignment.TaskDescription);
            Assert.Equal("City Updated", assignment.Location);
        }

        [Fact]
        public void EditAssignment_NonAdminAccess_ReturnsAccessDenied()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context, isAdmin: false);

            var model = new VolunteerAssignment { VolunteerId = 1 };

            var result = controller.EditAssignment(model) as ContentResult;

            Assert.NotNull(result);
            Assert.Equal("Access denied.", result.Content);
        }
    }
}
