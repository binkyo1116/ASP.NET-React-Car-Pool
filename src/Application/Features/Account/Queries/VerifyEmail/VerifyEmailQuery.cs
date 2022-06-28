namespace Carpool.Application.Features.Account.Queries.VerifyEmail;

public class VerifyEmailQuery : IRequest<bool>
{
    public string Email { get; set; } = String.Empty;
    public string Token { get; set; } = String.Empty;
}

public class VerifyEmailQueryHandler : IRequestHandler<VerifyEmailQuery, bool>
{

    private readonly IIdentityService _identityService;

    public VerifyEmailQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> Handle(VerifyEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is null) return false;


        var result = await _identityService.VerifyResetPasswordTokenAsync(user, request.Token);

        return result.Succeeded;
    }
}