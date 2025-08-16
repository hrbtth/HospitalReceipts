
using Microsoft.Data.Sqlite;
using HospitalReceipts.Models;

namespace HospitalReceipts.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
        public List<ReceiptBookMain> GetBooks()
        {
            var books = new List<ReceiptBookMain>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT BookId, DoctorName, Header1, Header2, Header3, DefaultAmount, DefaultTowards, NextReceiptNo FROM ReceiptBookMain";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(new ReceiptBookMain
                {
                    BookId = reader.GetInt32(0),
                    BookName = reader.GetString(1),
                    Header1 = reader.GetString(2),
                    Header2 = reader.GetString(3),
                    Header3 = reader.GetString(4),
                    DefaultAmount = reader.GetDecimal(5),
                    DefaultTowards = reader.GetString(6),
                    NextReceiptNo = reader.GetInt32(7)
                });
            }
            return books;
        }
        public void AddBook(ReceiptBookMain book)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO ReceiptBookMain 
        (DoctorName, Header1, Header2, Header3, DefaultAmount, DefaultTowards, NextReceiptNo)
        VALUES ($doctor, $h1, $h2, $h3, $amt, $towards, $next)";

            cmd.Parameters.AddWithValue("$doctor", book.BookName);
            cmd.Parameters.AddWithValue("$h1", book.Header1);
            cmd.Parameters.AddWithValue("$h2", book.Header2);
            cmd.Parameters.AddWithValue("$h3", book.Header3);
            cmd.Parameters.AddWithValue("$amt", book.DefaultAmount);
            cmd.Parameters.AddWithValue("$towards", book.DefaultTowards);
            cmd.Parameters.AddWithValue("$next", book.NextReceiptNo);

            cmd.ExecuteNonQuery();
        }

        public void UpdateBook(ReceiptBookMain book)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE ReceiptBookMain SET 
        DoctorName=$doctor, Header1=$h1, Header2=$h2, Header3=$h3, 
        DefaultAmount=$amt, DefaultTowards=$towards, NextReceiptNo=$next 
        WHERE BookId=$id";

            cmd.Parameters.AddWithValue("$doctor", book.BookName);
            cmd.Parameters.AddWithValue("$h1", book.Header1);
            cmd.Parameters.AddWithValue("$h2", book.Header2);
            cmd.Parameters.AddWithValue("$h3", book.Header3);
            cmd.Parameters.AddWithValue("$amt", book.DefaultAmount);
            cmd.Parameters.AddWithValue("$towards", book.DefaultTowards);
            cmd.Parameters.AddWithValue("$next", book.NextReceiptNo);
            cmd.Parameters.AddWithValue("$id", book.BookId);

            cmd.ExecuteNonQuery();
        }

        public void DeleteBook(int id)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM ReceiptBookMain WHERE BookId=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

    }
}
