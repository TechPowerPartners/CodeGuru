namespace Api.Contracts.Articles;

public class GetPageOfArticlesResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleAuthorDto Author { get; set; }
    public DateTime PublishedAt { get; set; }
}
