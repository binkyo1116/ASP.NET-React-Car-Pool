namespace Carpool.Application.Features.Account.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("The email field can't be empty");
        //    .Matches($"^[A-Za-z0-9._%+-]+@test.com$").WithMessage("The email isn't valid, please enter an 'positivethinking.lu' email");
        RuleFor(v => v.Token)
            .NotEmpty().WithMessage("The token field can't be empty");
        RuleFor(v => v.Password)
       .NotEmpty().WithMessage("The password should contain at least 8 characters")
       .MinimumLength(8).WithMessage("The password should contain at least 8 characters")
       .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
           .WithMessage($"The password isn't valid, it should contain at least one upperase letter, one lowercase letter, one number and one special character");
    }
}