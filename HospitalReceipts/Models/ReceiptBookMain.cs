namespace HospitalReceipts.Models
{
    public class ReceiptBookMain
    {
        public int BookId { get; set; }
        public string DoctorName { get; set; } = "";
        public string Header1 { get; set; } = "";
        public string Header2 { get; set; } = "";
        public string Header3 { get; set; } = "";
        public decimal DefaultAmount { get; set; }
        public string DefaultTowards { get; set; } = "";
        public int NextReceiptNo { get; set; }
    }
}
