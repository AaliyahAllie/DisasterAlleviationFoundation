using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        public string? UserId { get; set; } // Nullable to avoid required error

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "";

        public int? Quantity { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = "Pending"; // Pending / Distributed

        public DateTime DateDonated { get; set; } = DateTime.Now;
    }
}
