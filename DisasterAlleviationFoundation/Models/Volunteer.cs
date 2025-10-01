namespace DisasterAlleviationFoundation.Models
{
    public class Volunteer
    {
        public int VolunteerId { get; set; } // PK
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Skills { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;

        
    }
}
