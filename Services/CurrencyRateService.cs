using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Services
{
    internal class CurrencyRateService : ICurrencyRateService
    {
        private readonly ICurrencyRateRepository _repository;
        private readonly INationalbankClientService _nationalbankClient;

        public CurrencyRateService(
            ICurrencyRateRepository repository,
            INationalbankClientService nationalbankClient)
        {
            _repository = repository;
            _nationalbankClient = nationalbankClient;
        }

        public async Task<IEnumerable<CurrencyRateDto>> GetAllRatesAsync()
        {
            var rates = await _repository.GetAllRatesAsync();

            if (!rates.Any())
            {
                await UpdateRatesAsync();
                rates = await _repository.GetAllRatesAsync();
            }

            return rates.Select(r => new CurrencyRateDto
            {
                CurrencyCode = r.CurrencyCode,
                Name = r.Name,
                Rate = r.Rate,
                LastUpdated = r.LastUpdated
            });
        }

        public async Task<CurrencyRateDto?> GetRateByCodeAsync(string currencyCode)
        {
            var rate = await _repository.GetRateByCodeAsync(currencyCode);

            if (rate != null)
            {
                return new CurrencyRateDto
                {
                    CurrencyCode = rate.CurrencyCode,
                    Name = rate.Name,
                    Rate = rate.Rate,
                    LastUpdated = rate.LastUpdated
                };
            }
            else { return null; }


        }

        public async Task<bool> UpdateRatesAsync()
        {
            var latestRates = await _nationalbankClient.FetchLatestRatesAsync();
            if (latestRates != null)
            {
                await _repository.SaveRatesAsync(latestRates);
                return true;
            }
            else
            {
                return false;
            }
            

        }
    }
}
