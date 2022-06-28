namespace Carpool.Application.Features.Account.Commands.ResetPassword;


public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; } = String.Empty;
    public string Token { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is not null)
        {
            var result = await _identityService.ResetPasswordAsync(user, request.Token, request.Password);
            return result.Succeeded;
        }
        return false;
    }
}
