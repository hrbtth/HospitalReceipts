using System.ComponentModel.DataAnnotations;

namespace HospitalReceipts.Models
{
    public class ReceiptBookMain
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string BookName { get; set; } = string.Empty;

        public string? Header1 { get; set; }
        public string? Header2 { get; set; }
        public string? Header3 { get; set; }

        // Column is REAL in SQLite → use double? for clean mapping
        public decimal? DefaultAmount { get; set; }

        public string? DefaultTowards { get; set; }

        [Required]
        public int NextReceiptNo { get; set; }
    }
}
