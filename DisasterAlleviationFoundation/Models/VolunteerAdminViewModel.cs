using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerAdminViewModel
    {
        public Volunteer Volunteer { get; set; }
        public VolunteerAssignment Assignment { get; set; } // can be null if unassigned
    }
}
