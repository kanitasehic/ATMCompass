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

        [HttpGet]
        public async Task<IActionResult> GetATMs([FromQuery] GetATMsRequest request)
        {
            var atms = await _ATMService.GetATMsAsync(request);
            return Ok(atms);
        }
    }
}
