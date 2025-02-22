namespace Zust.API.Constants;

public class PermissionMiddlewareEndpointConstant
{
    public const string PostsController = "Posts"; 

    public const string PostsUserGET = "GetUserPosts";
    public const string PostsPostGET = "GetPost";
    public const string PostsCommentsGET = "GetPostComments";
    public const string PostsCommentGET = "GetComment";
    public const string PostsCommentPOST = "CreateComment";
    public const string PostsRepliesGET = "GetCommentReplies";
    public const string PostsLikesGET = "GetPostLikes";
    public const string PostsLikePOST = "CreatePostLike";
    public const string PostsCommentLikesGET = "GetCommentLikes";
    public const string PostsCommentLikePOST = "CreateCommentLike";

    public static string[] RequiredControllers = { PostsController };

    public static string[] UserIdEndpointMethods = { PostsUserGET };
    public static string[] PostIdEnpdointMethods = { PostsPostGET, PostsCommentsGET, PostsCommentPOST , PostsLikesGET, PostsLikePOST };
    public static string[] CommentIdEndpointMethods = { PostsCommentGET, PostsRepliesGET, PostsCommentLikesGET, PostsCommentLikePOST };
}
