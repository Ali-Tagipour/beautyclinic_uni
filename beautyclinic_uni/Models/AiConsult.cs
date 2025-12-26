using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beautyclinic_uni.Models
{
    [Table("AiConsults")]
    public class AiConsult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserMessage { get; set; }

        public string AiResponse { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}


// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour