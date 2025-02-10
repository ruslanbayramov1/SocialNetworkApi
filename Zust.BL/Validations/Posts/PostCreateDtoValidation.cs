using FluentValidation;
using Zust.BL.DTOs.Posts;
using Zust.DAL.Settings;

namespace Zust.BL.Validations.Posts;

public class PostCreateDtoValidation : AbstractValidator<PostCreateDto>
{
    public PostCreateDtoValidation()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .NotNull()
            .WithMessage("Content can not be empty.")
            .MaximumLength(PostSetting.ContentLength)
            .WithMessage($"Content can contain maximum {PostSetting.ContentLength} number of characters.");
    }
}
