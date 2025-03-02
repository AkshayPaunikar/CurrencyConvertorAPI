using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertorAPI.Entities
{
    public class ConversionHistory
    {
        public Guid Id { get; set; }
        public string FromCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ConversionDate { get; set; }
    }
}
