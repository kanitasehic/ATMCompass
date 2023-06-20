using ATMCompass.Core.Models.GeoCalculator;

namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class GetIsohronesRequest
    {
        public string Lat { get; set; }

        public string Lon { get; set; }

        public string TransportType { get; set; }
    }
}
