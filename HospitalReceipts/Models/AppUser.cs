namespace HospitalReceipts.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Privilege { get; set; } = "USER"; // "ADMIN" or "USER"
    }
}
