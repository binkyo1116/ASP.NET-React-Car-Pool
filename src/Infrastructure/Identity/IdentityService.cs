namespace Carpool.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
        {
            return await DeleteUserAsync(user);
        }

        return Result.Success();
    }

    public async Task<ApplicationUser> GetUserLoggedAsync(string email, string password)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email);

        if (user is null) return new ApplicationUser();

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded ? user : new ApplicationUser();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<ApplicationUser> GetCurrentUserByEmailAsync(string email)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.Email == email);
        return user!;
    }

    public async Task<string> GetEmailConfirmationTokenAsync(ApplicationUser user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }
    public async Task<string> GetResetPasswordTokenAsync(ApplicationUser user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }


    public async Task<Result> VerifyResetPasswordTokenAsync(ApplicationUser user, string token)
    {
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.ToApplicationResult();
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string password)
    {
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        return result.ToApplicationResult();
    }
}
