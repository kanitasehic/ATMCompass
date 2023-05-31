using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Models;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Insfrastructure.Data;
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

        public async Task<IList<ATM>> GetATMsAsync(GetATMsRequest request)
        {
            var query = _dbContext.ATMs.AsQueryable();

            FilterATMs(ref query, request);

            return await query.ToListAsync();
        }

        public double GetDistance(string t1Lat, string t1Lon, string t2Lat, string t2Lon)
        {
            return _dbContext.Set<SqlValueReturn<double>>().FromSqlRaw($"EXEC GetDistance {t1Lat}, {t1Lon}, {t2Lat}, {t2Lon}").AsEnumerable().First().Value;
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
            await _dbContext.AddRangeAsync(atms);
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

        public async Task<IList<ATM>> GetCannibalATMsAsync(GetCannibalATMsRequest request)
        {
            return await _dbContext.ATMs.FromSqlRaw($"EXEC GetCannibals {request.CenterLat}, {request.CenterLon}, '{request.BankName}', {request.RadiusInKilometers}").ToListAsync();
        }

        private void FilterATMs(ref IQueryable<ATM> query, GetATMsRequest request)
        {
            ;
            if (request.BankName is not null)
            {
                query = query.Where(a => a.BankName == request.BankName);
            }
            if (request.Location is not null)
            {
                query = query.Where(a => a.City == request.Location);
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
        }
    }
}
