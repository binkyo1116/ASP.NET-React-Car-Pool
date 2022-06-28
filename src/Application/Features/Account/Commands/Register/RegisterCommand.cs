namespace Carpool.Application.Features.Account.Commands.Register;

public class RegisterCommand : IRequest<string>
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Username
        };

        var result = await _identityService.CreateUserAsync(user, request.Password);

        if (!result.Result.Succeeded) return String.Empty;

        return await _identityService.GetEmailConfirmationTokenAsync(user);
    }
}
