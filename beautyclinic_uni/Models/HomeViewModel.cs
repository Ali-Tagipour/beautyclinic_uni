using System.Collections.Generic;

namespace beautyclinic_uni.Models
{
    public class HomeViewModel
    {
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalServices { get; set; }
        public decimal TotalPayments { get; set; }

        public List<object> RecentAppointments { get; set; } = new();
        public List<ContactRequest> RecentContactRequests { get; set; } = new();

        // فرم تماس داخل ویوو
        public ContactRequest ContactForm { get; set; } = new ContactRequest();
    }
}


// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour