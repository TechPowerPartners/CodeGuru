namespace Api.Contracts.Articles;
public class GetArticleForModerationResponse
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleAuthorDto Author { get; set; }
}
