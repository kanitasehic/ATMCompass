namespace ATMCompass.Core.Models.ATMs.Responses
{
    public class GetATMResponse
    {
        public string? ExternalId { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public bool? IsAccessibleUsingWheelchair { get; set; }

        public string? Name { get; set; }

        public bool? IsDriveThroughEnabled { get; set; }

        public string? Location { get; set; }

        public string? Address { get; set; }

        public string? OpeningHours { get; set; }

        public string? Website { get; set; }

        public double? Distance { get; set; }
    }
}
