using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public int Age { get; set; }

        [MaxLength(100)]
        public string? Service { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public string? Details { get; set; }
    }
}
