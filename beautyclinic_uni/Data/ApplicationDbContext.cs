using Microsoft.EntityFrameworkCore;
using beautyclinic_uni.Models;

namespace beautyclinic_uni.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet های مورد نیاز پروژه
        public DbSet<User> Users { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
    }
}
