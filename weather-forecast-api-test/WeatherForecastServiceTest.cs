using weather_forecast_api.Services;
using weather_forecast_api.Dto;
using weather_forecast_api;

namespace weather_forecast_api_test
{
    public class WeatherForecastServiceTest
    {
        private readonly Mock<ILogger<WeatherForecastService>> _mockLogger = new Mock<ILogger<WeatherForecastService>>();
        private readonly Mock<IOptions<Options>> _mockOptions = new Mock<IOptions<WeatherForecastApiOptions>>();
        private readonly Mock<HttpClient> _mockHttpClient = new Mock<HttpClient>();
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastServiceTest()
        {
            _weatherForecastService = new WeatherForecastService(_mockHttpClient.Object, _mockOptions.Object, _mockLogger.Object);
        }

        
        [Fact(DisplayName = "Return Coordinates - Success")]
        public async Task GetCoordinatesByAddress()
        {
            //Arrange   
            var address = new Address()
            {
                Address = "1600 Pennsylvania Ave NW, Washington, DC 20500"
            };

            //Act
            var result = await _weatherForecastService.GetCoordinatesByAddress(address);

            //Assert    
            Assert.NotNull(result);
            Assert.Contains("https://api.weather.gov/gridpoints", result);
        }

        [Fact(DisplayName = "Return Coordinates - Failure")]
        public async Task GetCoordinatesByAddress()
        {
            //Arrange   
            var address = new Address()
            {
                Address = "1600 Pennsylvania Ave NW, Washington, DC 20500"
            };

            //Act
            var result = await _weatherForecastService.GetCoordinatesByAddress(address);

            //Assert    
            Assert.NotNull(result);
        }
    }
}