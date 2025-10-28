namespace DisasterAlleviationFoundation.Models
{
    // Dashboard ViewModel
    public class HomeDashboardViewModel
    {
        public List<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
        public List<VolunteerTask> VolunteerTasks { get; set; } = new List<VolunteerTask>();
    }
}
