using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalReceipts.Migrations
{
    /// <inheritdoc />
    public partial class FixReceiptForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptBookMain",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookName = table.Column<string>(type: "TEXT", nullable: false),
                    Header1 = table.Column<string>(type: "TEXT", nullable: true),
                    Header2 = table.Column<string>(type: "TEXT", nullable: true),
                    Header3 = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultAmount = table.Column<decimal>(type: "TEXT", nullable: true),
                    DefaultTowards = table.Column<string>(type: "TEXT", nullable: true),
                    NextReceiptNo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptBookMain", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Receipt",
                columns: table => new
                {
                    ReceiptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiptNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    Towards = table.Column<string>(type: "TEXT", nullable: true),
                    Printed = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiptBookMainBookId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipt", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_Receipt_ReceiptBookMain_BookId",
                        column: x => x.BookId,
                        principalTable: "ReceiptBookMain",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receipt_ReceiptBookMain_ReceiptBookMainBookId",
                        column: x => x.ReceiptBookMainBookId,
                        principalTable: "ReceiptBookMain",
                        principalColumn: "BookId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_BookId",
                table: "Receipt",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ReceiptBookMainBookId",
                table: "Receipt",
                column: "ReceiptBookMainBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receipt");

            migrationBuilder.DropTable(
                name: "ReceiptBookMain");
        }
    }
}
