using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("TrackingSessions")]
    public class TrackingSession
    {
        [Key]
        public int Id { get; set; }

        public int Session { get; set; }

        // آپدیت شد: در دیتابیس واقعی ستون Date از نوع varchar است
        // بنابراین باید string باشد تا با دیتابیس BeautyClinicDB سازگار شود
        [Required]
        [MaxLength(20)]
        public string Date { get; set; } = null!;  // مثال فرمت: "2025-12-25"

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}