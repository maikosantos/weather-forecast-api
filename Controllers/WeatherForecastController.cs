using Microsoft.AspNetCore.Mvc;
using weather_forecast_api.Dto;
using weather_forecast_api.Services;

namespace weather_forecast_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForcastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForcastService)
        {
            _logger = logger;
            _weatherForcastService = weatherForcastService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeatherForcast([FromBody] Address address)
        {
            //"1600 Pennsylvania Ave NW, Washington, DC 20500";

            var coordinates = await _weatherForcastService.GetCoordinatesByAddress(address);

            var urlForecast = await _weatherForcastService.GetForecastUrl(coordinates);

            var forecast = await _weatherForcastService.GetForecast(urlForecast);

            return forecast;

        }
    }
}