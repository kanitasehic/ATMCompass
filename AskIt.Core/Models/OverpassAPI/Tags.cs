using Newtonsoft.Json;

namespace ATMCompass.Core.Models.OverpassAPI
{
    public sealed class Tags
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("brand")]
        public string? BrandName { get; set; }

        [JsonProperty("operator")]
        public string? OperatorName { get; set; }

        [JsonProperty("addr:city")]
        public string? AddressCity { get; set; }

        [JsonProperty("addr:street")]
        public string? AddressStreet { get; set; }

        [JsonProperty("addr:housenumber")]
        public string? AddressHouseNumber { get; set; }

        [JsonProperty("fee")]
        public string? Fee { get; set; }

        [JsonProperty("drive_through")]
        public string? DriveThrough { get; set; }

        [JsonProperty("opening_hours")]
        public string? OpeningHours { get; set; }

        [JsonProperty("wheelchair")]
        public string? Wheelchair { get; set; }

        [JsonProperty("cash_in")]
        public string? CashIn { get; set; }

        [JsonProperty("indoor")]
        public string? Indoor { get; set; }

        [JsonProperty("covered")]
        public string? Covered { get; set; }

        [JsonProperty("atm")]
        public string? WithinBank { get; set; }

        public string? Type { get; set; }
    }
}
