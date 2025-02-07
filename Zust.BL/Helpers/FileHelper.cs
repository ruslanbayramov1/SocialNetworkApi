using Microsoft.AspNetCore.Http;
using Zust.BL.Constants;

namespace Zust.BL.Helpers;

public static class FileHelper
{
    public static bool IsValidType(this IFormFile file) => file.ContentType.StartsWith(FileConstant.ImageType);
    public static bool IsValidSize(this IFormFile file) => file.Length <= FileConstant.ImageSize * 1024;
}
