namespace Zust.BL.DTOs.AiDatas;

public class FileResponse
{
    public string? Object { get; set; }
    public string id { get; set; }
    public string purpose { get; set; }
    public string filename { get; set; }
    public int bytes { get; set; }
    public long created_at { get; set; }
    public object? expires_at { get; set; }
    public string? status { get; set; }
    public object? status_details { get; set; }
}
