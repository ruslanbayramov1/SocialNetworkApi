using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.RelationStatuses;
using Zust.BL.Services.Interfaces;
using Zust.Core.Enums;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Auth(Roles.Admin | Roles.Moderator)]
public class RelationStatusesController : ControllerBase
{
    private readonly IRelationStatusService _RelationStatusService;
    public RelationStatusesController(IRelationStatusService RelationStatusService)
    {
        _RelationStatusService = RelationStatusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _RelationStatusService.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _RelationStatusService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(RelationStatusCreateDto dto)
    {
        await _RelationStatusService.CreateAsync(dto);
        return Created();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, RelationStatusUpdateDto dto)
    {
        await _RelationStatusService.UpdateAsync(id, dto);
        return Created();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _RelationStatusService.DeleteAsync(id);
        return NoContent();
    }
}
