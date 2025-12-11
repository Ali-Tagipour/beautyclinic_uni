namespace beautyclinic_uni.Models
{
    public class User
    {
        public int Id { get; set; }

        // این را اضافه می‌کنیم تا ارور رفع شود
        public string Fullname { get; set; }

        // اگر خواستی ایمیل هم ذخیره بشه
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour