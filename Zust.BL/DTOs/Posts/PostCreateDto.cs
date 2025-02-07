using Microsoft.AspNetCore.Http;

namespace Zust.BL.DTOs.Posts;

public class PostCreateDto
{
    public string Content { get; set; } = null!;
    public IFormFile? Image { get; set; }
}
