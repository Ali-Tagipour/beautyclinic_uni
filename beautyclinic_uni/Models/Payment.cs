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
        public DateTime Date { get; set; }

        [Required, MaxLength(100)]
        public string PatientName { get; set; }

        [MaxLength(100)]
        public string Service { get; set; }

        [MaxLength(100)]
        public string Doctor { get; set; }

        public long Amount { get; set; }
    }
}
