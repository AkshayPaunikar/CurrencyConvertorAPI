using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using CurrencyConvertorAPI.Constants;
using CurrencyConvertorAPI.Entities;
using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


namespace CurrencyConvertorAPI.Repositories
{
    internal class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly string _filePath;        
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CurrencyRateRepository(IWebHostEnvironment environment)
        {
            var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            _filePath = Path.Combine(dataDirectory, CurrencyConstants.CurrencyRatesFileName);
        }

        public async Task<IEnumerable<CurrencyRate>> GetAllRatesAsync()
        {
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<CurrencyRate>();
            }

            try
            {
                await _semaphore.WaitAsync();
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<CurrencyRate>>(json) ?? new List<CurrencyRate>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error reading currency rates from file");
                return Enumerable.Empty<CurrencyRate>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<CurrencyRate?> GetRateByCodeAsync(string currencyCode)
        {
            var rates = await GetAllRatesAsync();
            return rates.FirstOrDefault(r => r.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        }

        public async Task SaveRatesAsync(IEnumerable<CurrencyRate> rates)
        {
            try
            {
                await _semaphore.WaitAsync();
                var json = JsonSerializer.Serialize(rates, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving currency rates to file");
                
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
