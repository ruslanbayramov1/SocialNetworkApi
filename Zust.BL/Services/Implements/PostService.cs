using Zust.BL.Services.Interfaces;

namespace Zust.BL.Services.Implements;

public class PostService : IPostService
{
    //private readonly IPostRepository _postRepository;
    //private readonly IUserClaimsService _userClaimsService;
    //private readonly UserManager<User> _userManager;
    //private readonly IAzureCloudBlobService _azureCloudBlobService;
    //public PostService(IPostRepository postRepository, IUserClaimsService userClaimsService, UserManager<User> userManager, IAzureCloudBlobService azureCloudBlobService)
    //{
    //    _postRepository = postRepository;
    //    _userClaimsService = userClaimsService;
    //    _userManager = userManager;
    //    _azureCloudBlobService = azureCloudBlobService;
    //}

    //public async Task CreatePostAsync(PostCreateDto vm)
    //{
    //    var user = await _userManager.FindByIdAsync(_userClaimsService.GetUserId());
    //    if (user == null) throw new NotFoundException<User>();

    //    string? imageUrl = null;
    //    if (vm.Image != null)
    //    {
    //        if (!vm.Image.IsValidSize() || !vm.Image.IsValidType())
    //        {
    //            throw new Exception();
    //        }
    //        imageUrl = await _azureCloudBlobService.UploadImageAsync(vm.Image);
    //    }

    //    var model = new Post
    //    {
    //        Content = vm.Content,
    //        PostedUserId = user.Id,
    //        ImageUrl = imageUrl,
    //    };
    //    await _postRepository.AddAsync(model);
    //    await _postRepository.SaveAsync();
    //}

    //public Task<List<PostGetDto>> GetProfilePosts()
    //{
    //    var data = _postRepository.GetAllAsync(x => new PostGetDto
    //    {
    //        ImageUrl = x.ImageUrl,
    //        Content = x.Content,
    //        LikeCount = x.Likes.Count,
    //        VideoUrl = x.VideoUrl,
    //    });

    //    return data;
    //}
}
