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

        public async Task<ATM> GetATMById(int id)
        {
            return await _dbContext.ATMs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddMultipleATMsAsync(IList<ATM> atms)
        {
            await _dbContext.ATMs.AddRangeAsync(atms);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ATM> AddATMAsync(ATM atm)
        {
            await _dbContext.ATMs.AddAsync(atm);
            await _dbContext.SaveChangesAsync();
            return atm;
        }

        public async Task UpdateATMAsync(ATM atm)
        {
            _dbContext.Entry(atm).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteATMAsync(ATM atm)
        {
            _dbContext.ATMs.Remove(atm);
            await _dbContext.SaveChangesAsync();
        }
    }
}
