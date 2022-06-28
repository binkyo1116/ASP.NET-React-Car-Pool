namespace Carpool.Application.Features.Account.Queries.ResendConfirmationEmail;

public class ResendConfirmationEmailQueryValidator : AbstractValidator<ResendConfirmationEmailQuery>
{
    public ResendConfirmationEmailQueryValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("The email field can't be empty");
        //            .Matches($"^[A-Za-z0-9._%+-]+@test.com$").WithMessage("The email isn't valid, please enter an 'positivethinking.lu' email");
    }
}