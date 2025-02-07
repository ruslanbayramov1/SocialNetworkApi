using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.Occupations;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OccupationsController : ControllerBase
{
    private readonly IOccupationService _OccupationService;
    public OccupationsController(IOccupationService OccupationService)
    {
        _OccupationService = OccupationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _OccupationService.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _OccupationService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(OccupationCreateDto dto)
    {
        await _OccupationService.CreateAsync(dto);
        return Created();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, OccupationUpdateDto dto)
    {
        await _OccupationService.UpdateAsync(id, dto);
        return Created();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _OccupationService.DeleteAsync(id);
        return NoContent();
    }
}
