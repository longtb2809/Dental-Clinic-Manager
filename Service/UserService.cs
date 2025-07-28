using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class UserService
    {
        private readonly DentalClinicDbContext _context;
        private readonly UserRepo _userRepo;

        public UserService()
        {
            _context = new DentalClinicDbContext();         // 👈 Duy nhất 1 DbContext dùng chung
            _userRepo = new UserRepo(_context);             // 👈 Truyền vào UserRepo
        }

        public User Login(string email, string password)
        {
            if (!email.Contains("@") || password.Length < 8)
                return null;

            return _userRepo.GetByEmailandPassword(email, password);
        }

        public List<User> GetDoctors()
        {
            return _context.Users
                .Where(u => u.RoleId == 2)
                .ToList();
        }

        public List<Patient> GetPatients()
        {
            return _context.Patients.Include(p => p.User).ToList();
        }

        public User GetUserById(int userId)
        {
            return _userRepo.GetUserById(userId);
        }

        public List<User> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public bool AddUser(User user)
        {
            return _userRepo.AddUser(user);
        }

        public bool UpdateUser(User user)
        {
            return _userRepo.UpdateUser(user);     // 👈 Sửa được lỗi không update
        }

        public bool DeleteUser(int userId)
        {
            return _userRepo.DeleteUser(userId);
        }
    }
}
