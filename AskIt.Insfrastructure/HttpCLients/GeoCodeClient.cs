using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.Locations.GeoCodeAPI;
using Newtonsoft.Json;

namespace ATMCompass.Insfrastructure.HttpCLients
{
    public class GeoCodeClient : IGeoCodeClient
    {
        public GeoCodeClient()
        {
        }

        public async Task<GetLocationByCoordinatesResponse> GetLocationByCoordinatesAsync(string latitude, string longitude)
        {
            var httpClient = new HttpClient();
            var apiUrl = $"https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={latitude}&longitude={longitude}&localityLanguage=bs";

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<GetLocationByCoordinatesResponse>(content);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
