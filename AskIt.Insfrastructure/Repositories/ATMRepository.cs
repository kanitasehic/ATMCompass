using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Models;
using ATMCompass.Core.Models.Accommodations.Requests;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.Transports.Requests;
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

        public async Task<IList<ATM>> GetATMsAsync()
        {
            return await _dbContext.ATMs.Include(a => a.Node).ToListAsync();
        }

        public async Task<IList<Location>> GetCitiesAsync()
        {
            return await _dbContext.Locations.Where(l => l.Type == "City").ToListAsync();
        }

        public async Task<IList<Location>> GetMunicipalitiesAsync()
        {
            return await _dbContext.Locations.Where(l => l.Type == "Municipality").ToListAsync();
        }

        public IList<NumberOfATMsPerLocationResponse> GetNumberOfATMsPerCity()
        {
            return _dbContext.Set<NumberOfATMsPerLocationResponse>().FromSqlRaw($"EXEC GetNumberOfATMsPerLocation 'City'").AsEnumerable().ToList();
        }

        public IList<NumberOfATMsPerLocationResponse> GetNumberOfATMsPerMunicipality()
        {
            return _dbContext.Set<NumberOfATMsPerLocationResponse>().FromSqlRaw($"EXEC GetNumberOfATMsPerLocation 'Municipality'").AsEnumerable().ToList();
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
            foreach (var atm in atms)
            {
                await AddATMAsync(atm);
            }
        }

        public async Task AddMultipleTransportsAsync(IList<Transport> transports)
        {
            foreach (var transport in transports)
            {
                await AddTransportAsync(transport);
            }
        }

        public async Task AddMultipleAccommodationsAsync(IList<Accommodation> accommodations)
        {
            foreach (var accommodation in accommodations)
            {
                await AddAccommodationAsync(accommodation);
            }
        }

        public async Task<ATM> AddATMAsync(ATM atm)
        {
            await _dbContext.Nodes.AddAsync(atm.Node);
            await _dbContext.Addresses.AddAsync(atm.Address);
            await _dbContext.ATMs.AddAsync(atm);

            await _dbContext.SaveChangesAsync();
            return atm;
        }

        public async Task<Transport> AddTransportAsync(Transport transport)
        {
            await _dbContext.Nodes.AddAsync(transport.Node);
            await _dbContext.Addresses.AddAsync(transport.Address);
            await _dbContext.Transports.AddAsync(transport);

            await _dbContext.SaveChangesAsync();
            return transport;
        }

        public async Task<Accommodation> AddAccommodationAsync(Accommodation accommodation)
        {
            await _dbContext.Nodes.AddAsync(accommodation.Node);
            await _dbContext.Addresses.AddAsync(accommodation.Address);
            await _dbContext.Accommodations.AddAsync(accommodation);

            await _dbContext.SaveChangesAsync();
            return accommodation;
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
            var atms = await _dbContext.ATMs.FromSqlRaw($"EXEC GetCannibals {request.CenterLat}, {request.CenterLon}, '{request.BankName}', {request.RadiusInKilometers}").ToListAsync();
            var result = new List<ATM>();
            foreach (var atm in atms)
            {
                result.Add(await _dbContext.ATMs.Where(a => a.Id == atm.Id).Include(a => a.Node).Include(a => a.Address).FirstOrDefaultAsync());
            }

            return result;
        }

        public string GetBoundary(GetCannibalATMsRequest request)
        {
            return _dbContext.Set<SqlValueReturn<string>>().FromSqlRaw($"EXEC GetBoundary {request.CenterLat}, {request.CenterLon}, {request.RadiusInKilometers}").AsEnumerable().First().Value;
        }

        public async Task<IList<string>> GetAllLocationsAsync()
        {
            return await _dbContext.Addresses.Where(a => a.City != null).Select(a => a.City).Distinct().ToListAsync();
        }

        public async Task<IList<string>> GetAllBanksAsync()
        {
            return await _dbContext.ATMs.Select(a => a.BankName).Distinct().ToListAsync();
        }

        public async Task<IList<Accommodation>> GetAccommodationsAsync(GetAccommodationsRequest request)
        {
            var type = request.Type is null ? "NULL" : $"'{request.Type}'";
            var accommodations = await _dbContext.Accommodations.FromSqlRaw($"EXEC GetAccommodationsWithoutATMsAround {request.RadiusInKilometers}, {type}, '{request.Location}'").ToListAsync();
            var result = new List<Accommodation>();
            foreach (var acc in accommodations)
            {
                result.Add(await _dbContext.Accommodations.Where(a => a.Id == acc.Id).Include(a => a.Node).Include(a => a.Address).FirstOrDefaultAsync());
            }

            return result;
        }

        public async Task<IList<Transport>> GetTransportsAsync(GetTransportsRequest request)
        {
            var type = request.Type is null ? "NULL" : $"'{request.Type}'";
            var transports = await _dbContext.Transports.FromSqlRaw($"EXEC GetTransportsWithoutATMsAround {request.RadiusInKilometers}, {type}, '{request.Location}'").ToListAsync();
            var result = new List<Transport>();
            foreach (var tr in transports)
            {
                result.Add(await _dbContext.Transports.Where(a => a.Id == tr.Id).Include(a => a.Node).Include(a => a.Address).FirstOrDefaultAsync());
            }

            return result;
        }

        public async Task AddLocationsAsync(IList<Location> locations)
        {
            await _dbContext.Locations.AddRangeAsync(locations);

            await _dbContext.SaveChangesAsync();
        }

        private void FilterATMs(ref IQueryable<ATM> query, GetATMsRequest request)
        {
            query = query
                .Include(a => a.Node)
                .Include(a => a.Address);

            if (request.BankName is not null)
            {
                query = query.Where(a => a.BankName == request.BankName);
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
        }
    }
}
