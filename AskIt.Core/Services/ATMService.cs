using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Interfaces.Services;
using AutoMapper;

namespace ATMCompass.Core.Services
{
    public class ATMService : IATMService
    {
        private readonly IOverpassAPIClient _overpassAPIClient;
        private readonly IGeoCodeClient _geoCodeClient;
        private readonly IATMRepository _ATMRepository;
        private readonly IMapper _mapper;
        public ATMService(IOverpassAPIClient overpassAPIClient, IGeoCodeClient geoCodeClient, IATMRepository ATMRepository, IMapper mapper)
        {
            _overpassAPIClient = overpassAPIClient;
            _geoCodeClient = geoCodeClient;
            _ATMRepository = ATMRepository;
            _mapper = mapper;
        }

        public async Task SynchronizeATMDataAsync()
        {
            var rawAtmsData = await _overpassAPIClient.GetATMsInBosniaAndHerzegovinaAsync();
            var existingAtmExternalIds = await _ATMRepository.GetAllExternalIdsAsync();

            // sync OSM and DB data by adding only new ATMs to the DB
            if(existingAtmExternalIds.Any())
            {
                rawAtmsData = rawAtmsData.Where(a => !existingAtmExternalIds.Contains(a.Id)).ToList();
            }

            var atms = _mapper.Map<IList<ATM>>(rawAtmsData);
            var atmsWithUpdatedLocation = await GetATMsWithUpdatedLocationAsync(atms);

            await _ATMRepository.AddMultipleATMsAsync(atmsWithUpdatedLocation);
        }

        private async Task<IList<ATM>> GetATMsWithUpdatedLocationAsync(IList<ATM> atms)
        {
            foreach (var atm in atms)
            {
                if(atm.Location is null)
                {
                    var location = await _geoCodeClient.GetLocationByCoordinatesAsync(atm.Lat, atm.Lon);
                    atm.Location = string.IsNullOrEmpty(location.City) ? location.Locality : location.City;
                }
            }

            return atms;
        }
    }
}
