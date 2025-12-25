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

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
