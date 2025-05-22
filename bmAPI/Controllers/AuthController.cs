using bmAPI.DTO;
using bmAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("banana")]
    public IActionResult Login([FromForm] string u, [FromForm] string p)
    {
        var result = _authService.Authenticate(u, p);

        if (result == null)
            return Unauthorized("Invalid credentials");

        return Ok(result);
    }
}
