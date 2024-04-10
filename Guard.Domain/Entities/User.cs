namespace Guard.Domain.Entities;

public class User : IAggregateRoot
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}
