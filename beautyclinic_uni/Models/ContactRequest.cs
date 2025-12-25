using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beautyclinic_uni.Models
{
    [Table("ContactRequests")]
    public class ContactRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Fullname { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [MaxLength(150)]
        public string? Subject { get; set; }

        [Required]
        public string Message { get; set; } = null!;

        [MaxLength(20)]
        public string CreatedAt { get; set; } = null!;
    }
}
