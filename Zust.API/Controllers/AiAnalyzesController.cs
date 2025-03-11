using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Zust.BL.DTOs.AiDatas;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AiAnalyzesController : ControllerBase
{
    private readonly IAiAnalyzeService _aiAnalyzeService;

    public AiAnalyzesController(IAiAnalyzeService aiAnalyzeService)
    {
        _aiAnalyzeService = aiAnalyzeService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var data = await _aiAnalyzeService.UploadFileAsync(file);
        return Ok(data);
    }

    [HttpGet("Files")]
    public async Task<IActionResult> Files()
    {
        var data = await _aiAnalyzeService.GetAllFilesAsync();
        return Ok(data);
    }

    [HttpGet("Files/{fileId}")]
    public async Task<IActionResult> Files(string fileId)
    {
        var data = await _aiAnalyzeService.GetFileByIdAsync(fileId);
        return Ok(data);
    }

    [HttpPost("Files/{fileId}/Analyze")]
    public async Task<IActionResult> Analyze(string fileId)
    {
        var data = await _aiAnalyzeService.AnalyzeAsync(fileId);
        return Ok(data);
    }
}
