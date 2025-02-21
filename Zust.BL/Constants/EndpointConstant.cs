namespace Zust.BL.Constants;

public class EndpointConstant
{
    /// <summary>
    /// Profile stats info of the user, requires /{userId}
    /// </summary>
    public const string UserProfileGet = $"api/Users/Profile";
    /// <summary>
    /// Profile stats info of the user, requires /{userId}
    /// </summary>
    public const string UserAccountGet = $"api/Users/Account";
    /// <summary>
    /// Post enpoint, requires /{postId}
    /// </summary>
    public const string PostGet = "api/Posts/Post";
    /// <summary>
    /// Comment enpoint, requires /{commentId}
    /// </summary>
    public const string CommentGet = "api/Posts/Comment";
    /// <summary>
    /// Comment enpoint, requires /{commentId}
    /// </summary>
    public const string StoryGet = "api/Stories/Story";
}
