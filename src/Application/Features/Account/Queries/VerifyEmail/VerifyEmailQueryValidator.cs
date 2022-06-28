namespace Carpool.Application.Features.Account.Queries.VerifyEmail;

public class VerifyEmailQueryValidator : AbstractValidator<VerifyEmailQuery>
{
    public VerifyEmailQueryValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("The email field can't be empty");
        //            .Matches($"^[A-Za-z0-9._%+-]+@test.com$").WithMessage("The email isn't valid, please enter an 'positivethinking.lu' email");
        RuleFor(v => v.Token)
            .NotEmpty().WithMessage("The token field can't be empty");
    }
}