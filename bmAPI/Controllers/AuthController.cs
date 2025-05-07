using bmAPI.Services;
using Microsoft.AspNetCore.Mvc;
using bmAPI.DTO;

namespace bmAPI.Controllers
{
    [Route("bm/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

     /*   [HttpPost("GetProviderData")] // chainname and datetime?
        public async Task<IActionResult> SignIn([FromBody] AuthRequest authRequest)
        {
            var authResponse = await _authService.Authenticate(authRequest);

            return Ok(authResponse);
        }*/
    }
}
