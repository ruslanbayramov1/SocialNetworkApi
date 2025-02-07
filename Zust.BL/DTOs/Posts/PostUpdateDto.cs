using Microsoft.AspNetCore.Http;

namespace Zust.BL.DTOs.Posts;

public class PostUpdateDto
{
    public string Content { get; set; } = null!;
    public IFormFile? Image { get; set; }
}
