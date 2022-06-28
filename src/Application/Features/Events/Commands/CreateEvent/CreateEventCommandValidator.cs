namespace Carpool.Application.Features.Events.Commands.CreateEvent;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(v => v.Date)
            .NotNull().WithMessage("Date must be selected");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name can't be empty");
        RuleFor(v => v.File)
            .NotEmpty().WithMessage("File can't be empty");
    }
}
