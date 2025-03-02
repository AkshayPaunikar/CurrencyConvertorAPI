using CurrencyConvertorAPI.Entities;
using CurrencyConvertorAPI.Interfaces;
using Serilog;
using System.Xml.Linq;

namespace CurrencyConvertorAPI.Clients
{
    internal class NationalbankClientService : INationalbankClientService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public NationalbankClientService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CurrencyRate>> FetchLatestRatesAsync()
        {
            try
            {
                string NationalbankApiUrl = _configuration["CurrencyApi:NationalbankApiUrl"];
                var response = await _httpClient.GetAsync(NationalbankApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var rates = ParseCurrencyRatesFromXml(content);

                Log.Information("Successfully fetched {Count} currency rates from Nationalbanken", rates.Count());
                return rates;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching currency rates from Nationalbanken");
                return null;
            }
        }


        private IEnumerable<CurrencyRate> ParseCurrencyRatesFromXml(string xml)
        {
            var currencyRates = new List<CurrencyRate>();
            var now = DateTime.Now;

            try
            {
                var doc = XDocument.Parse(xml);

                currencyRates = doc.Descendants("currency")
                .Select(currency => new CurrencyRate
                {
                    CurrencyCode = (string)currency.Attribute("code"),
                    Name = (string)currency.Attribute("desc"),
                    Rate = decimal.Parse((string)currency.Attribute("rate"), System.Globalization.CultureInfo.InvariantCulture),
                    LastUpdated = now
                })
                .ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error parsing currency rates XML", ex);
                throw;
            }

            return currencyRates;
        }
    }

}
