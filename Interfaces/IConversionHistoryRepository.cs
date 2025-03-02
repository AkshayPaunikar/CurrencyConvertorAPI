using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface IConversionHistoryRepository
    {
        Task<IEnumerable<ConversionHistory>> GetConversionHistoryAsync(ConversionHistoryRequestDto filter);
        Task SaveConversionAsync(ConversionHistory conversion);
    }
}
