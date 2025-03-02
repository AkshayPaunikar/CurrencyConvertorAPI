using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CurrencyConvertorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversionService _conversionService;

        public CurrencyConversionController(ICurrencyConversionService conversionService)
        {
            _conversionService = conversionService;
        }


        [HttpPost("convert")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> ConvertToDkk([FromBody] CurrencyConversionRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FromCurrency))
            {
                return BadRequest("Currency code is required");
            }

            if (request.Amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }
            Log.Information("Converting {Amount} {Currency} to DKK", request.Amount, request.FromCurrency);
            var result = await _conversionService.ConvertToDkkAsync(request);
            return Ok(result);
        }


        [HttpGet("history")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetConversionHistory([FromQuery] ConversionHistoryRequestDto filter)
        {
            if(filter.FromDate == null)
            {
                return BadRequest("From Date cannot be empty");
            }

            if (filter.ToDate == null)
            {
                return BadRequest("To Date cannot be empty");
            }

            if(filter.Currency == null)
            {
                return BadRequest("Currency cannot be empty");
            }

            Log.Information("Getting conversion history with filters: {@Filter}", filter);
            var history = await _conversionService.GetConversionHistoryAsync(filter);

            return Ok(history);
        }
    }
}

