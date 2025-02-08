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

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SendEmailConfirmation()
    {
        string res = await _authService.SendEmailConfirmationAsync();
        return Ok(res);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> VerifyEmail(string code)
    {
        await _authService.VerifyEmail(code);
        return Ok("Email successfully confirmed!");
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SendNewPasswordEmail(string oldPassword)
    {
        string res = await _authService.SendNewPasswordEmailAsync(oldPassword);
        return Ok(res);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SetNewPassword([FromHeader]string code, NewPasswordDto dto)
    {
        await _authService.SetNewPassword(code, dto);
        return Ok("");
    }
}
