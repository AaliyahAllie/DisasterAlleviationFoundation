using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Disaster Incident Reporting
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<DisasterFile> DisasterFiles { get; set; }

        // Resource Donations
        public DbSet<Donation> Donations { get; set; }

        // Volunteers and Tasks
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<VolunteerAssignment>()
    .HasKey(va => va.AssignmentId);

            builder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Volunteer)
                .WithMany()
                .HasForeignKey(va => va.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Disaster)
                .WithMany()
                .HasForeignKey(va => va.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Disaster)
                .WithMany()
                .HasForeignKey(va => va.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Disaster -> User
            builder.Entity<Disaster>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // DisasterFile -> Disaster
            builder.Entity<DisasterFile>()
                .HasKey(df => df.FileId);
            builder.Entity<DisasterFile>()
                .HasOne(df => df.Disaster)
                .WithMany(d => d.DisasterFiles)
                .HasForeignKey(df => df.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Donation -> User
            builder.Entity<Donation>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Volunteer -> User
            builder.Entity<Volunteer>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // VolunteerTask: only PK, optional AssignedVolunteerId as reference, no navigation
            builder.Entity<VolunteerTask>()
                .HasKey(t => t.TaskId);

            builder.Entity<VolunteerTask>()
                .Property(t => t.AssignedVolunteerId)
                .IsRequired(false); // Nullable for unassigned tasks
        }
    }
}
