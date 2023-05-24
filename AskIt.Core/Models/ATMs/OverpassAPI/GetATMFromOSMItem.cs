using Newtonsoft.Json;

namespace ATMCompass.Core.Models.ATMs.OverpassAPI
{
    public class GetATMFromOSMItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }

        [JsonProperty("tags")]
        public ATMTags Tags { get; set; }
    }
}
