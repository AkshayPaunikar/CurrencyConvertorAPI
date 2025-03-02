using CurrencyConvertorAPI.Entities;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface IUserService
    {
        bool ValidateUser(string username, string password, out User user);
    }
}
