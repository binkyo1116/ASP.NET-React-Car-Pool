using Carpool.Application.Features.Account.Commands.Register;

namespace Carpool.Application.IntegrationTests.Features.Account;

using static Testing;

public class RegisterTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new RegisterCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireSpecificEmailSuffixe()
    {
        var command = new RegisterCommand
        {
            Username = "Username",
            Password = "Pa$$word13",
            Email = "test@testb.com"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueUsername()
    {
        var command = new RegisterCommand
        {
            Username = "Username",
            Password = "Pa$$word13",
            Email = "test@test.com"
        };

        var user = await SendAsync(command);

        command.Email = "test2@test.com";

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueEmail()
    {
        var command = new RegisterCommand
        {
            Username = "Username",
            Password = "Pa$$word13",
            Email = "test@test.com"
        };

        var user = await SendAsync(command);

        command.Username = "Username2";

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }


    [Test]
    public async Task ShouldBeStrongPassword()
    {
        var command = new RegisterCommand
        {
            Username = "Username",
            Password = "Password",
            Email = "test@test.com"
        };


        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();

        command.Password = "Password13";

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();

        command.Password = "Pa$$word#";

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();

        command.Password = "Pa$$word13";

        var token = await SendAsync(command);

        token.Should().NotBeNullOrEmpty();
    }


    [Test]
    public async Task ShouldSucceed()
    {
        var command = new RegisterCommand
        {
            Username = "Username",
            Password = "Pa$$word13",
            Email = "test@test.com"
        };

        var token = await SendAsync(command);

        token.Should().NotBeNullOrEmpty();
    }

}