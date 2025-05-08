using bmAPI.DTO;
using bmAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("bm/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("token")]
    [EnableRateLimiting("AuthLimiter")]
    public IActionResult Login([FromBody] AuthRequest request)
    {
        var result = _authService.Authenticate(request.Username, request.Password);

        if (result == null)
            return Unauthorized("Invalid credentials");

        return Ok(result); // will return an AuthResponse { Token, ExpiryTime }
    }
}