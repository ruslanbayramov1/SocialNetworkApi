using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.Auths;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthsController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(RegisterDto dto)
    { 
        await _authService.RegisterAsync(dto);
        return Created();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        string token = await _authService.LoginAsync(dto);
        return StatusCode(StatusCodes.Status201Created,token);
    }
}
