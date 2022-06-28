using Carpool.Application.Features.Events.Commands.CreateEvent;

namespace Carpool.Application.IntegrationTests.Features.Events;

using static Testing;

public class CreateEventTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateEventCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateEvent()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateEventCommand
        {
            Name = "New event",
            Date = new DateTime()
        };

        var eventId = 1;
        //        var eventId = await SendAsync(command);


        var ev = await FindAsync<Event>(eventId);

    }
}
