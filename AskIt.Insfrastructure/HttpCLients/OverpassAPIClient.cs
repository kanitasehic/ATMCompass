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
            var atms = await GetATMsAsync();
            var banksWithAtms = await GetBanksWithATMsAsync();

            return (IList<GetATMFromOSMItem>)atms.Concat(banksWithAtms);
        }

        private async Task<IList<GetATMFromOSMItem>> GetATMsAsync()
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

        private async Task<IList<GetATMFromOSMItem>> GetBanksWithATMsAsync()
        {
            var httpClient = new HttpClient();
            var apiUrl = "https://overpass-api.de/api/interpreter";

            // The Overpass query to fetch ATM nodes in Bosnia and Herzegovina
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

            var requestData = new StringContent($"data={query}");
            var response = await httpClient.PostAsync(apiUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var ATMs = JsonConvert.DeserializeObject<GetATMsFromOSMResponse>(content);
                return ATMs.Elements.Where(a => a.Tags.WithinBank == "yes").ToList();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
