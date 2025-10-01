namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerTaskViewModel
    {
        // Task info
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }

        // Assigned volunteer info (nullable)
        public int? AssignedVolunteerId { get; set; }
        public string? AssignedVolunteerName { get; set; }

        // Optional: Disaster info if needed
        public int? DisasterId { get; set; }
        public string? DisasterTitle { get; set; }

        // Convenience property for display
        public string AssignedInfo => AssignedVolunteerName != null ? $"{AssignedVolunteerName}" : "Unassigned";
    }
}
