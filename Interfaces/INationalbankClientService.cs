using CurrencyConvertorAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface INationalbankClientService
    {
        Task<IEnumerable<CurrencyRate>> FetchLatestRatesAsync();
    }
}
