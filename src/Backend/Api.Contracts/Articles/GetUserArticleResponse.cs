using Domain.Enums;

namespace Api.Contracts.Articles;
public class GetUserArticleResponse
{
    public string Title { get; set; }
    public string Content { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleState State { get; set; }
}
