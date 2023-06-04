using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.OverpassAPI;
using Newtonsoft.Json;

namespace ATMCompass.Insfrastructure.HttpCLients
{
    public class OverpassAPIClient : IOverpassAPIClient
    {
        public async Task<IList<GetObjectFromOSMItem>> GetATMsInBosniaAndHerzegovinaAsync()
        {
            var atms = await GetATMsAsync();
            var banks = await GetBanksAsync();
            var banksWithAtms = banks.Where(b => b.Tags.WithinBank == "yes").ToList();

            return atms.Concat(banksWithAtms).ToList();
        }

        public async Task<IList<GetObjectFromOSMItem>> GetTransportsInBosniaAndHerzegovinaAsync()
        {
            var aerodromes = await GetAerodromesAsync();
            var busStations = await GetBusStationsAsync();
            var carRentals = await GetCarRentalsAsync();
            var trainStations = await GetTrainStationsAsync();

            return aerodromes.Concat(busStations).Concat(carRentals).Concat(trainStations).ToList();
        }

        public async Task<IList<GetObjectFromOSMItem>> GetAccommodationsInBosniaAndHerzegovinaAsync()
        {
            var hotels = await GetHotelsAsync();
            var hostels = await GetHostelsAsync();
            var bedAndBreakfasts = await GetBedAndBreakfastAsync();

            return hotels.Concat(hostels).Concat(bedAndBreakfasts).ToList();
        }

        private async Task<IList<GetObjectFromOSMItem>> GetATMsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""amenity""=""atm""](area);
              way[""amenity""=""atm""](area);
              relation[""amenity""=""atm""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "ATM");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBanksAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""amenity""=""bank""](area);
              way[""amenity""=""bank""](area);
              relation[""amenity""=""bank""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "Bank");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetAerodromesAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              way[""aeroway""=""aerodrome""](area);
            );
            out center;
            ";

            return await GetObjectsAsync(query, "Aerodrome");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBusStationsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              way[""amenity""=""bus_station""](area);
            );
            out center;
            ";

            return await GetObjectsAsync(query, "Bus Station");

        }

        private async Task<IList<GetObjectFromOSMItem>> GetCarRentalsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""amenity""=""car_rental""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "Car Rental");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetTrainStationsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              way[""building""=""train_station""](area);
            );
            out center;
            ";

            return await GetObjectsAsync(query, "Train Station");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetHotelsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""tourism""=""hotel""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "Hotel");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetHostelsAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""tourism""=""hostel""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "Hostel");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBedAndBreakfastAsync()
        {
            string query = $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""tourism""=""guest_house""](area);
            );
            out;
            ";

            return await GetObjectsAsync(query, "Bed&Breakfast");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetObjectsAsync(string query, string type)
        {
            var httpClient = new HttpClient();
            var apiUrl = "https://overpass-api.de/api/interpreter";

            var requestData = new StringContent($"data={query}");
            var response = await httpClient.PostAsync(apiUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var objects = JsonConvert.DeserializeObject<GetObjectsFromOSMResponse>(content);
                foreach (var obj in objects.Elements)
                {
                    obj.Tags.Type = type;
                }
                return objects.Elements;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
