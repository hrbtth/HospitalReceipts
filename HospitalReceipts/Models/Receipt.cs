using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalReceipts.Models
{
    public class Receipt
    {
        [Key]
        public int ReceiptId { get; set; }

        [ForeignKey(nameof(ReceiptBookMain))]
        public int BookId { get; set; }

        [Required]
        public int ReceiptNo { get; set; }

        // Stored as TEXT in SQLite; DateTime maps fine (SQLite stores as TEXT)
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Column is REAL → use double for clean mapping
        [Required]
        public double Amount { get; set; }

        public string? Towards { get; set; }

        public int Printed { get; set; } = 0;

        // optional nav
        public ReceiptBookMain? ReceiptBookMain { get; set; }
    }
}
