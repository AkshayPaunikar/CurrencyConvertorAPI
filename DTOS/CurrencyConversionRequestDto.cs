using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.DTOS
{
    public class CurrencyConversionRequestDto
    {
        public string FromCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
