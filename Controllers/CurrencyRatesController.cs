using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CurrencyConvertorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ICurrencyRateService _currencyRateService;

        public CurrencyRatesController(ICurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRates()
        {
            Log.Information("Getting all currency rates");

            var rates = await _currencyRateService.GetAllRatesAsync();

            return Ok(rates);
        }


        [HttpGet("{code}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetRateByCode(string code)
        {
            Log.Information("Getting currency rate for code: {code}", code);
            var rate = await _currencyRateService.GetRateByCodeAsync(code);

            if (rate == null)
            {
                Log.Information("Currency with code {code} not found");
                return NotFound($"Currency with code {code} not found");
            }
            return Ok(rate);
        }

        [HttpPost("update")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateRates()
        {
            Log.Information("Manually updating currency rates");
            bool res = await _currencyRateService.UpdateRatesAsync();
            if (res)
            {
                string message = $"Update Successful at {DateTime.Now}";
                return Ok(message);
            }

            return Ok("Service unavailable");


        }
    }
}

