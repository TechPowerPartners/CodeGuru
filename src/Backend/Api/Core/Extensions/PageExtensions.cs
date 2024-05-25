namespace Api.Core.Extensions;

public static class PageExtensions
{
    public static IQueryable<T> WithPage<T>(this IQueryable<T> query, int number, int size)
        => query.Skip(size * (number - 1)).Take(size);
}