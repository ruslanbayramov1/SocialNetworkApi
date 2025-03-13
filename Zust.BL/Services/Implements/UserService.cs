using Microsoft.AspNetCore.Http;
using Zust.BL.DTOs.Users;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IGenderRepository _genderRepo;
    private readonly IOccupationRepository _occupationRepo;
    private readonly IRelationStatusRepository _relStatusRepo;
    private readonly IBloodGroupRepository _bloodGroupRepo;
    private readonly ILanguageRepository _languageRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly IAzureCloudBlobService _azureCloudBlobService;
    public UserService(IUserRepository userRepo, IUserClaimService userClaimService, IGenderRepository genderRepository, IOccupationRepository occupationRepository, IRelationStatusRepository relationStatusRepository, IBloodGroupRepository bloodGroupRepository, ILanguageRepository languageRepository, IAzureCloudBlobService azureCloudBlobService)
    {
        _userRepo = userRepo;
        _userClaimService = userClaimService;
        _genderRepo = genderRepository;
        _occupationRepo = occupationRepository;
        _relStatusRepo = relationStatusRepository;
        _bloodGroupRepo = bloodGroupRepository;
        _languageRepo = languageRepository;
        _azureCloudBlobService = azureCloudBlobService;
    }

    public async Task<UserProfileGetDto> GetUserProfileById(Guid id)
    {
        bool res = await _userRepo.IsExistsAsync(id);
        if (!res) throw new NotFoundException<User>();

        UserProfileGetDto? userProfile = await _userRepo.GetByIdAsync(id,x => new UserProfileGetDto {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            CoverImageUrl = x.CoverImageUrl!,
            ProfileImageUrl = x.ProfileImageUrl!,
            Role = ((Roles)x.Role).ToString(),
            UserName = x.UserName,
            FollowerCount = x.Followers.Count(),
            FollowingCount = x.Followings.Count(),
            LikeCount = x.Posts.SelectMany(x => x.Likes).Count()
        });

        return userProfile;
    }

    public async Task<List<UserProfileGetDto>> GetUserProfileByName(string userName)
    {
        bool res = await _userRepo.IsExistsAsync(x => x.UserName.Contains(userName));
        if (!res) throw new NotFoundException<User>();

        List<UserProfileGetDto>? userProfile = await _userRepo.GetWhereAsync(x => x.UserName.Contains(userName), x => new UserProfileGetDto
        {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            CoverImageUrl = x.CoverImageUrl!,
            ProfileImageUrl = x.ProfileImageUrl!,
            Role = ((Roles)x.Role).ToString(),
            UserName = x.UserName,
            FollowerCount = x.Followers.Count(),
            FollowingCount = x.Followings.Count(),
            LikeCount = x.Posts.SelectMany(x => x.Likes).Count()
        });

        return userProfile;
    }

    public async Task<UserAccountGetDto> GetUserAccountByName(string userName)
    {
        bool res = await _userRepo.IsExistsAsync(x => x.UserName == userName);
        if (!res) throw new NotFoundException<User>();

        UserAccountGetDto? userProfile = await _userRepo.GetByExpressionAsync(x => x.UserName == userName, x => new UserAccountGetDto
        {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            CoverImageUrl = x.CoverImageUrl!,
            ProfileImageUrl = x.ProfileImageUrl!,
            Role = ((Roles)x.Role).ToString(),
            UserName = x.UserName,
            FollowerCount = x.Followers.Count(),
            FollowingCount = x.Followings.Count(),
            LikeCount = x.Posts.SelectMany(x => x.Likes).Count(),
            BloodGroup = x.BloodGroup.Name,
            DateOfBirth = x.DateOfBirth,
            Gender = x.Gender.Name,
            Language = x.Language.Name,
            Occupation = x.Occupation.Name,
            RelationStatus = x.RelationStatus.Name,
            Website = x.Website,
            IsPrivate = x.IsPrivate
        });

        return userProfile;
    }

    public async Task UpdateProfile(UserProfileUpdateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        if (dto.OccupationId.HasValue)
        {
            if (!await _occupationRepo.IsExistsAsync(dto.OccupationId.Value))
                throw new NotFoundException<Occupation>();
        }
        if (dto.GenderId.HasValue)
        {
            if (!await _genderRepo.IsExistsAsync(dto.GenderId.Value))
                throw new NotFoundException<Gender>();
        }
        if (dto.BloodGroupId.HasValue)
        {
            if (!await _bloodGroupRepo.IsExistsAsync(dto.BloodGroupId.Value))
                throw new NotFoundException("Blood group");
        }
        if (dto.LanguageId.HasValue)
        {
            if (!await _languageRepo.IsExistsAsync(dto.LanguageId.Value))
                throw new NotFoundException<Language>();
        }
        if (dto.RelationStatusId.HasValue)
        {
            if (!await _relStatusRepo.IsExistsAsync(dto.RelationStatusId.Value))
                throw new NotFoundException("Relation status");
        }

        user.Address = dto.Address;
        user.GenderId = dto.GenderId;
        user.DateOfBirth = dto.DateOfBirth;
        user.IsPrivate = dto.IsPrivate;
        user.BackupEmail = dto.BackupEmail;
        user.OccupationId = dto.OccupationId;
        user.RelationStatusId = dto.RelationStatusId;
        user.BloodGroupId = dto.BloodGroupId;
        user.LanguageId = dto.LanguageId;
        user.Website = dto.Website;

        await _userRepo.SaveAsync();
    }

    public async Task UpdateProfileImage(IFormFile image)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        image.IsValidTypeAndSize();

        user.ProfileImageUrl = await _azureCloudBlobService.UploadImageAsync(image, AzureFolderDestinations.Profiles);
        await _userRepo.SaveAsync();
    }

    public async Task UpdateProfileBanner(IFormFile banner)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        banner.IsValidTypeAndSize();

        user.CoverImageUrl = await _azureCloudBlobService.UploadImageAsync(banner, AzureFolderDestinations.Banners);
        await _userRepo.SaveAsync();
    }

    public async Task<User> GetById(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException<User>();

        return user;
    }

    public async Task<User> GetByName(string userName)
    {
        var user = await _userRepo.GetByExpressionAsync(x => x.UserName == userName);
        if (user == null) throw new NotFoundException<User>();

        return user;
    }

    public async Task DeleteProfileImageAsync()
    {
        var user = await GetById(_userClaimService.GetId());
        if (user.ProfileImageUrl != "https://zuststorage.blob.core.windows.net/media/defaultuserimage.jpg")
        {
            await _azureCloudBlobService.DeleteImageAsync(user.ProfileImageUrl);
        }
        user.ProfileImageUrl = "https://zuststorage.blob.core.windows.net/media/defaultuserimage.jpg";
        await _userRepo.SaveAsync();
    }

    public async Task DeleteProfileBannerAsync()
    {
        var user = await GetById(_userClaimService.GetId());
        if (user.CoverImageUrl != "https://zuststorage.blob.core.windows.net/media/defaultusercover.jpg")
        {
            await _azureCloudBlobService.DeleteImageAsync(user.CoverImageUrl);
        }
        user.CoverImageUrl = "https://zuststorage.blob.core.windows.net/media/defaultusercover.jpg";
        await _userRepo.SaveAsync();
    }

    public async Task<string> ChangeVisibilityAsync()
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user.IsPrivate == true)
        {
            user.IsPrivate = false;
            await _userRepo.SaveAsync();
            return "Visibility changed to public";
        }
        else
        { 
            user.IsPrivate = true;
            await _userRepo.SaveAsync();
            return "Visibility changed to private";
        }

    }
}
