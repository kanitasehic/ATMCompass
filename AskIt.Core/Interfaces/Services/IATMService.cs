using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IATMService
    {
        Task<IList<GetATMResponse>> GetATMsAsync(GetATMsRequest request);

        Task SynchronizeATMDataAsync();

        Task AddATMAsync(AddATMRequest atm);

        Task UpdateATMAsync(int id, UpdateATMRequest atmUpdateRequest);

        Task DeleteATMAsync(int id);
    }
}
