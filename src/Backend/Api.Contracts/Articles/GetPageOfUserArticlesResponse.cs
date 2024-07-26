using Domain.Enums;

namespace Api.Contracts.Articles;

public class GetPageOfUserArticlesResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<string> Tags { get; set; }
    public ArticleState State { get; set; }
}
