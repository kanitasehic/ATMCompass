namespace ATMCompass.Core.Models.Transports.Requests
{
    public class GetTransportsRequest
    {
        public double RadiusInKilometers { get; set; }

        public string? Type { get; set; }

        public string Location { get; set; }
    }
}
