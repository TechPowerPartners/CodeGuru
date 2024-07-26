using Domain.Enums;

namespace Api.Contracts.Articles;

public class GetPageOfUserArticlesRequest
{
    public IReadOnlyList<string> Tags { get; set; }
    public IReadOnlyList<ArticleState> States { get; set; }
    public PageRequest Page { get; set; }
}