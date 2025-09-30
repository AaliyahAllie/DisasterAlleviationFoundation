using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Models
{
    public class Volunteer
    {
        public int VolunteerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
