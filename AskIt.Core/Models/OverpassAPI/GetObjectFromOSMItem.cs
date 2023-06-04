using Newtonsoft.Json;

namespace ATMCompass.Core.Models.OverpassAPI
{
    public class GetObjectFromOSMItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lat")]
        public string? Lat { get; set; }

        [JsonProperty("lon")]
        public string? Lon { get; set; }

        [JsonProperty("center")]
        public CenterNode? Center { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }
    }
}
