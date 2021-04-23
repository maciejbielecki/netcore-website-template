using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebAppTemplate.Data;
using WebAppTemplate.Models.Request;

namespace WebAppTemplate.Services
{
    public interface IUserService
    {
        User Login(Login request);
        User Register(Register request);
        bool UserAuthorized(string email, string role);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Login(Login request)
        {
            AddUser();
            var user = _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserData)
                .FirstOrDefault(u => u.Email == request.Email);
            user = BCrypt.Net.BCrypt.Verify(request.Password, user.Password)
                ? user
                : null;
            return user;
        }

        public User Register(Register request)
        {
            var user = AddUser();
            return user;
        }

        public bool UserAuthorized(string email, string role)
        {
            return _context.Users
                .Include(u => u.Role)
                .Any(u => u.Email == email && u.Role.Name == role);
        }

        private User AddUser()
        {
            User user = null;
            if (!_context.Users.Any())
            {
                user = _context.Users.Add(new User() { Email = "asd", Password = BCrypt.Net.BCrypt.HashPassword("asd"), UserData = new UserData() { FirstName = "Mac", LastName = "Biel" }, Role = new Role() { Id = 1, Name = "admin", RolePrivileges = new List<RolePrivilege>() } }).Entity;
                _context.SaveChanges();
            }
            return user;
        }
    }
}
