using AskIt.Filters;
using ATMCompass.Core.Interfaces.Services;
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
        public async Task<IActionResult> GetATMs()
        {
            return Ok();
        }
    }
}
