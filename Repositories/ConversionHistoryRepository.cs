using CurrencyConvertorAPI.Constants;
using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Entities;
using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Repositories
{
    internal class ConversionHistoryRepository : IConversionHistoryRepository
    {
        private readonly string _filePath;        
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ConversionHistoryRepository(IWebHostEnvironment environment)
        {
            var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            _filePath = Path.Combine(dataDirectory, CurrencyConstants.ConversionHistoryFileName);

            // Create the file if it does not exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<IEnumerable<ConversionHistory>> GetConversionHistoryAsync(ConversionHistoryRequestDto filter)
        {
            try
            {
                await _semaphore.WaitAsync();
                var json = await File.ReadAllTextAsync(_filePath);
                var allHistory = JsonSerializer.Deserialize<List<ConversionHistory>>(json) ?? new List<ConversionHistory>();

                var filteredHistory = allHistory.AsEnumerable();

                if (filter.FromDate.HasValue)
                {
                    filteredHistory = filteredHistory.Where(h => h.ConversionDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    filteredHistory = filteredHistory.Where(h => h.ConversionDate <= filter.ToDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.Currency))
                {
                    filteredHistory = filteredHistory.Where(h =>
                        h.FromCurrency.Equals(filter.Currency, StringComparison.OrdinalIgnoreCase));
                }

                return filteredHistory.OrderByDescending(h => h.ConversionDate).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error reading conversion history from file");
                return Enumerable.Empty<ConversionHistory>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SaveConversionAsync(ConversionHistory conversion)
        {
            try
            {
                await _semaphore.WaitAsync();

                var json = await File.ReadAllTextAsync(_filePath);
                var history = JsonSerializer.Deserialize<List<ConversionHistory>>(json) ?? new List<ConversionHistory>();

                history.Add(conversion);

                json = JsonSerializer.Serialize(history, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving conversion history to file");
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
