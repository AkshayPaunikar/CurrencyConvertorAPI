using CurrencyConvertorAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface ICurrencyRateRepository
    {
        Task<IEnumerable<CurrencyRate>> GetAllRatesAsync();
        Task<CurrencyRate?> GetRateByCodeAsync(string currencyCode);
        Task SaveRatesAsync(IEnumerable<CurrencyRate> rates);
    }
}
