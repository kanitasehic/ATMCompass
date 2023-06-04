using ATMCompass.Core.Entities;
using ATMCompass.Core.Exceptions;
using ATMCompass.Core.Interfaces.Repositories;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.GeoCalculator;
using ATMCompass.Core.Models.OverpassAPI;
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

            var atms = GetMappedATMData(updatedRawAtmData);

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

        public async Task<IList<string>> GetAllLocationsAsync()
        {
            return await _ATMRepository.GetAllLocationsAsync();
        }

        public async Task<IList<string>> GetAllBanksAsync()
        {
            return await _ATMRepository.GetAllBanksAsync();
        }

        public async Task SynchronizeTransportDataAsync()
        {
            var rawTransportData = await _overpassAPIClient.GetTransportsInBosniaAndHerzegovinaAsync();

            var transports = GetMappedTransportData(rawTransportData);

            await _ATMRepository.AddMultipleTransportsAsync(transports);
        }

        public async  Task SynchronizeAccommodationDataAsync()
        {
            var rawAccommodationData = await _overpassAPIClient.GetAccommodationsInBosniaAndHerzegovinaAsync();

            var accommodations = GetMappedAccommodationData(rawAccommodationData);

            await _ATMRepository.AddMultipleAccommodationsAsync(accommodations);
        }

        private async Task<IList<GetObjectFromOSMItem>> GetATMsWithUpdatedLocationAsync(IList<GetObjectFromOSMItem> atms)
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

        private IList<ATM> GetMappedATMData(IList<GetObjectFromOSMItem> rawAtms)
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
                    Node = new Node()
                    {
                        Lat = double.Parse(rawAtm.Lat),
                        Lon = double.Parse(rawAtm.Lon)
                    },
                    BankName = !string.IsNullOrEmpty(rawAtm.Tags.Name) ? rawAtm.Tags.Name :
                                    !string.IsNullOrEmpty(rawAtm.Tags.OperatorName) ? rawAtm.Tags.OperatorName :
                                    !string.IsNullOrEmpty(rawAtm.Tags.BrandName) ? rawAtm.Tags.BrandName :
                                    rawAtm.Tags.Fee,
                    Address = new Address()
                    {
                        City = rawAtm.Tags.AddressCity,
                        Street = rawAtm.Tags.AddressStreet,
                        HouseNumber = rawAtm.Tags.AddressHouseNumber
                    }
                };

                atms.Add(atm);
            }

            return atms;
        }

        private IList<Transport> GetMappedTransportData(IList<GetObjectFromOSMItem> rawTransportObjects)
        {
            var transports = new List<Transport>();

            foreach (var rawTransport in rawTransportObjects)
            {
                var transport = new Transport()
                {
                    Node = new Node()
                    {
                        Lat = rawTransport.Lat is not null ? double.Parse(rawTransport.Lat) : rawTransport.Center.Lat,
                        Lon = rawTransport.Lon is not null ? double.Parse(rawTransport.Lon) : rawTransport.Center.Lon
                    },
                    Name = !string.IsNullOrEmpty(rawTransport.Tags.Name) ? rawTransport.Tags.Name :
                                    !string.IsNullOrEmpty(rawTransport.Tags.OperatorName) ? rawTransport.Tags.OperatorName :
                                    !string.IsNullOrEmpty(rawTransport.Tags.BrandName) ? rawTransport.Tags.BrandName :
                                    rawTransport.Tags.Fee,
                    Address = new Address()
                    {
                        City = rawTransport.Tags.AddressCity,
                        Street = rawTransport.Tags.AddressStreet,
                        HouseNumber = rawTransport.Tags.AddressHouseNumber
                    },
                    Type = rawTransport.Tags.Type
                };

                transports.Add(transport);
            }

            return transports;
        }

        private IList<Accommodation> GetMappedAccommodationData(IList<GetObjectFromOSMItem> rawAccommodationObjects)
        {
            var accommodations = new List<Accommodation>();

            foreach (var rawAccommodation in rawAccommodationObjects)
            {
                var transport = new Accommodation()
                {
                    Node = new Node()
                    {
                        Lat = rawAccommodation.Lat is not null ? double.Parse(rawAccommodation.Lat) : rawAccommodation.Center.Lat,
                        Lon = rawAccommodation.Lon is not null ? double.Parse(rawAccommodation.Lon) : rawAccommodation.Center.Lon
                    },
                    Name = !string.IsNullOrEmpty(rawAccommodation.Tags.Name) ? rawAccommodation.Tags.Name :
                                    !string.IsNullOrEmpty(rawAccommodation.Tags.OperatorName) ? rawAccommodation.Tags.OperatorName :
                                    !string.IsNullOrEmpty(rawAccommodation.Tags.BrandName) ? rawAccommodation.Tags.BrandName :
                                    rawAccommodation.Tags.Fee,
                    Address = new Address()
                    {
                        City = rawAccommodation.Tags.AddressCity,
                        Street = rawAccommodation.Tags.AddressStreet,
                        HouseNumber = rawAccommodation.Tags.AddressHouseNumber
                    },
                    Type = rawAccommodation.Tags.Type
                };

                accommodations.Add(transport);
            }

            return accommodations;
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
                var lat = splittedLatAndLon[1];
                var lon = splittedLatAndLon[0];

                var coordinate = new Coordinate(double.Parse(lat), double.Parse(lon));
                boundaryCoordinates.Add(coordinate);
            }

            return boundaryCoordinates;
        }
    }
}
