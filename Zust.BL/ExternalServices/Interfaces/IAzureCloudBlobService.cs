using Microsoft.AspNetCore.Http;
using Zust.BL.Enums;

namespace Zust.BL.ExternalServices.Interfaces;

public interface IAzureCloudBlobService
{
    Task<string> UploadImageAsync(IFormFile file, AzureFolderDestinations folderDestinations);
}
