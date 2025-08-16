
using Microsoft.EntityFrameworkCore;
using HospitalReceipts.Models;

namespace HospitalReceipts.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tables
        public DbSet<ReceiptBookMain> ReceiptBookMain { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map ReceiptBookMain
            modelBuilder.Entity<ReceiptBookMain>(entity =>
            {
                entity.ToTable("ReceiptBookMain");   // ensure table name matches
                entity.HasKey(e => e.BookId);        // primary key
            });

            // Map Receipt
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");           // ensure table name matches
                entity.HasKey(e => e.ReceiptId);     // primary key

                // foreign key relation
                entity.HasOne<ReceiptBookMain>()
                      .WithMany()
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
