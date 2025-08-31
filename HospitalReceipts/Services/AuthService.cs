using HospitalReceipts.Data;
using HospitalReceipts.Models;

namespace HospitalReceipts.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private AppUser? _currentUser;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public AppUser? CurrentUser => _currentUser;

        //  Simple Login (plain password)
        public bool Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return false;

            if (user.Password == password)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        public void Logout() => _currentUser = null;

        //  Only ADMIN can create new users
        public void CreateUser(string username, string password, string privilege)
        {
            if (_currentUser?.Privilege != "ADMIN")
                throw new UnauthorizedAccessException("Only ADMIN can create users");

            if (_context.Users.Any(u => u.UserName == username))
                throw new InvalidOperationException("User already exists");

            var newUser = new AppUser
            {
                UserName = username,
                Password = password,   //  plain password
                Privilege = privilege
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        //  ADMIN can change anyone's password, user can change own
        public void ChangePassword(string username, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return;

            if (_currentUser?.Privilege == "ADMIN" || _currentUser?.UserName == username)
            {
                user.Password = newPassword;
                _context.SaveChanges();
            }
        }
        public bool ChangeOwnPassword(string username, string oldPassword, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return false;

            if (user.Password == oldPassword) // verify old password
            {
                user.Password = newPassword;
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        // === New methods ===

        //  List all users (only for ADMIN)
        public List<AppUser> GetAllUsers()
        {
            if (_currentUser?.Privilege != "ADMIN")
                throw new UnauthorizedAccessException("Only ADMIN can view all users");

            return _context.Users.ToList();
        }

        //  Delete a user (only for ADMIN, and cannot delete self)
        public void DeleteUser(string username)
        {
            if (_currentUser?.Privilege != "ADMIN")
                throw new UnauthorizedAccessException("Only ADMIN can delete users");

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return;

            if (user.UserName == _currentUser.UserName)
                throw new InvalidOperationException("Admin cannot delete themselves");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        //  Optional: change privilege (only for ADMIN)
        public void ChangePrivilege(string username, string newPrivilege)
        {
            if (_currentUser?.Privilege != "ADMIN")
                throw new UnauthorizedAccessException("Only ADMIN can change privileges");

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return;

            user.Privilege = newPrivilege;
            _context.SaveChanges();
        }
    }
}
