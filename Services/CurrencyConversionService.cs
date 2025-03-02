using CurrencyConvertorAPI.Constants;
using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Entities;
using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Services
{
    internal class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly ICurrencyRateRepository _rateRepository;
        private readonly IConversionHistoryRepository _historyRepository;
        

        public CurrencyConversionService(
            ICurrencyRateRepository rateRepository,
            IConversionHistoryRepository historyRepository)
        {
            _rateRepository = rateRepository;
            _historyRepository = historyRepository;
        }

        public async Task<CurrencyConversionResponseDto> ConvertToDkkAsync(CurrencyConversionRequestDto request)
        {
            CurrencyConversionResponseDto response;
            
            if (request.FromCurrency.Equals(CurrencyConstants.BaseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                response = new CurrencyConversionResponseDto
                {
                    FromCurrency = request.FromCurrency,
                    Amount = request.Amount,
                    ConvertedAmount = request.Amount,
                    ToCurrency = CurrencyConstants.BaseCurrency,
                    ConversionDate = DateTime.UtcNow
                };

                await SaveConversionHistory(response);
                return response;
            }

            var rate = await _rateRepository.GetRateByCodeAsync(request.FromCurrency);

            var convertedAmount = Math.Round(request.Amount * (rate.Rate/100), 2);

            response = new CurrencyConversionResponseDto
            {
                FromCurrency = request.FromCurrency,
                Amount = request.Amount,
                ConvertedAmount = convertedAmount,
                ToCurrency = CurrencyConstants.BaseCurrency,
                ConversionDate = DateTime.UtcNow
            };

            await SaveConversionHistory(response);
            return response;
        }

        public async Task<IEnumerable<ConversionHistoryResponseDto>> GetConversionHistoryAsync(ConversionHistoryRequestDto filter)
        {
            var history = await _historyRepository.GetConversionHistoryAsync(filter);

            return history.Select(h => new ConversionHistoryResponseDto
            {
                Id = h.Id,
                FromCurrency = h.FromCurrency,
                Amount = h.Amount,
                ConvertedAmount = h.ConvertedAmount,
                ToCurrency = CurrencyConstants.BaseCurrency,
                ConversionDate = h.ConversionDate
            });
        }

        private async Task SaveConversionHistory(CurrencyConversionResponseDto conversion)
        {
            var historyEntry = new ConversionHistory
            {
                Id = Guid.NewGuid(),
                FromCurrency = conversion.FromCurrency,
                Amount = conversion.Amount,
                ConvertedAmount = conversion.ConvertedAmount,
                ConversionDate = conversion.ConversionDate
            };

            await _historyRepository.SaveConversionAsync(historyEntry);
        }
    }
}
