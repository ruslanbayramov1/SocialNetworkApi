namespace Zust.BL.Constants;

public class PermissionMiddlewareEndpointConstant
{
    public const string PostsUserGET = "/api/Posts/User";
    public const string PostsPostGET = "/api/Posts/Post";
    public const string PostsCommentsGET = "/api/Posts/Comments";
    public const string PostsCommentGET = "/api/Posts/Comment";
    public const string PostsCommentPOST = "/api/Posts/Comment";
    public const string PostsRepliesGET = "/api/Posts/Replies";
    public const string PostsLikesGET = "/api/Posts/Likes";
    public const string PostsLikePOST = "/api/Posts/Like";
    public const string PostsCommentLikesGET = "/api/Posts/Comment/Likes";
    public const string PostsCommentLikePOST = "/api/Posts/CommentLike";

    public static string[] UserIdEnpoints = { PostsUserGET };
    public static string[] PostIdEnpoints = { PostsPostGET, PostsCommentsGET, PostsCommentPOST , PostsRepliesGET, PostsLikesGET, PostsLikePOST };
    public static string[] CommentIdEnpoints = { PostsCommentGET, PostsCommentLikesGET, PostsCommentLikePOST };
}
