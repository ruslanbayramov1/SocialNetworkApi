using Microsoft.AspNetCore.Http;
using Zust.BL.Constants;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Files;

namespace Zust.BL.Helpers;

public static class FileHelper
{
    public static void IsValidTypeAndSize(this IFormFile file)
    {
        MediaTypes type = file.IsValidType();
        file.IsValidSize(type);
    }

    public static MediaTypes IsValidType(this IFormFile file)
    {
        if (file.ContentType.StartsWith(FileConstant.MusicType)) return MediaTypes.Music;
        if (file.ContentType.StartsWith(FileConstant.VideoType)) return MediaTypes.Video;
        if (file.ContentType.StartsWith(FileConstant.ImageType)) return MediaTypes.Image;

        throw new InvalidFileTypeException($"File type {Path.GetExtension(file.FileName)} is not valid.");
    }

    public static bool IsValidSize(this IFormFile file, MediaTypes mediaType)
    {
        if (mediaType == MediaTypes.Music)
            return file.Length <= FileConstant.MusicSize * 1024;
        if (mediaType == MediaTypes.Video)
            return file.Length <= FileConstant.VideoSize * 1024;
        if (mediaType == MediaTypes.Image)
            return file.Length <= FileConstant.ImageSize * 1024;

        throw new InvalidFileTypeException($"File size is not valid. {mediaType.ToString()} size is maximum of {FileConstant.MediaSizeValues[mediaType]}");
    }
}
