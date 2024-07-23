using Domain.Entities;

namespace Api.Services;

public static class ArticleCacheKeyFactory
{
    /// <summary>
    /// Создать ключ для статей черновиков.
    /// </summary>
    public static string CreateForDraft(Guid articleId)
        => $"article:draft:{articleId}";
}