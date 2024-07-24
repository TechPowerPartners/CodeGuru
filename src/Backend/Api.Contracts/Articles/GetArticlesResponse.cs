namespace Api.Contracts.Articles;

public class GetArticlesResponse
{
    public string Title { get; set; }
    public string Content { get; set; }
    public ICollection<string> Tags { get; set; }
}
