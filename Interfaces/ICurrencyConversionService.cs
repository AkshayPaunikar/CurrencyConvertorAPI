using CurrencyConvertorAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Interfaces
{
    public interface ICurrencyConversionService
    {

        Task<CurrencyConversionResponseDto> ConvertToDkkAsync(CurrencyConversionRequestDto request);
        Task<IEnumerable<ConversionHistoryResponseDto>> GetConversionHistoryAsync(ConversionHistoryRequestDto filter);
    }
}
