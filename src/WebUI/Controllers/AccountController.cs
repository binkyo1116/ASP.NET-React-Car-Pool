using Carpool.Application.Features.Account.Commands.Register;
using Carpool.Application.Features.Account.Commands.ResetPassword;
using Carpool.Application.Features.Account.Queries.CurrentUser;
using Carpool.Application.Features.Account.Queries.ForgotPassword;
using Carpool.Application.Features.Account.Queries.Login;
using Carpool.Application.Features.Account.Queries.ResendConfirmationEmail;
using Carpool.Application.Features.Account.Queries.VerifyEmail;

namespace Carpool.WebUI.Controllers;

[Authorize]
public class AccountController : ApiControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IEmailSender _emailSender;
    public AccountController(TokenService tokenService, IEmailSender emailSender)
    {
        _tokenService = tokenService;
        _emailSender = emailSender;
    }


    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginQuery query)
    {
        var userLogged = await Mediator.Send(query);

        if (string.IsNullOrEmpty(userLogged.UserName)) return BadRequest("Invalid email, password or email not confirmed yet");

        return Ok(CreateUserObject(userLogged));
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterCommand command)
    {
        var token = await Mediator.Send(command);

        if (String.IsNullOrEmpty(token)) return Ok();

        await SendVerificationEmail(command.Email, token);

        return Ok();
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("verifyEmail")]
    public async Task<IActionResult> VerifyEmail([FromQuery] VerifyEmailQuery query)
    {
        query.Token = _tokenService.DecodeToken(query.Token);

        var succeeded = await Mediator.Send(query);

        if (!succeeded) return BadRequest("Could not verify email address");

        return Ok("Email confirmed you can now login");
    }


    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpGet("resendEmailConfirmationLink")]
    public async Task<IActionResult> ResendEmailConfirmationLink([FromQuery] string email)
    {
        var token = await Mediator.Send(new ResendConfirmationEmailQuery { Email = email });

        if (String.IsNullOrEmpty(token)) return BadRequest();

        await SendVerificationEmail(email, token);

        return Ok();
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var token = await Mediator.Send(new ForgotPasswordQuery { Email = email });

        if (String.IsNullOrEmpty(token)) return BadRequest();

        await SendForgotPasswordEmail(email, token);

        return Ok();
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        command.Token = _tokenService.DecodeToken(command.Token);
        var succeeded = await Mediator.Send(command);

        if (!succeeded) return BadRequest();

        return Ok();
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await Mediator.Send(new CurrentUserQuery());
        if (user is null) return BadRequest("Problem getting the current user");
        return CreateUserObject(user);
    }
    private UserDto CreateUserObject(ApplicationUser user)
    {
        return new UserDto { Token = _tokenService.CreateToken(user), Username = user.UserName };
    }

    private async Task SendVerificationEmail(string email, string token)
    {
        var origin = Request.Headers["origin"];

        token = _tokenService.EncodeToken(token);

        var verifyUrl = $"{origin}/verifyEmail?token={token}&email={email}";

        var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify Email</a></p><p>The link will be alive two hours</p>";

        await _emailSender.SendEmailAsync(email, "Please verify email", message);
    }

    private async Task SendForgotPasswordEmail(string email, string token)
    {
        var origin = Request.Headers["origin"];

        token = _tokenService.EncodeToken(token);

        var resetUrl = $"{origin}/resetPassword?token={token}&email={email}";
        var message = $"<p>Please click the below link to reset your password:</p><p><a href='{resetUrl}'>Click to reset Password</a></p><p>The link will be alive two hours</p>";

        await _emailSender.SendEmailAsync(email, "Please verify email", message);
    }
}
