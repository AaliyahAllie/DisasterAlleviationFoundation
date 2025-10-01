using DisasterAlleviationFoundation.Models;
namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerAssignment
    {
        public int AssignmentId { get; set; }

        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }

        public string TaskDescription { get; set; } = string.Empty; // <-- new

        public int DisasterId { get; set; }
        public Disaster Disaster { get; set; }

        public string Location { get; set; } = string.Empty;
        public DateTime DateAssigned { get; set; } = DateTime.Now;
    }

}