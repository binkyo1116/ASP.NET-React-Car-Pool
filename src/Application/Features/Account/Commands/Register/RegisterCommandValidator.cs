
namespace Carpool.Application.Features.Account.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCommandValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        RuleFor(v => v.Username)
                   .NotEmpty().WithMessage("Username field can't be empty nor exceed 15 characters")
                   .MaximumLength(15).WithMessage("Username can't exceed 15 characters")
                   .MustAsync(BeUniqueUsername).WithMessage("The specified Username already exists.");
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("The email field can't be empty")
            //.Matches($"^[A-Za-z0-9._%+-]+@test.com$").WithMessage("The email isn't valid, please enter an 'positivethinking.lu' email")
            .MustAsync(BeUniqueEmail).WithMessage("The specified Email already exists.");
        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("The password should contain at least 8 characters")
            .MinimumLength(8).WithMessage("The password should contain at least 8 characters")
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                .WithMessage($"The password isn't valid, it should contain at least one upperase letter, one lowercase letter, one number and one special character");
    }

    public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .AllAsync(u => u.Email.ToLower() != email.ToLower());
    }
    public async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .AllAsync(u => u.UserName.ToLower() != username.ToLower());
    }
}