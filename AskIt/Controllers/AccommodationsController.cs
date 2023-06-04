using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ATMCompass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AccommodationsController : ControllerBase
    {
        private readonly IATMService _ATMService;

        public AccommodationsController(IATMService ATMService)
        {
            _ATMService = ATMService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SynchronizeAccommodationData()
        {
            await _ATMService.SynchronizeAccommodationDataAsync();

            return NoContent();
        }
    }
}
