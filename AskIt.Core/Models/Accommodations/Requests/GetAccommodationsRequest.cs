namespace ATMCompass.Core.Models.Accommodations.Requests
{
    public class GetAccommodationsRequest
    {
        public double RadiusInKilometers { get; set; }

        public string? Type { get; set; }

        public string Location { get; set; }
    }
}
