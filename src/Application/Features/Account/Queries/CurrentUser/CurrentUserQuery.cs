namespace Carpool.Application.Features.Account.Queries.CurrentUser
{
    public class CurrentUserQuery : IRequest<ApplicationUser>
    {
    }

    public class CurrentUserQueryHandler : IRequestHandler<CurrentUserQuery, ApplicationUser>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public async Task<ApplicationUser> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var email = _currentUserService.Email;

            if (email is null) return null!;

            return await _identityService.GetCurrentUserByEmailAsync(email);
        }

        public CurrentUserQueryHandler(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;

        }
        
    }
}