
using Carpool.Application.Features.Events.Commands.CreateEvent;
using Carpool.Application.Features.Events.Queries;
using Carpool.Application.Features.Events.Queries.GetEvents;

namespace Carpool.WebUI.Controllers;

[Authorize]
public class EventController : ApiControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpGet]
    public async Task<IEnumerable<EventDto>> Get()
    {
        return await Mediator.Send(new GetEventsQuery());
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost]
    public async Task<int> Post([FromForm] CreateEventCommand command)
    {
        return await Mediator.Send(command);
    }
}


