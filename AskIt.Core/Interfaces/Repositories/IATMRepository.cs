using ATMCompass.Core.Entities;

namespace ATMCompass.Core.Interfaces.Repositories
{
    public interface IATMRepository
    {
        Task<IList<string>> GetAllExternalIdsAsync();

        Task AddMultipleATMsAsync(IList<ATM> atms);
    }
}
