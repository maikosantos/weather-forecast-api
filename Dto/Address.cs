namespace weather_forecast_api.Dto
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string ZipCode { get; set; }

        public string FullAddress
        {
            get
            {
                return $"{Street}, {City}, {StateAbbreviation} {ZipCode}";
            }
        }
    }
}
