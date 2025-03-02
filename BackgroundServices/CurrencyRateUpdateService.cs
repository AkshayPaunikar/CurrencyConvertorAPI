using CurrencyConvertorAPI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.BackgroundServices
{
    internal class CurrencyRateUpdateService : BackgroundService
    {
        
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(60);

        public CurrencyRateUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Currency Rate Update Service is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Updating currency rates at: {time}", DateTime.Now);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var currencyRateService = scope.ServiceProvider.GetRequiredService<ICurrencyRateService>();
                    await currencyRateService.UpdateRatesAsync();
                    Log.Information("Currency rates updated successfully");
                }
                catch (Exception ex)
                {
                    Log.Error("Error occurred while updating currency rates - {ex}", ex);
                }

                await Task.Delay(_updateInterval, stoppingToken);
            }
        }
    }
}

