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
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
