using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.ATMs.OverpassAPI;
using Newtonsoft.Json;

namespace ATMCompass.Insfrastructure.HttpCLients
{
    public class OverpassAPIClient : IOverpassAPIClient
    {
        public OverpassAPIClient()
        {
        }

        public async Task<IList<GetATMFromOSMItem>> GetATMsInBosniaAndHerzegovinaAsync()
        {
            var httpClient = new HttpClient();
            var apiUrl = "https://overpass-api.de/api/interpreter";

            // The Overpass query to fetch ATM nodes in Bosnia and Herzegovina
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

            var requestData = new StringContent($"data={query}");
            var response = await httpClient.PostAsync(apiUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var ATMs = JsonConvert.DeserializeObject<GetATMsFromOSMResponse>(content);
                return ATMs.Elements;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
