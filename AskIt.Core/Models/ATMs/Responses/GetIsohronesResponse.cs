using Newtonsoft.Json;

namespace ATMCompass.Core.Models.ATMs.Responses
{
    public class GetIsohronesResponse
    {
        [JsonProperty("polygons")]
        public IList<GeoJSON.Net.Feature.Feature> Polygons { get; set; }
    }
}
