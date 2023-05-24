using Newtonsoft.Json;

namespace ATMCompass.Core.Models.ATMs.OverpassAPI
{
    public sealed class ATMTags
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("operator")]
        public string? Operator { get; set; }

        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("fee")]
        public string? Fee { get; set; }

        [JsonProperty("drive_through")]
        public string? DriveThrough { get; set; }

        [JsonProperty("addr:city")]
        public string? City { get; set; }

        [JsonProperty("addr:street")]
        public string? Street { get; set; }

        [JsonProperty("opening_hours")]
        public string? OpeningHours { get; set; }

        [JsonProperty("wheelchair")]
        public string? Wheelchair { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }
    }
}
