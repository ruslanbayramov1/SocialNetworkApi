namespace Zust.BL.DTOs.PostComments;

public class PostCommentCreateDto
{
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public Guid? ParentCommentId { get; set; }
}
