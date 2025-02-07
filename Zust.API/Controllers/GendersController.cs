using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.Genders;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GendersController : ControllerBase
{
    private readonly IGenderService _genderService;
    public GendersController(IGenderService genderService)
    {
        _genderService = genderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _genderService.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _genderService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GenderCreateDto dto)
    {
        await _genderService.CreateAsync(dto);
        return Created();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, GenderUpdateDto dto)
    {
        await _genderService.UpdateAsync(id, dto);
        return Created();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _genderService.DeleteAsync(id);
        return NoContent();
    }
}
