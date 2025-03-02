# Currency Converter API

A RESTful API for currency conversion to Danish Krone (DKK) using the latest exchange rates from Nationalbanken.

## Features

- Fetch and store currency exchange rates from the Nationalbanken API
- Scheduled update of currency rates every 60 minutes
- RESTful API endpoints for:
  - Retrieving current exchange rates
  - Converting amounts between currencies and DKK
  - Storing and retrieving conversion history with filtering options
- File-based storage for both currency rates and conversion history
- Error handling and logging
- Swagger API documentation

## Technical Stack

- **Language**: C# 
- **Framework**: .NET 8
- **API**: RESTful with proper HTTP methods (GET, POST)
- **Documentation**: Swagger/OpenAPI
- **Storage**: File-based with JSON serialization

## API Endpoints

### Currency Rates

- **GET /api/CurrencyRates**: Get all available currency rates
- **GET /api/CurrencyRates/{code}**: Get currency rate by code
- **POST /api/CurrencyRates/update**: Manually trigger an update of currency rates

### Currency Conversion

- **POST /api/CurrencyConversion/convert**: Convert an amount from a specified currency to DKK
- **GET /api/CurrencyConversion/history**: Get conversion history with optional filtering



## File Storage

The application stores data in JSON files located in the `Data` directory of the web project:

- `currency_rates.json`: Stores the latest currency rates
- `conversion_history.json`: Stores the conversion history
