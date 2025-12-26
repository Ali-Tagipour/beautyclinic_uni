using System.Collections.Generic;

namespace beautyclinic_uni.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalPayments { get; set; }
        public int TotalServices { get; set; }
        public int PendingAppointments { get; set; }
        public int TodayAppointments { get; set; }

        public List<ContactRequestItem> RecentContactRequests { get; set; }
            = new();
    }

    public class ContactRequestItem
    {
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? CreatedAt { get; set; }
    }
}
