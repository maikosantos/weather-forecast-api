using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using weather_forecast_api.Dto;

namespace weather_forecast_api.Services
{
    public interface IWeatherForecastService
    {
        Task<Coordinates> GetCoordinatesByAddress(Address address);
        Task<string> GetForecastUrl(Coordinates coordinates);
        Task<IActionResult> GetForecast(string forecastUrl);

    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions _option;
        private ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(HttpClient httpClient, IOptions option, ILogger<WeatherForecastService> logger)
        {
            _httpClient = httpClient;
            _option = option;
            _logger = logger;
        }

        public async Task<Coordinates> GetCoordinatesByAddress(Address address)
        {
            _logger.LogInformation("GetCoordinatesByAddress called with address: {address}", address.FullAddress);

            try
            {
                var geocodingRequestUri = $"{_option.GeocoderApiUrl}/locations/onelineaddress?address={address.FullAddress}&benchmark=Public_AR_Current&format=json";

                var geocodingResponse = await _httpClient.GetAsync(geocodingRequestUri);

                var geocodingContent = geocodingResponse.Content;

                if (geocodingResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully got coordinates for address: {address}", address.FullAddress);
                    var geocodingResult = await geocodingContent.ReadAsStringAsync();
                    var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(geocodingResult, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return weatherResponse.Result.AddressMatches[0].Coordinates;
                }
                else
                {
                    _logger.LogError("Error getting coordinates for address: {address}", address.FullAddress);
                    throw new Exception($"Error getting coordinates for address: {address}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting coordinates for address: {address}", address.FullAddress);
                throw;
            }
        }

        public async Task<string> GetForecastUrl(Coordinates coordinates)
        {
            try
            {
                //https://api.weather.gov/points/38.846,-76.9275
                //https://api.weather.gov/points/{latitude},{longitude}

                var weatherRequestUri = $"{_option.WeatherApiUrl}/points/{coordinates.y},{coordinates.x}";

                _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.noaa.dwml+xml");

                var weatherResponse = await _httpClient.GetAsync(weatherRequestUri);

                var weatherContent = weatherResponse.Content;

                if (weatherResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully got URL forecast for coordinates: {x}, {y}", coordinates.x, coordinates.y);

                    var jsonString = await weatherContent.ReadAsStringAsync();
                  
                    JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

                    JsonElement root = jsonDocument.RootElement;
                    string urlForecast = root.GetProperty("properties").GetProperty("forecast").GetString();

                    return urlForecast;
                }
                else
                {
                    _logger.LogError("Error getting URL forecast for coordinates: {x}, {y}", coordinates.x, coordinates.y);
                    throw new Exception($"Error getting URL forecast for coordinates: {coordinates.x} {coordinates.y}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting URL forecast for coordinates: {x}, {y}", coordinates.x, coordinates.y);
                throw;
            }
        }

        public async Task<IActionResult> GetForecast(string forecastUrl)
        {
            try
            {
                //https://api.weather.gov/points/38.846,-76.9275
                //https://api.weather.gov/points/{latitude},{longitude}

                _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/ld+json"));

                var weatherResponse = await _httpClient.GetAsync(forecastUrl);
                var weatherContent = weatherResponse.Content;

                if (weatherResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully got forecast for url: {url}", forecastUrl);
                   
                    var jsonString = await weatherContent.ReadAsStringAsync();

                    return new OkObjectResult(jsonString);

                }
                else
                {
                    _logger.LogError("Error getting forecast for url: {url}", forecastUrl);
                    throw new Exception($"Error getting forecast for url: {forecastUrl}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting forecast for url: {forecastUrl}");
                throw;
            }

        }
    }
}
