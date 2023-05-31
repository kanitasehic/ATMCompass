using ATMCompass.Core.Entities;
using ATMCompass.Core.Exceptions;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.ATMs.OverpassAPI;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.GeoCalculator;
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
            var rawAtmData = await _overpassAPIClient.GetATMsInBosniaAndHerzegovinaAsync();
            var existingAtmExternalIds = await _ATMRepository.GetAllExternalIdsAsync();

            // sync OSM and DB data by adding only new ATMs to the DB
            if (existingAtmExternalIds.Any())
            {
                rawAtmData = rawAtmData.Where(a => !existingAtmExternalIds.Contains(a.Id)).ToList();
            }

            var updatedRawAtmData = await GetATMsWithUpdatedLocationAsync(rawAtmData);

            var atms = GetMappedData(updatedRawAtmData);

            await _ATMRepository.AddMultipleATMsAsync(atms);
        }

        public async Task<IList<GetATMResponse>> GetATMsAsync(GetATMsRequest request)
        {
            var atms = _mapper.Map<List<GetATMResponse>>(await _ATMRepository.GetATMsAsync(request));


            if (atms.Any() && request.CurrentLon is not null && request.CurrentLat is not null)
            {
                CalculateATMsDistances((double)request.CurrentLat, (double)request.CurrentLon, ref atms);
                atms = atms.OrderBy(a => a.Distance).Take(10).ToList();
            }

            return atms;
        }

        public async Task<GetCannibalATMsResponse> GetCannibalATMsAsync(GetCannibalATMsRequest request)
        {
            var atms = _mapper.Map<List<GetATMResponse>>(await _ATMRepository.GetCannibalATMsAsync(request));
            var boundaryString = _ATMRepository.GetBoundary(request);

            return new GetCannibalATMsResponse()
            {
                Atms = atms,
                BoundaryCoordinates = GetBoundaryCoordinates(boundaryString)
            };
        }

        public async Task AddATMAsync(AddATMRequest addAtmRequest)
        {
            var atm = _mapper.Map<ATM>(addAtmRequest);

            await _ATMRepository.AddATMAsync(atm);
        }

        public async Task UpdateATMAsync(int id, UpdateATMRequest atmUpdateRequest)
        {
            var atm = await _ATMRepository.GetATMById(id);

            if (atm is null)
                throw new NotFoundException($"ATM with id {id} does not exist.");

            atm = _mapper.Map(atmUpdateRequest, atm);

            await _ATMRepository.UpdateATMAsync(atm);
        }

        public async Task DeleteATMAsync(int id)
        {
            var atm = await _ATMRepository.GetATMById(id);

            if (atm is null)
                throw new NotFoundException($"ATM with id {id} does not exist.");

            await _ATMRepository.DeleteATMAsync(atm);
        }

        private async Task<IList<GetATMFromOSMItem>> GetATMsWithUpdatedLocationAsync(IList<GetATMFromOSMItem> atms)
        {
            foreach (var atm in atms)
            {
                if (atm.Tags.AddressCity is null)
                {
                    var location = await _geoCodeClient.GetLocationByCoordinatesAsync(atm.Lat, atm.Lon);
                    atm.Tags.AddressCity = string.IsNullOrEmpty(location.City) ? location.Locality : location.City;
                }
            }

            return atms;
        }

        private void CalculateATMsDistances(double currentLat, double currentLon, ref List<GetATMResponse> atms)
        {
            foreach (var atm in atms)
            {
                atm.Distance = _ATMRepository.GetDistance(currentLat.ToString(), currentLon.ToString(), atm.Lat.ToString(), atm.Lon.ToString());
            }
        }

        private IList<ATM> GetMappedData(IList<GetATMFromOSMItem> rawAtms)
        {
            var atms = new List<ATM>();

            foreach (var rawAtm in rawAtms)
            {
                var atm = new ATM()
                {
                    ExternalId = rawAtm.Id,
                    Wheelchair = rawAtm.Tags.Wheelchair == "yes" ? true : rawAtm.Tags.Wheelchair == "no" ? false : null,
                    DriveThrough = rawAtm.Tags.DriveThrough == "yes" ? true : rawAtm.Tags.DriveThrough == "no" ? false : null,
                    CashIn = rawAtm.Tags.CashIn == "yes" ? true : rawAtm.Tags.CashIn == "no" ? false : null,
                    Indoor = rawAtm.Tags.Indoor == "yes" ? true : rawAtm.Tags.Indoor == "no" ? false : null,
                    Covered = rawAtm.Tags.Covered == "yes" ? true : rawAtm.Tags.Covered == "no" ? false : null,
                    WithinBank = rawAtm.Tags.WithinBank == "yes" ? true : rawAtm.Tags.WithinBank == "no" ? false : null,
                    OpeningHours = rawAtm.Tags.OpeningHours,
                    Lat = double.Parse(rawAtm.Lat),
                    Lon = double.Parse(rawAtm.Lon),
                    BankName = !string.IsNullOrEmpty(rawAtm.Tags.BankName) ? rawAtm.Tags.BankName :
                                    !string.IsNullOrEmpty(rawAtm.Tags.OperatorName) ? rawAtm.Tags.OperatorName :
                                    !string.IsNullOrEmpty(rawAtm.Tags.BrandName) ? rawAtm.Tags.BrandName :
                                    rawAtm.Tags.Fee,
                    City = rawAtm.Tags.AddressCity,
                    Street = rawAtm.Tags.AddressStreet,
                    HouseNumber = rawAtm.Tags.AddressHouseNumber,
                };

                atms.Add(atm);
            }

            return atms;
        }

        private IList<Coordinate> GetBoundaryCoordinates(string boundaryString)
        {
            boundaryString = boundaryString.Replace("POLYGON ((", "");
            boundaryString = boundaryString.Replace("))", "");

            var splittedBoundaryCoordinates = boundaryString.Split(", ");

            var boundaryCoordinates = new List<Coordinate>();
            foreach(var splittedCoordinate in splittedBoundaryCoordinates)
            {
                var splittedLatAndLon = splittedCoordinate.Split(' ');
                var lat = splittedLatAndLon[0];
                var lon = splittedLatAndLon[1];

                var coordinate = new Coordinate(double.Parse(lat), double.Parse(lon));
                boundaryCoordinates.Add(coordinate);
            }

            return boundaryCoordinates;
        }
    }
}
