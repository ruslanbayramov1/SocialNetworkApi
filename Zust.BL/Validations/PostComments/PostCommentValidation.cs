using FluentValidation;
using Zust.BL.DTOs.PostComments;
using Zust.DAL.Settings;

namespace Zust.BL.Validations.PostComments;

public class PostCommentValidation : AbstractValidator<PostCommentCreateDto>
{
    public PostCommentValidation()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .NotNull()
            .WithMessage("Content can not be empty.")
            .MaximumLength(PostCommentSetting.ContentLength)
            .WithMessage($"Content can contain maximum {PostCommentSetting.ContentLength} number of characters.");
    }
}
