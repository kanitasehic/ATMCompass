using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ATMCompass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ATMsController : ControllerBase
    {
        private readonly IATMService _ATMService;

        public ATMsController(IATMService ATMService)
        {
            _ATMService = ATMService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SynchronizeATMData()
        {
            await _ATMService.SynchronizeATMDataAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetATMs([FromQuery] GetATMsRequest request)
        {
            var atms = await _ATMService.GetATMsAsync(request);
            return Ok(atms);
        }

        [HttpGet("cannibals")]
        public async Task<IActionResult> GetCannibalATMs([FromQuery] GetCannibalATMsRequest request)
        {
            var atms = await _ATMService.GetCannibalATMsAsync(request);

            return Ok(atms);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateATM([FromBody] UpdateATMRequest request, [FromRoute] int id)
        {
            await _ATMService.UpdateATMAsync(id, request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteATM([FromRoute] int id)
        {
            await _ATMService.DeleteATMAsync(id);

            return NoContent();
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetAllATMLocations()
        {
            var locations = await _ATMService.GetAllLocationsAsync();

            return Ok(locations);
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetAllATMBanks()
        {
            var banks = await _ATMService.GetAllBanksAsync();

            return Ok(banks);
        }

        [HttpPost("locations")]
        public async Task<IActionResult> AddLocations()
        {
            await _ATMService.SaveLocationFromGeoJsonAsync();

            return Ok();
        }

        [HttpGet("per-city")]
        public async Task<IActionResult> GetNumberOfATMsPerCity()
        {
            var numberOfATMsPerCity = _ATMService.GetNumberOfATMsPerCity();

            return Ok(numberOfATMsPerCity);
        }

        [HttpGet("per-municipality")]
        public async Task<IActionResult> GetNumberOfATMsPerMunicipality()
        {
            var numberOfATMsPerMunicipality = _ATMService.GetNumberOfATMsPerMunicipality();

            return Ok(numberOfATMsPerMunicipality);
        }

        [HttpGet("isohrones")]
        public async Task<IActionResult> GetIsohrones([FromQuery] GetIsohronesRequest request)
        {
            var isohrones = await _ATMService.GetIsohronesAsync(request);

            return Ok(isohrones);
        }
    }
}
