using Zust.BL.Enums;

namespace Zust.BL.Constants;

public class FileConstant
{
    public FileConstant()
    {
        MediaSizeValues.Add(MediaTypes.Music, MusicSize);
        MediaSizeValues.Add(MediaTypes.Video, VideoSize);
        MediaSizeValues.Add(MediaTypes.Image, ImageSize);
    }

    public static Dictionary<MediaTypes, int> MediaSizeValues = new Dictionary<MediaTypes, int>();

    public const string ImageType = "image";
    public const int ImageSize = 20480;

    public const string VideoType = "video/mp4";
    public const int VideoSize = 30720;

    public const string MusicType = "audio/mpeg";
    public const int MusicSize = 10240;
}
