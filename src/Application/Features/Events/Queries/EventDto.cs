namespace Carpool.Application.Features.Events.Queries;
public class EventDto : IMapFrom<Event>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Event, EventDto>()
            .ForMember(d => d.url, o => o.MapFrom(s => s.Photo!.Url));
    }

    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public DateTime Date { get; set; }
    public string url { get; set; } = String.Empty;
}