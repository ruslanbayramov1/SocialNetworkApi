using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.Languages;
using Zust.BL.Services.Interfaces;
using Zust.Core.Enums;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Auth(Roles.Admin | Roles.Moderator)]
public class LanguagesController : ControllerBase
{
    private readonly ILanguageService _LanguageService;
    public LanguagesController(ILanguageService LanguageService)
    {
        _LanguageService = LanguageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _LanguageService.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _LanguageService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(LanguageCreateDto dto)
    {
        await _LanguageService.CreateAsync(dto);
        return Created();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, LanguageUpdateDto dto)
    {
        await _LanguageService.UpdateAsync(id, dto);
        return Created();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _LanguageService.DeleteAsync(id);
        return NoContent();
    }
}
