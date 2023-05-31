using ATMCompass.Core.Models.GeoCalculator;

namespace ATMCompass.Core.Models.ATMs.Responses
{
    public class GetCannibalATMsResponse
    {
        public IList<GetATMResponse> Atms { get; set; }

        public IList<Coordinate> BoundaryCoordinates { get; set; }
    }
}
