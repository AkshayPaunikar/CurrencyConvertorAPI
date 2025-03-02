using CurrencyConvertorAPI.Entities;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
