using Newtonsoft.Json;

namespace ATMCompass.Core.Models.OverpassAPI
{
    public class CenterNode
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }
}
