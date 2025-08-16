namespace HospitalReceipts.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public int BookId { get; set; }
        public int ReceiptNo { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = "";
        public decimal Amount { get; set; }
        public string Towards { get; set; } = "";
    }
}
