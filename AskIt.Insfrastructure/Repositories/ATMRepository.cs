using ATMCompass.Insfrastructure.Data;
using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ATMCompass.Insfrastructure.Repositories
{
    public class ATMRepository : IATMRepository
    {
        private readonly ATMCompassDbContext _dbContext;

        public ATMRepository(ATMCompassDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<string>> GetAllExternalIdsAsync()
        {
            return await _dbContext.ATMs.Select(a => a.ExternalId).ToListAsync();
        }

        public async Task AddMultipleATMsAsync(IList<ATM> ATMs)
        {
            await _dbContext.ATMs.AddRangeAsync(ATMs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
