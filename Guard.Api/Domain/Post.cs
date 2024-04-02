using System.Text.Json.Serialization;

namespace Guard.Api.Domain;

public class Post
{
    public Guid Id { get; set; }
    public string PostContent { get; set; }
}
