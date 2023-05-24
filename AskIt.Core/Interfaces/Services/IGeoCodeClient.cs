using ATMCompass.Core.Models.Locations.GeoCodeAPI;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IGeoCodeClient
    {
        Task<GetLocationByCoordinatesResponse> GetLocationByCoordinatesAsync(string latitude, string longitude);
    }
}
