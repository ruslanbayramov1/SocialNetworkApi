using FluentValidation;
using Zust.BL.DTOs.Auths;
using Zust.DAL.Settings;

namespace Zust.BL.Validations.Auths;

public class RegisterDtoValidation : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidation()
    {
        RuleFor(user => user.UserName)
            .NotEmpty()
            .WithMessage("Username cannot be empty.")
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("Username can only contain letters and numbers.")
            .Length(UserSetting.UserNameMinLength, UserSetting.UserNameLength)
            .WithMessage($"Username must be between {UserSetting.UserNameMinLength} and {UserSetting.UserNameLength} characters.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .MaximumLength(UserSetting.EmailLength)
            .WithMessage($"Email can be a maximum of {UserSetting.EmailLength} characters.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(user => user.FirstName)
           .NotEmpty()
           .WithMessage("First Name cannot be empty.")
           .MaximumLength(UserSetting.FirstNameLength)
           .WithMessage($"First Name can be maximum {UserSetting.FirstNameLength} characters.");

        RuleFor(user => user.LastName)
           .NotEmpty()
           .WithMessage("Last Name cannot be empty.")
           .MaximumLength(UserSetting.LastNameLength)
           .WithMessage($"Last Name can be maximum {UserSetting.LastNameLength} characters.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .MinimumLength(UserSetting.PasswordMinLength)
            .WithMessage($"Password must be at least {UserSetting.PasswordMinLength} characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.ConfirmPassword)
            .Equal(user => user.Password)
            .WithMessage("Passwords must match.");
    }
}
