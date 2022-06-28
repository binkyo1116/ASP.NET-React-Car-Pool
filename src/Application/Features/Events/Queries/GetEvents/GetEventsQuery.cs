namespace Carpool.Application.Features.Events.Queries.GetEvents;

public class GetEventsQuery : IRequest<IEnumerable<EventDto>>
{
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, IEnumerable<EventDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetEventsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Events
            .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}