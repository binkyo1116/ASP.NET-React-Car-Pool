namespace Carpool.Application.Features.Account.Queries.Login;

public class LoginQuery : IRequest<ApplicationUser>
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, ApplicationUser>
{
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;

    }
    public async Task<ApplicationUser> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetUserLoggedAsync(request.Email, request.Password);
    }
}
