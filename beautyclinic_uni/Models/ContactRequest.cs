namespace beautyclinic_uni.Models
{
    public class ContactRequest
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
