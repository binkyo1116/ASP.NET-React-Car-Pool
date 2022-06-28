namespace Carpool.Application.Features.Account.Queries.ForgotPassword;

public class ForgotPasswordQueryValidator : AbstractValidator<ForgotPasswordQuery>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ForgotPasswordQueryValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("The email field can't be empty")
            .MustAsync(BeExistingEmail).WithMessage("The email doesn't exist, you can register it");
        //            .Matches($"^[A-Za-z0-9._%+-]+@test.com$").WithMessage("The email isn't valid, please enter an 'positivethinking.lu' email");
    }

    public async Task<bool> BeExistingEmail(string email, CancellationToken cancellationToken)
       => await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email) != null;
}