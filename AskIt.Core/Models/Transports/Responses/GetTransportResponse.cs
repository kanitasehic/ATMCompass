namespace ATMCompass.Core.Models.Transports.Responses
{
    public class GetTransportResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Type { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string City { get; set; }

        public string? Street { get; set; }

        public string? HouseNumber { get; set; }
    }
}
