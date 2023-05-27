using ATMCompass.Core.Entities;
using ATMCompass.Core.Exceptions;
using ATMCompass.Core.Helpers;
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
            var atms = _mapper.Map<IList<GetATMResponse>>(await _ATMRepository.GetATMsAsync(request));

            if (atms.Any())
            {
                CalculateATMsDistances(request.CurrentLat, request.CurrentLon, ref atms);
                atms = atms.OrderBy(a => a.Distance).Take(10).ToList();
            }

            return atms;
        }

        public async Task<AddATMResponse> AddATMAsync(AddATMRequest atm)
        {
            var newAtm = _mapper.Map<ATM>(atm);

            return _mapper.Map<AddATMResponse>(await _ATMRepository.AddATMAsync(newAtm));
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

        private void CalculateATMsDistances(double currentLat, double currentLon, ref IList<GetATMResponse> atms)
        {
            foreach (var atm in atms)
            {
                var originalCoordinate = new Coordinate(currentLat, currentLon);
                var destinationCoordinate = new Coordinate(double.Parse(atm.Lat), double.Parse(atm.Lon));

                double distance = GeoCalculator.GetDistance(originalCoordinate, destinationCoordinate);

                atm.Distance = distance;
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
                    Fee = rawAtm.Tags.Fee,
                    Phone = rawAtm.Tags.Phone,
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
                    Bank = new Bank()
                    {
                        Name = !string.IsNullOrEmpty(rawAtm.Tags.BankName) ? rawAtm.Tags.BankName :
                                    !string.IsNullOrEmpty(rawAtm.Tags.OperatorName) ? rawAtm.Tags.OperatorName :
                                    !string.IsNullOrEmpty(rawAtm.Tags.BrandName) ? rawAtm.Tags.BrandName :
                                    rawAtm.Tags.Fee,
                        Website = rawAtm.Tags.Website,
                        Email = rawAtm.Tags.Email
                    },
                    Address = new Address()
                    {
                        City = rawAtm.Tags.AddressCity,
                        Street = rawAtm.Tags.AddressStreet,
                        HouseNumber = rawAtm.Tags.AddressHouseNumber,
                        Postcode = rawAtm.Tags.AddressPostcode
                    },
                };

                if ((rawAtm.Tags.OperatorName is not null) || (rawAtm.Tags.OperatorWikidata is not null) || (rawAtm.Tags.OperatorWikipedia is not null))
                {
                    atm.Operator = new Operator()
                    {
                        Name = rawAtm.Tags.OperatorName,
                        Wikidata = rawAtm.Tags.OperatorWikidata,
                        Wikipedia = rawAtm.Tags.OperatorWikipedia
                    };
                }

                if ((rawAtm.Tags.BrandName is not null) || (rawAtm.Tags.BrandWikidata is not null) || (rawAtm.Tags.BrandWikipedia is not null))
                {
                    atm.Brand = new Brand()
                    {
                        Name = rawAtm.Tags.BrandName,
                        Wikidata = rawAtm.Tags.BrandWikidata,
                        Wikipedia = rawAtm.Tags.BrandWikipedia
                    };
                }

                if ((rawAtm.Tags.CurrencyBAM is not null) || (rawAtm.Tags.CurrencyEUR is not null) || (rawAtm.Tags.CurrencyUSD is not null) || (rawAtm.Tags.CurrencyOthers is not null))
                {
                    atm.Currency = new Currency()
                    {
                        BAM = rawAtm.Tags.CurrencyBAM == "yes" ? true : rawAtm.Tags.CurrencyBAM == "no" ? false : null,
                        EUR = rawAtm.Tags.CurrencyEUR == "yes" ? true : rawAtm.Tags.CurrencyEUR == "no" ? false : null,
                        USD = rawAtm.Tags.CurrencyUSD == "yes" ? true : rawAtm.Tags.CurrencyUSD == "no" ? false : null,
                        Others = rawAtm.Tags.CurrencyOthers == "yes" ? true : rawAtm.Tags.CurrencyOthers == "no" ? false : null,
                    };
                }

                atms.Add(atm);
            }

            return atms;
        }
    }
}
