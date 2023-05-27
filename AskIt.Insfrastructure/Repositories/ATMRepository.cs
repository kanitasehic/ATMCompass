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
            var query = _dbContext.ATMs.Where(a => a.ApprovedByAdmin == request.ApprovedByAdmin);

            FilterATMs(ref query, request);

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
            {
                await _dbContext.Brands.AddAsync(atm.Brand);
            }
            if (atm.Operator is not null)
            {
                await _dbContext.Operators.AddAsync(atm.Operator);
            }
            if (atm.Currency is not null)
            {
                await _dbContext.Currencies.AddAsync(atm.Currency);
            }
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
            _dbContext.Nodes.Remove(atm.Node);
            _dbContext.Banks.Remove(atm.Bank);
            _dbContext.Addresses.Remove(atm.Address);
            if (atm.Brand is not null)
            {
                _dbContext.Brands.Remove(atm.Brand);
            }
            if (atm.Operator is not null)
            {
                _dbContext.Operators.Remove(atm.Operator);
            }
            if (atm.Currency is not null)
            {
                _dbContext.Currencies.Remove(atm.Currency);
            }
            await _dbContext.SaveChangesAsync();
        }

        private void FilterATMs(ref IQueryable<ATM> query, GetATMsRequest request)
        {
            if (request.BankName is not null)
            {
                query = query.Where(a => a.Bank.Name == request.BankName);
            }
            if (request.Location is not null)
            {
                query = query.Where(a => a.Address.City == request.Location);
            }
            if (request.Wheelchair is not null)
            {
                query = query.Where(a => a.Wheelchair != null && a.Wheelchair == request.Wheelchair);
            }
            if (request.DriveThrough is not null)
            {
                query = query.Where(a => a.DriveThrough != null && a.DriveThrough == request.DriveThrough);
            }
            if (request.CashIn is not null)
            {
                query = query.Where(a => a.CashIn != null && a.CashIn == request.CashIn);
            }
            if (request.Covered is not null)
            {
                query = query.Where(a => a.Covered != null && a.Covered == request.Covered);
            }
            if (request.Indoor is not null)
            {
                query = query.Where(a => a.Indoor != null && a.Indoor == request.Indoor);
            }
            if (request.WithinBank is not null)
            {
                query = query.Where(a => a.WithinBank != null && a.WithinBank == request.WithinBank);
            }
            if (request.CurrencyEUR is not null)
            {
                query = query.Where(a => a.Currency != null && a.Currency.EUR != null && a.Currency.EUR == request.CurrencyEUR);
            }
            if (request.CurrencyUSD is not null)
            {
                query = query.Where(a => a.Currency != null && a.Currency.USD != null && a.Currency.USD == request.CurrencyUSD);
            }
        }
    }
}
