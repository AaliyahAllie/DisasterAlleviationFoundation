using System;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        public int DonationId { get; set; }
        public string DonorName { get; set; }
        public string ResourceType { get; set; }
        public int Quantity { get; set; }
        public DateTime DateDonated { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
