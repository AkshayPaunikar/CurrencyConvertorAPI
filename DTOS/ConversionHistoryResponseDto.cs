using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.DTOS
{
    public class ConversionHistoryResponseDto
    {
        public Guid Id { get; set; }
        public string FromCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string ToCurrency { get; set; } = "DKK";
        public DateTime ConversionDate { get; set; }
    }
}
