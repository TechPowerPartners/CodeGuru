namespace Api.Contracts.Articles;
public class GetArticleResponse
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishedAt { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleAuthorDto Author { get; set; }
}
