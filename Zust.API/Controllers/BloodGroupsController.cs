using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.BloodGroups;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BloodGroupsController : ControllerBase
{
    private readonly IBloodGroupService _BloodGroupService;
    public BloodGroupsController(IBloodGroupService BloodGroupService)
    {
        _BloodGroupService = BloodGroupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _BloodGroupService.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _BloodGroupService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(BloodGroupCreateDto dto)
    {
        await _BloodGroupService.CreateAsync(dto);
        return Created();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, BloodGroupUpdateDto dto)
    {
        await _BloodGroupService.UpdateAsync(id, dto);
        return Created();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _BloodGroupService.DeleteAsync(id);
        return NoContent();
    }
}
