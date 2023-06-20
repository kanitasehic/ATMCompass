using ATMCompass.Core.Entities;
using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.OverpassAPI;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;

namespace ATMCompass.Insfrastructure.HttpCLients
{
    public class OverpassAPIClient : IOverpassAPIClient
    {
        public IList<Location> GetLocationsFromGeoJson()
        {
            string citiesJson = File.ReadAllText("C:\\Users\\Kanita\\Desktop\\Skripte za zavrsni\\cities.geojson");
            string municipalitiesJson = File.ReadAllText("C:\\Users\\Kanita\\Desktop\\Skripte za zavrsni\\municipalities.geojson");

            var citiesFeatureCollection = JsonConvert.DeserializeObject<FeatureCollection>(citiesJson);
            var municipalitiesFeatureCollection = JsonConvert.DeserializeObject<FeatureCollection>(municipalitiesJson);

            var locations = new List<Location>();

            AddLocations(ref locations, "City", citiesFeatureCollection.Features);
            AddLocations(ref locations, "Municipality", municipalitiesFeatureCollection.Features);

            return locations;
        }

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
            string query = GetOverpassQueryForNode("amenity", "atm");

            return await GetObjectsAsync(query, "ATM");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBanksAsync()
        {
            string query = GetOverpassQueryForNode("amenity", "bank");

            return await GetObjectsAsync(query, "Bank");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetAerodromesAsync()
        {
            string query = GetOverpassQueryForWay("aeroway", "aerodrome");

            return await GetObjectsAsync(query, "Aerodrome");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBusStationsAsync()
        {
            string query = GetOverpassQueryForWay("amenity", "bus_station");

            return await GetObjectsAsync(query, "Bus Station");

        }

        private async Task<IList<GetObjectFromOSMItem>> GetCarRentalsAsync()
        {
            string query = GetOverpassQueryForNode("amenity", "car_rental");

            return await GetObjectsAsync(query, "Car Rental");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetTrainStationsAsync()
        {
            string query = GetOverpassQueryForWay("building", "train_station");

            return await GetObjectsAsync(query, "Train Station");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetHotelsAsync()
        {
            string query = GetOverpassQueryForNode("tourism", "hotel");

            return await GetObjectsAsync(query, "Hotel");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetHostelsAsync()
        {
            string query = GetOverpassQueryForNode("tourism", "hostel");

            return await GetObjectsAsync(query, "Hostel");
        }

        private async Task<IList<GetObjectFromOSMItem>> GetBedAndBreakfastAsync()
        {
            string query = GetOverpassQueryForNode("tourism", "guest_house");

            return await GetObjectsAsync(query, "Bed&Breakfast");
        }

        private string GetOverpassQueryForNode(string key, string value)
        {
            return $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              node[""{key}""=""{value}""](area);
            );
            out;
            ";
        }

        private string GetOverpassQueryForWay(string key, string value)
        {
            return $@"
            [out:json];
            area[""ISO3166-1""=""BA""][admin_level=2];
            (
              way[""{key}""=""{value}""](area);
            );
            out;
            ";
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

        private void AddLocations(ref List<Location> locations, string type, List<Feature> features)
        {
            foreach (Feature feature in features)
            {
                if (feature.Geometry.Type == GeoJSON.Net.GeoJSONObjectType.Polygon)
                {
                    var name = feature.Properties["name"];
                    Polygon geometry = (Polygon)feature.Geometry;
                    var coordinates = geometry.Coordinates.FirstOrDefault().Coordinates;

                    StringBuilder polygonBuilder = new StringBuilder();
                    polygonBuilder.Append("POLYGON ((");

                    for (int i = 0; i < coordinates.Count; i++)
                    {
                        if (i != coordinates.Count - 1)
                        {
                            polygonBuilder.Append($"{coordinates[i].Latitude} {coordinates[i].Longitude}, ");
                        }
                        else
                        {
                            polygonBuilder.Append($"{coordinates[i].Latitude} {coordinates[i].Longitude}))");
                        }
                    }

                    var polygon = new SqlChars(polygonBuilder.ToString());

                    SqlGeometry polygonGeometry = SqlGeometry.STGeomFromText(polygon, 4326);

                    var city = new Location()
                    {
                        Name = name.ToString(),
                        Type = type,
                        Geometry = polygonBuilder.ToString()
                    };

                    locations.Add(city);
                }
            }
        }
    }
}
