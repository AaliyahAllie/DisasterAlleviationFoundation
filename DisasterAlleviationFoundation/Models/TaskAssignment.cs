using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterAlleviationFoundation.Models
{
    public class TaskAssignment
    {
        [Key] // <-- EF needs this
        public int TaskId { get; set; }

        [Required]
        public string TaskName { get; set; }

        public string Status { get; set; } = "Pending";

        [ForeignKey("Disaster")]
        public int DisasterId { get; set; }
        public Disaster Disaster { get; set; }

        [ForeignKey("Volunteer")]
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
