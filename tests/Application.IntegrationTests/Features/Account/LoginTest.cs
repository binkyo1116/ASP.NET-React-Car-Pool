using Carpool.Application.Features.Account.Commands.Register;
using Carpool.Application.Features.Account.Queries.Login;

namespace Carpool.Application.IntegrationTests.Features.Account;

using static Testing;

public class LoginTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var query = new LoginQuery();

        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireExistingUser()
    {
        var query = new LoginQuery
        {
            Email = "jdoe@test.com",
            Password = "Pa$$word13"
        };

        var user = await SendAsync(query);

        user!.UserName.Should().BeNull();
        user!.Email.Should().BeNull();
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

        await SendAsync(command);


        var query = new LoginQuery
        {
            Email = "test@test.com",
            Password = "Pa$$word13"
        };

        var user = await SendAsync(query);

        user!.UserName.Should().Be("Username");
        user!.Email.Should().Be("test@test.com");
    }

}