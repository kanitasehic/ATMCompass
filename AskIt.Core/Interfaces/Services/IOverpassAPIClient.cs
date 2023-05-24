using ATMCompass.Core.Models.ATMs.OverpassAPI;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IOverpassAPIClient
    {
        Task<IList<GetATMFromOSMItem>> GetATMsInBosniaAndHerzegovinaAsync();
    }
}
