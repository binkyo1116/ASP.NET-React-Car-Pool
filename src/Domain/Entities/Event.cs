namespace Carpool.Domain.Entities;

public class Event : AuditableEntity
{
    public int Id { get; set; }

    public String Name { get; set; } = String.Empty;

    public DateTime Date { get; set; }

    public Photo? Photo { get; set; }
}
