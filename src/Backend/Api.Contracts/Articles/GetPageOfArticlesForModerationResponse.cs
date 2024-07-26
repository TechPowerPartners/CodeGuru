namespace Api.Contracts.Articles;
public class GetPageOfArticlesForModerationResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleAuthorDto Author { get; set; }
}
