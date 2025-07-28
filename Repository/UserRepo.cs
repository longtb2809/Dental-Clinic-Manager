using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class UserRepo
    {
        private readonly DentalClinicDbContext _context;

        public UserRepo(DentalClinicDbContext context) // 👈 Truyền context từ ngoài vào
        {
            _context = context;
        }

        public User? GetByEmailandPassword(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public bool AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (existingUser == null) return false;

                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Gender = user.Gender;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Address = user.Address;
                existingUser.Username = user.Username;
                existingUser.RoleId = user.RoleId;
                existingUser.IsActive = user.IsActive;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    existingUser.PasswordHash = user.PasswordHash;
                }

                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }
    }
}
