using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Date { get; set; } = null!;

        [Required, MaxLength(100)]
        public string PatientName { get; set; } = null!;

        [MaxLength(100)]
        public string? Service { get; set; }

        [MaxLength(100)]
        public string? Doctor { get; set; }

        // تغییر از long به decimal → سازگار با دیتابیس واقعی
        [Column(TypeName = "decimal(18,2)")]  // یا money در دیتابیس
        public decimal Amount { get; set; }
    }
}