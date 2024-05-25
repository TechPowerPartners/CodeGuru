using Api.Contracts;
using System.Linq.Expressions;

namespace Api.Core.Extensions;

public static class FilterExpresions
{
    public static IQueryable<TEntity> ApplyDateFilter<TEntity>(this IQueryable<TEntity> query, DateFilterDto? filter, Expression<Func<TEntity, DateTime>> selector)
    {
        if (filter is null)
            return query;
        
        if (filter.Target.HasValue)
        {
            var equalExpression = Expression.Equal(selector, Expression.Constant(filter.Target.Value.ToDateTime(TimeOnly.MinValue)));
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(equalExpression, selector.Parameters);
            query = query.Where(lambdaExpression);

            return query;
        }

        if (filter.From.HasValue)
        {
            var equalExpression = Expression.GreaterThanOrEqual(selector, Expression.Constant(filter.From.Value));
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(equalExpression, selector.Parameters);
            query = query.Where(lambdaExpression);
        }

        if (filter.To.HasValue)
        {
            var equalExpression = Expression.LessThanOrEqual(selector, Expression.Constant(filter.To.Value));
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(equalExpression, selector.Parameters);
            query = query.Where(lambdaExpression);
        }

        return query;
    }
}
