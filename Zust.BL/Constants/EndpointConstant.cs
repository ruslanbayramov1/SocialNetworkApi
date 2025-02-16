namespace Zust.BL.Constants;

public class EndpointConstant
{
    /// <summary>
    /// Profile stats info of the user, requires /{userId}
    /// </summary>
    public const string UserProfileGet = $"api/Users/GetProfileById";
    /// <summary>
    /// Post enpoint, requires /{postId}
    /// </summary>
    public const string PostGet = "api/Posts/GetById";
    /// <summary>
    /// Comment enpoint, requires /{commentId}
    /// </summary>
    public const string CommentGet = "api/Posts/Comment";
    /// <summary>
    /// Comment enpoint, requires /{commentId}
    /// </summary>
    public const string StoryGet = "api/Stories/GetById";
}
