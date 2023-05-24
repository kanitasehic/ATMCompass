using ATMCompass.Core.Entities;

namespace ATMCompass.Core.Interfaces.Repositories
{
    public interface IATMRepository
    {
        Task<IList<string>> GetAllExternalIdsAsync();

        Task<ATM> GetATMById(int id);

        Task AddMultipleATMsAsync(IList<ATM> atms);

        Task<ATM> AddATMAsync(ATM atm);

        Task UpdateATMAsync(ATM atm);

        Task DeleteATMAsync(ATM atm);
    }
}
