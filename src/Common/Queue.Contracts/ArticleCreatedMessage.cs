namespace Queue.Contracts;
public record ArticleCreatedMessage
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorName { get; set; }
}
