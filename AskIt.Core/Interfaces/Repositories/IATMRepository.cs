using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.ATMs.Requests;

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
    }
}
