using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accura.Models
{
    [Table("Consults")]
    public class Consult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserMessage { get; set; }

        public string AiResponse { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
