namespace Carpool.Application.Features.Account.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email field can't be empty");
        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password field can't be empty");
    }
}
