namespace Carpool.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommand : IRequest<int>
{
    public String Name { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public IFormFile? File { get; set; }

}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, int>
{

    private readonly IApplicationDbContext _context;
    private readonly IPhotoAccessor _photoAccessor;
    public CreateEventCommandHandler(IApplicationDbContext context, IPhotoAccessor photoAccessor)
    {
        _photoAccessor = photoAccessor;
        _context = context;

    }


    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var photoUploadResult = await _photoAccessor.AddPhoto(request.File!);

        var entity = new Event
        {
            Name = request.Name,
            Date = request.Date,
            Photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicID
            }
        };

        _context.Events.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}