using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.OverpassAPI;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IOverpassAPIClient
    {
        Task<IList<GetObjectFromOSMItem>> GetATMsInBosniaAndHerzegovinaAsync();

        Task<IList<GetObjectFromOSMItem>> GetTransportsInBosniaAndHerzegovinaAsync();

        Task<IList<GetObjectFromOSMItem>> GetAccommodationsInBosniaAndHerzegovinaAsync();

        IList<Location> GetLocationsFromGeoJson();
    }
}
