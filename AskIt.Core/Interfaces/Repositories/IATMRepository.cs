using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.Accommodations.Requests;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.Transports.Requests;

namespace ATMCompass.Core.Interfaces.Repositories
{
    public interface IATMRepository
    {
        Task<IList<ATM>> GetATMsAsync(GetATMsRequest request);

        Task<IList<string>> GetAllExternalIdsAsync();

        Task<ATM> GetATMById(int id);

        Task AddMultipleATMsAsync(IList<ATM> atms);

        Task<ATM> AddATMAsync(ATM atm);

        Task UpdateATMAsync(ATM atm);

        Task DeleteATMAsync(ATM atm);

        double GetDistance(string t1Lat, string t1Lon, string t2Lat, string t2Lon);

        Task<IList<ATM>> GetCannibalATMsAsync(GetCannibalATMsRequest request);

        string GetBoundary(GetCannibalATMsRequest request);

        Task<IList<string>> GetAllLocationsAsync();

        Task<IList<string>> GetAllBanksAsync();

        Task<Accommodation> AddAccommodationAsync(Accommodation accommodation);

        Task AddMultipleAccommodationsAsync(IList<Accommodation> accommodations);

        Task<Transport> AddTransportAsync(Transport transport);

        Task AddMultipleTransportsAsync(IList<Transport> transports);

        Task<IList<Accommodation>> GetAccommodationsAsync(GetAccommodationsRequest request);

        Task<IList<Transport>> GetTransportsAsync(GetTransportsRequest request);

        Task AddLocationsAsync(IList<Location> locations);

        Task<IList<Location>> GetMunicipalitiesAsync();

        Task<IList<Location>> GetCitiesAsync();

        Task<IList<ATM>> GetATMsAsync();

        IList<NumberOfATMsPerLocationResponse> GetNumberOfATMsPerCity();

        IList<NumberOfATMsPerLocationResponse> GetNumberOfATMsPerMunicipality();
    }
}
