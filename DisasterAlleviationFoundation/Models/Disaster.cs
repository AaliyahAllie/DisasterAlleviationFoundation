using System;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Models
{
    public class Disaster
    {
        public int DisasterId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.Now;
        public string Severity { get; set; } // Low, Medium, High
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Resolved
        public string EvidenceFileName { get; set; } // optional file upload

        // Foreign key to IdentityUser
        public string UserId { get; set; } // existing FK
        public IdentityUser User { get; set; } // navigation property

        // **New property to track who created the disaster**
        public string CreatedBy { get; set; }
    }
}
