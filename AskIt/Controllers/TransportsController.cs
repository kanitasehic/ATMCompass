using ATMCompass.Core.Interfaces.Services;
using ATMCompass.Core.Models.Transports.Requests;
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

        [HttpGet("without-atms-around")]
        public async Task<IActionResult> GetTransportsWithoutATMsAround([FromQuery] GetTransportsRequest request)
        {
            var transports = await _ATMService.GetTransportsWithoutATMsAroundAsync(request);

            return Ok(transports);
        }
    }
}
