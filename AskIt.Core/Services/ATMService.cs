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
            var rawATMsData = await _overpassAPIClient.GetATMsInBosniaAndHerzegovinaAsync();
            var existingATMExternalIds = await _ATMRepository.GetAllExternalIdsAsync();

            // sync OSM and DB data by adding only new ATMs to the DB
            if(existingATMExternalIds.Any())
            {
                rawATMsData = rawATMsData.Where(a => !existingATMExternalIds.Contains(a.Id)).ToList();
            }

            var ATMs = _mapper.Map<IList<ATM>>(rawATMsData);
            var ATMsWithUpdatedLocation = await GetATMsWithUpdatedLocationAsync(ATMs);

            await _ATMRepository.AddMultipleATMsAsync(ATMsWithUpdatedLocation);
        }

        private async Task<IList<ATM>> GetATMsWithUpdatedLocationAsync(IList<ATM> ATMs)
        {
            foreach (var ATM in ATMs)
            {
                if(ATM.Location is null)
                {
                    var location = await _geoCodeClient.GetLocationByCoordinatesAsync(ATM.Lat, ATM.Lon);
                    ATM.Location = string.IsNullOrEmpty(location.City) ? location.Locality : location.City;
                }
            }

            return ATMs;
        }
    }
}
