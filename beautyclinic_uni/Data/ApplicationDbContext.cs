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

        // جدول‌های اصلی
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ContactRequest> ContactRequests { get; set; } = null!;
        public DbSet<AiConsult> AiConsults { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<ServiceItem> Services { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<TrackingSession> Tracking { get; set; } = null!;
        public DbSet<Consult> Consults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // نام جداول (صریح)
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            modelBuilder.Entity<ServiceItem>().ToTable("Services");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<ContactRequest>().ToTable("ContactRequests");
            modelBuilder.Entity<AiConsult>().ToTable("AiConsults");
            modelBuilder.Entity<TrackingSession>().ToTable("TrackingSessions");
            modelBuilder.Entity<Consult>().ToTable("Consults");

            // پیکربندی ContactRequest طبق ستون‌هایی که گفتی:
            modelBuilder.Entity<ContactRequest>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName)
                      .HasMaxLength(100)
                      .IsRequired(); // نام الزامی

                entity.Property(e => e.Phone)
                      .HasMaxLength(20)
                      .IsRequired(); // شماره تماس الزامی

                entity.Property(e => e.Email)
                      .HasMaxLength(150)
                      .IsRequired(false); // ایمیل اختیاری

                entity.Property(e => e.Message)
                      .HasMaxLength(500)
                      .IsRequired(); // پیام الزامی

                // CreatedAt: بهتر است هنگام ذخیره پر شود؛ اجازه Nullable می‌دهیم
                entity.Property(e => e.CreatedAt)
                      .HasMaxLength(50)
                      .IsRequired(false);
            });

            // مثال پیکربندی برای User (اگر تفاوت نام فیلد داری آن را اصلاح کن)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                // اگر خاصیت در مدل اسم Fullname دارد، مطابق آن تنظیم کن
                entity.Property(e => e.Fullname)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.Email)
                      .HasMaxLength(150)
                      .IsRequired(false);
                entity.Property(e => e.Phone)
                      .HasMaxLength(20)
                      .IsRequired(false);
            });

            // (در صورت نیاز می‌توان تنظیمات بیشتری برای بقیه موجودیت‌ها اضافه کرد)
        }
    }
}
