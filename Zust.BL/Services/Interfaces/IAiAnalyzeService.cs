using Microsoft.AspNetCore.Http;
using Zust.BL.DTOs.AiDatas;

namespace Zust.BL.Services.Interfaces;

public interface IAiAnalyzeService
{
    Task<FileResponse> UploadFileAsync(IFormFile file);
    Task<FileListResponse> GetAllFilesAsync();
    Task<FileResponse> GetFileByIdAsync(string fileId);
    Task<string> AnalyzeAsync(string fileId);
}
