using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beautyclinic_uni.Models
{
    [Table("ContactRequests")]
    public class ContactRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "شماره تماس الزامی است.")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "پیام الزامی است.")]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CreatedAt { get; set; } = string.Empty;
    }
}


// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour