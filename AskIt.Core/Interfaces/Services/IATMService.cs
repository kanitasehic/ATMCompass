using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;

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
    }
}
