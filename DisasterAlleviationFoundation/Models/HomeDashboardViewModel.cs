namespace DisasterAlleviationFoundation.Models
{
    // Dashboard ViewModel
    public class HomeDashboardViewModel
    {
        public List<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
        public List<VolunteerTaskViewModel> VolunteerTasks { get; set; } = new List<VolunteerTaskViewModel>();
    }
}
