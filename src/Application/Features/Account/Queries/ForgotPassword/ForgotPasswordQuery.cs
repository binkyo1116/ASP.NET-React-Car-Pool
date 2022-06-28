namespace Carpool.Application.Features.Account.Queries.ForgotPassword;

public class ForgotPasswordQuery : IRequest<string>
{
    public string Email { get; set; } = String.Empty;
}

public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQuery, string>
{
    private readonly IIdentityService _identityService;

    public ForgotPasswordQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is not null)
        {
            return await _identityService.GetResetPasswordTokenAsync(user);
        }
        return String.Empty;
    }
}
