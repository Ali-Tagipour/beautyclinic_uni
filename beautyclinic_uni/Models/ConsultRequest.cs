using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beautyclinic_uni.Models
{
    [Table("ConsultRequests")]
    public class ConsultRequest
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Fullname { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(150)]
        public string Subject { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
