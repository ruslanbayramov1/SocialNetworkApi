using FluentValidation;
using Zust.BL.DTOs.Auths;
using Zust.DAL.Settings;

namespace Zust.BL.Validations.Auths;

public class NewPasswordDtoValidation : AbstractValidator<NewPasswordDto>
{
    public NewPasswordDtoValidation()
    {
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .MinimumLength(UserSetting.PasswordMinLength)
            .WithMessage($"Password must be at least {UserSetting.PasswordMinLength} characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.PasswordConfirm)
            .Equal(user => user.Password)
            .WithMessage("Passwords must match.");
    }
}
