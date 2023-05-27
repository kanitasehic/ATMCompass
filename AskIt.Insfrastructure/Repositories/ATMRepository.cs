using ATMCompass.Insfrastructure.Data;
using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using ATMCompass.Core.Models.ATMs.Requests;

namespace ATMCompass.Insfrastructure.Repositories
{
    public class ATMRepository : IATMRepository
    {
        private readonly ATMCompassDbContext _dbContext;

        public ATMRepository(ATMCompassDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<ATM>> GetATMsAsync(GetATMsRequest request)
        {
            var query = _dbContext.ATMs.AsQueryable();

            /*if(request.BankName is not null)
            {
                query = query.Where(a => a.Name == request.BankName);
            }
            if(request.Location is not null)
            {
                query = query.Where(a => a.Location == request.Location);
            }
            if(request.IsAccessibleUsingWheelchair is not null)
            {
                query = query.Where(a => a.IsAccessibleUsingWheelchair == request.IsAccessibleUsingWheelchair);
            }
            if(request.IsDriveThroughEnabled is not null)
            {
                query = query.Where(a => a.IsDriveThroughEnabled == request.IsDriveThroughEnabled);
            }*/

            return await query.ToListAsync();
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
            foreach (var atm in atms)
            {
                await AddATMAsync(atm);
            }
        } 

        public async Task<ATM> AddATMAsync(ATM atm)
        {
            await _dbContext.Nodes.AddAsync(atm.Node);
            await _dbContext.Banks.AddAsync(atm.Bank);
            await _dbContext.Addresses.AddAsync(atm.Address);
            if(atm.Brand is not null)
                await _dbContext.Brands.AddAsync(atm.Brand);
            if (atm.Operator is not null)
                await _dbContext.Operators.AddAsync(atm.Operator);
            if (atm.Currency is not null)
                await _dbContext.Currencies.AddAsync(atm.Currency);
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
