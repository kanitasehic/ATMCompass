using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.ATMs.Responses;
using ATMCompass.Core.Models.GeoCalculator;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace ATMCompass.Insfrastructure.HttpCLients
{
    public class GraphHopperClient : IGraphHopperClient
    {
        public async Task<IList<List<Coordinate>>> GetIsohronesAsync(string transportType, string lat, string lon)
        {
            var httpClient = new HttpClient();
            var apiUrl = $"https://graphhopper.com/api/1/isochrone?point={lat},{lon}&buckets=3&time_limit=900&profile={transportType}&key=117366b4-dd76-4a06-96b8-1954207e0034";

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GetIsohronesResponse>(content);

                IList<List<Coordinate>> isohrones = new List<List<Coordinate>>();

                foreach (var feature in result.Polygons)
                {
                    Polygon geometry = (Polygon)feature.Geometry;
                    var coordinates = geometry.Coordinates.FirstOrDefault().Coordinates;

                    var isohroneCoordinates = new List<Coordinate>();

                    foreach (var coord in coordinates)
                    {
                        var coordinate = new Coordinate(coord.Latitude, coord.Longitude);
                        isohroneCoordinates.Add(coordinate);
                    }

                    isohrones.Add(isohroneCoordinates);
                }
                return isohrones;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
