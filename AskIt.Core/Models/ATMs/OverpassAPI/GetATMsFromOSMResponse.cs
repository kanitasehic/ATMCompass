using Newtonsoft.Json;

namespace ATMCompass.Core.Models.ATMs.OverpassAPI
{
    public class GetATMsFromOSMResponse
    {
        [JsonProperty("elements")]
        public IList<GetATMFromOSMItem> Elements { get; set; }
    }
}
