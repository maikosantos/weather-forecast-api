namespace weather_forecast_api.Dto
{
    using System.Collections.Generic;

    public class WeatherResponse
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public List<AddressMatch> AddressMatches { get; set; }
    }

    public class AddressMatch
    {
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public double x { get; set; }
        public double y { get; set; }
    }


}
