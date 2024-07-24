using Domain.Entities;
using Domain.Enums;

namespace Api.Core.Extensions;

public static class ArticleCollectionExtensions
{
    public static IQueryable<Article> WithDraftState(this IQueryable<Article> query)
        => query.Where(a => a.State == ArticleState.Draft);

    public static IQueryable<Article> WithState(this IQueryable<Article> query, ArticleState state)
        => query.Where(a => a.State == state);
}
