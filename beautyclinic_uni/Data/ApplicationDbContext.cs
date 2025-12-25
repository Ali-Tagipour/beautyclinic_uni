using Accura.Models;
using beautyclinic_uni.Models;
using Microsoft.EntityFrameworkCore;

namespace beautyclinic_uni.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<AiConsult> AiConsults { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ServiceItem> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TrackingSession> Tracking { get; set; }
        public DbSet<Consult> Consults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            modelBuilder.Entity<ServiceItem>().ToTable("Services");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<ContactRequest>().ToTable("ContactRequests");

            modelBuilder.Entity<AiConsult>().ToTable("AiConsults");
            modelBuilder.Entity<TrackingSession>().ToTable("TrackingSessions");
            modelBuilder.Entity<Consult>().ToTable("Consults");
        }
    }
}
