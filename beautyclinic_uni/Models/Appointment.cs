using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(10)]
        public string Time { get; set; }

        [MaxLength(100)]
        public string Service { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public string Note { get; set; }
    }
}
