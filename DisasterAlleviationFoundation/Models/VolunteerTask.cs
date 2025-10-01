using System;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }

        public int? AssignedVolunteerId { get; set; }
        public Volunteer? AssignedVolunteer { get; set; }

        // Optional: add Location if you want it in the table
        public string? Location { get; set; }

        
    }
}
