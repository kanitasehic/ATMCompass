using ATMCompass.Filters;
using ATMCompass.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using ATMCompass.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using ATMCompass.Core.Models.ATMs.Requests;

namespace ATMCompass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.ADMIN_ROLE)]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AdminController : ControllerBase
    {
        private readonly IATMService _ATMService;

        public AdminController(IATMService ATMService)
        {
            _ATMService = ATMService;
        }

        [HttpPost("atms/sync")]
        public async Task<IActionResult> SynchronizeATMData()
        {
            await _ATMService.SynchronizeATMDataAsync();

            return NoContent();
        }

        [HttpPost("atms")]
        public async Task<IActionResult> AddATM([FromBody] AddATMRequest request)
        {
            var atm = await _ATMService.AddATMAsync(request);

            return Ok(atm);
        }

        [HttpPut("atms/{id}")]
        public async Task<IActionResult> UpdateATM([FromBody] UpdateATMRequest request, [FromRoute] int id)
        {
            await _ATMService.UpdateATMAsync(id, request);

            return NoContent();
        }

        [HttpDelete("atms/{id}")]
        public async Task<IActionResult> DeleteATM([FromRoute] int id)
        {
            await _ATMService.DeleteATMAsync(id);

            return NoContent();
        }
    }
}
