using CurrencyConvertorAPI.Entities;
using CurrencyConvertorAPI.Interfaces;

namespace CurrencyConvertorAPI.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;

        public UserService()
        {            
            _users = new List<User>
            {
            new User { Username = "admin", Password = "admin123", Role = "Admin" },
            new User { Username = "user", Password = "user123", Role = "User" }
            };
        }

        public bool ValidateUser(string username, string password, out User user)
        {
            user = _users.FirstOrDefault(u =>
                u.Username.Equals(username) && u.Password.Equals(password));

            return user != null;
        }
    }
}
