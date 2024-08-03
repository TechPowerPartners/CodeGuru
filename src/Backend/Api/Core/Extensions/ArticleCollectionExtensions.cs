using Domain.Entities;
using Domain.Enums;

namespace Api.Core.Extensions;

public static class ArticleCollectionExtensions
{
    public static IQueryable<Article> WithDraftState(this IQueryable<Article> query)
        => query.Where(a => a.State == ArticleState.Draft);

    public static IQueryable<Article> WithStates(this IQueryable<Article> query, params ArticleState[] states)
        => query.Where(a => states.Contains(a.State));
}
