namespace Carpool.Application.Features.Account.Queries.ResendConfirmationEmail;

public class ResendConfirmationEmailQuery : IRequest<string>
{
    public string Email { get; set; } = String.Empty;
}

public class ResendConfirmationEmailQueryHandler : IRequestHandler<ResendConfirmationEmailQuery, string>
{
    private readonly IIdentityService _identityService;

    public ResendConfirmationEmailQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(ResendConfirmationEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByEmailAsync(request.Email);

        if (user is not null)
        {
            return await _identityService.GetEmailConfirmationTokenAsync(user);
        }
        return String.Empty;
    }
}
