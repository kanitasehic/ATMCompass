using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ATMCompass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class TransportsController : ControllerBase
    {
        private readonly IATMService _ATMService;

        public TransportsController(IATMService ATMService)
        {
            _ATMService = ATMService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SynchronizeTransportData()
        {
            await _ATMService.SynchronizeTransportDataAsync();

            return NoContent();
        }
    }
}
