using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        // اگر دیتابیس فعلیت تاریخ را رشته ذخیره می‌کند، این نگه داشته می‌شود.
        // در آینده بهتر است به DateTime تغییر داده و Migration بگیری.
        [Required]
        [MaxLength(20)]
        public string Date { get; set; } = null!;

        [Required, MaxLength(100)]
        public string PatientName { get; set; } = null!;

        [MaxLength(100)]
        public string? Service { get; set; }

        [MaxLength(100)]
        public string? Doctor { get; set; }

        // استفاده از decimal برای مقادیر پولی
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
