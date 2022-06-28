namespace Carpool.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<ApplicationUser> GetUserByEmailAsync(string email);

    Task<(Result Result, string UserId)> CreateUserAsync(ApplicationUser user, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<ApplicationUser> GetUserLoggedAsync(string email, string password);

    Task<ApplicationUser> GetCurrentUserByEmailAsync(string email);

    Task<string> GetEmailConfirmationTokenAsync(ApplicationUser user);

    Task<string> GetResetPasswordTokenAsync(ApplicationUser user);

    Task<Result> VerifyResetPasswordTokenAsync(ApplicationUser user, string token);

    Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string password);

}
