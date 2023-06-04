using Newtonsoft.Json;

namespace ATMCompass.Core.Models.OverpassAPI
{
    public class GetObjectsFromOSMResponse
    {
        [JsonProperty("elements")]
        public IList<GetObjectFromOSMItem> Elements { get; set; }
    }
}
