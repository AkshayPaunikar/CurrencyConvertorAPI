using CurrencyConvertorAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface ICurrencyRateService
    {
        Task<IEnumerable<CurrencyRateDto>> GetAllRatesAsync();
        Task<CurrencyRateDto?> GetRateByCodeAsync(string currencyCode);
        Task<bool> UpdateRatesAsync();
    }
}
