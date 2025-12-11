namespace Accura.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string PatientName { get; set; }
        public string Service { get; set; }
        public string Doctor { get; set; }
        public long Amount { get; set; }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour