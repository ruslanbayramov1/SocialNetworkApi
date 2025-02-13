using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly ILanguageRepository _languageRepo;
    public UserService(IUserRepository userRepo, ILanguageRepository languageRepo)
    {
        _userRepo = userRepo;
        _languageRepo = languageRepo;
    }

    public async Task<UserGetDto> GetUserById(Guid id)
    {
        var userGet = await _userRepo.GetByIdAsync(id);
        if (userGet == null) throw new NotFoundException<User>();

        var roleName = Enum.GetName(typeof(Roles), userGet.Role);

        var userProfile = await _userRepo.GetByIdAsync(id, user => new UserGetDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CoverImageUrl = user.CoverImageUrl!,
            ProfileImageUrl = user.ProfileImageUrl!,
            Role = roleName,
            UserName = user.UserName,
            Address = user.Address,
            BackupEmail = user.BackupEmail,
            BloodGroup = user.BloodGroup.Name,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender.Name,
            Website = user.Website,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Occupation = user.Occupation.Name,
            Language = user.Language.Name,
            RelationStatus = user.RelationStatus.Name,
            Posts = user.Posts.Select(x => new PostGetDto
            {
                Id = x.Id,
                Content = x.Content,
                ImageUrl = x.ImageUrl,
                LikeCount = x.Likes.Count(),
                Comments = x.Comments
                .Where(y => y.ParentCommentId == null)
                .Select(y => new PostCommentGetDto
                {
                    Id = y.Id,
                    PostId = y.PostId,
                    Content = y.Content,
                    ParentCommentId = y.ParentCommentId,
                }).ToList(),
            }).ToList(),
        }); 

        return userProfile;
    }

    public async Task<UserProfileGetDto> GetUserProfileById(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id, x=> new User
        {
            Address = x.Address,
            CreatedAt = x.CreatedAt,
            BackupEmail = x.BackupEmail,
            BloodGroup = x.BloodGroup,
            BloodGroupId = x.BloodGroupId,
            CoverImageUrl = x.CoverImageUrl,
            DateOfBirth = x.DateOfBirth,
            DeletedAt = x.DeletedAt,
            Email = x.Email,
            FirstName = x.FirstName,
            Gender = x.Gender,
            GenderId = x.GenderId,
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            IsEmailConfirmed = x.IsEmailConfirmed,
            Language = x.Language,
            LanguageId = x.LanguageId,
            LastName = x.LastName,
            Occupation = x.Occupation,
            OccupationId = x.OccupationId,
            ProfileImageUrl = x.ProfileImageUrl,
            RelationStatus = x.RelationStatus,
            RelationStatusId = x.RelationStatusId,
            UpdatedAt = x.UpdatedAt,
            UserName = x.UserName,
            Website = x.Website,
            Posts = x.Posts,
            Role = x.Role,
            PostCommentLikes = x.PostCommentLikes,
            PostComments = x.PostComments,
            PostLikes = x.PostLikes,
        });
        if (user == null) throw new NotFoundException<User>();

        int count = user.Posts.Sum(x => x.Likes.Count());

        UserProfileGetDto? userProfile = await _userRepo.GetByIdAsync(id,x => new UserProfileGetDto {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CoverImageUrl = user.CoverImageUrl!,
            ProfileImageUrl = user.ProfileImageUrl!,
            Role = Enum.GetName(typeof(Roles), user.Role)!,
            UserName = user.UserName,
            FollowerCount = 0,
            FollowingCount = 0,
            LikeCount = count
        });

        return userProfile;
    }
}
