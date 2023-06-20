using ATMCompass.Core.Models.GeoCalculator;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IGraphHopperClient
    {
        Task<IList<List<Coordinate>>> GetIsohronesAsync(string transportType, string lat, string lon);
    }
}
