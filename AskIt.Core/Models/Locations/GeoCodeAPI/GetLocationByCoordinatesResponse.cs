using Newtonsoft.Json;

namespace ATMCompass.Core.Models.Locations.GeoCodeAPI
{
    public class GetLocationByCoordinatesResponse
    {
        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("locality")]
        public string? Locality { get; set; }
    }
}
