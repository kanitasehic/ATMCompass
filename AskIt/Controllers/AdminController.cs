using ATMCompass.Filters;
using ATMCompass.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using ATMCompass.Core.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ATMCompass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AdminController : ControllerBase
    {
        private readonly IATMService _ATMService;
        private readonly string _loggedUserId;

        public AdminController(IATMService ATMService)
        {
            _ATMService = ATMService;
        }

        [HttpPost("atms/sync")]
        public async Task<IActionResult> SynchronizeATMData()
        {
            await _ATMService.SynchronizeATMDataAsync();

            return Ok();
        }
    }
}
