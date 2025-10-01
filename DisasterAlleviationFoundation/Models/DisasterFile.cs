using DisasterAlleviationFoundation.Models;

public class DisasterFile
{
    public int FileId { get; set; }           // Primary Key
    public int DisasterId { get; set; }       // Foreign Key
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.Now;

    public Disaster Disaster { get; set; }    // Navigation property
}
