namespace DisasterAlleviationFoundation.Models
{
    public class Disaster
    {
       
        public int DisasterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateReported { get; set; }
        public string Severity { get; set; } // Low / Mild / High
        public string? UserId { get; set; }

        public ICollection<DisasterFile> DisasterFiles { get; set; } = new List<DisasterFile>();
        
    }

}