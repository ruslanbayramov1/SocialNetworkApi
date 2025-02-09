using Zust.BL.Constants;
using Zust.BL.DTOs.Posts;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Common;
using Zust.BL.Exceptions.Files;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserClaimService _userClaimService;
    private readonly IUserRepository _userRepo;
    private readonly IAzureCloudBlobService _azureCloudBlobService;
    public PostService(IPostRepository postRepository, IUserClaimService userClaimService, IUserRepository userRepo, IAzureCloudBlobService azureCloudBlobService)
    {
        _postRepository = postRepository;
        _userClaimService = userClaimService;
        _userRepo = userRepo;
        _azureCloudBlobService = azureCloudBlobService;
    }

    public async Task CreatePostAsync(PostCreateDto vm)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        string? imageUrl = null;
        if (vm.Image != null)
        {
            if (!vm.Image.IsValidSize())
            {
                throw new InvalidFileSizeException($"The image size is invalid. Maximum allowed size is {FileConstant.ImageSize / 1024} mb");
            }
            else if(!vm.Image.IsValidType())
            {
                throw new InvalidFileTypeException($"The image type is invalid. Allowed ones are any types of images.");
            }
            imageUrl = await _azureCloudBlobService.UploadImageAsync(vm.Image, AzureFolderDestinations.Posts);
        }

        var model = new Post
        {
            Content = vm.Content,
            PostedUserId = user.Id,
            ImageUrl = imageUrl,
        };
        await _postRepository.AddAsync(model);
        await _postRepository.SaveAsync();
    }
}
