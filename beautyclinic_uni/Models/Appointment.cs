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
        public string PatientName { get; set; } = null!;

        
        [Required, MaxLength(50)]
        [Column("AppointmentDate")]
        public string AppointmentDateTime { get; set; } = null!;

        [MaxLength(100)]
        public string? Service { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "در انتظار";

        public string? Note { get; set; }
    }
}
