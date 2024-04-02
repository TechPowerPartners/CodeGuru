using System.Text.Json.Serialization;

namespace Guard.Api.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}
