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
    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
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
            Posts = x.Posts.Select(y => new Post
            { 
                Comments = y.Comments,
                Content = y.Content,
                CreatedAt = y.CreatedAt,
                UpdatedAt = y.UpdatedAt,
                Id = y.Id,
                Likes = y.Likes,
                ImageUrl = y.ImageUrl,
                PostedUser = y.PostedUser,
                PostedUserId = y.PostedUserId,
                DeletedAt = x.DeletedAt,
                IsDeleted = x.IsDeleted
            }).ToList(),
            Role = x.Role,
            PostCommentLikes = x.PostCommentLikes,
            PostComments = x.PostComments,
            PostLikes = x.PostLikes,
            Followings = x.Followings,
            Followers = x.Followers,
        });
        if (user == null) throw new NotFoundException<User>();

        var likes = user.Posts.SelectMany(x => x.Likes);

        UserProfileGetDto? userProfile = await _userRepo.GetByIdAsync(id,x => new UserProfileGetDto {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CoverImageUrl = user.CoverImageUrl!,
            ProfileImageUrl = user.ProfileImageUrl!,
            Role = Enum.GetName(typeof(Roles), user.Role)!,
            UserName = user.UserName,
            FollowerCount = x.Followers.Count(),
            FollowingCount = x.Followings.Count(),
            LikeCount = likes.Count()
        });

        return userProfile;
    }
}
