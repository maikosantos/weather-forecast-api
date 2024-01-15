namespace weather_forecast_api
{
    
    public interface IOptions
    {
        string GeocoderApiUrl { get; set; }
        string WeatherApiUrl { get; set; }
    }

    public class Options : IOptions
    {
        public string GeocoderApiUrl { get; set; }
        public string WeatherApiUrl { get; set; }

        public Options()
        {
            GeocoderApiUrl = ValidateEnvironmentVariable("GEOCODER_API_URL");
            WeatherApiUrl = ValidateEnvironmentVariable("WEATHER_API_URL");
        }

        private string ValidateEnvironmentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName);
            if (string.IsNullOrEmpty(variable))
            {
                throw new Exception($"Environment variable {variableName} is not set");
            }
            return variable;
        }
    }
}
