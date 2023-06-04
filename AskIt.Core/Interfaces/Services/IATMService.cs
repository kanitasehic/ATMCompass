using ATMCompass.Core.Models.Accommodations.Requests;
using ATMCompass.Core.Models.Accommodations.Responses;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.Transports.Requests;
using ATMCompass.Core.Models.Transports.Responses;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IATMService
    {
        Task<IList<GetATMResponse>> GetATMsAsync(GetATMsRequest request);

        Task SynchronizeATMDataAsync();

        Task SynchronizeTransportDataAsync();

        Task SynchronizeAccommodationDataAsync();

        Task UpdateATMAsync(int id, UpdateATMRequest atmUpdateRequest);

        Task DeleteATMAsync(int id);

        Task<GetCannibalATMsResponse> GetCannibalATMsAsync(GetCannibalATMsRequest request);

        Task<IList<string>> GetAllLocationsAsync();

        Task<IList<string>> GetAllBanksAsync();

        Task<IList<GetAccommodationResponse>> GetAccommodationsWithoutATMsAroundAsync(GetAccommodationsRequest request);

        Task<IList<GetTransportResponse>> GetTransportsWithoutATMsAroundAsync(GetTransportsRequest request);
    }
}
