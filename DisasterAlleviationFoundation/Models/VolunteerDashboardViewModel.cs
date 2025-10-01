namespace DisasterAlleviationFoundation.Models
{
    // For Home/Index (volunteer dashboard)
    public class VolunteerDashboardViewModel
    {
        public Volunteer Volunteer { get; set; }
        public List<VolunteerTask> AssignedTasks { get; set; } = new List<VolunteerTask>();
    }

}
