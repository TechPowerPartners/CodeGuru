using Api.Core.Extensions;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.BackroundWorkers;

/// <summary>
/// Фоновый сервис для сохранения статей черновиков из кэша.
/// </summary>
//public class SaveCachedDraftArticlesBackgroundService(IServiceScopeFactory _serviceScopeFactory) : BackgroundService
//{
//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        using IServiceScope scope = _serviceScopeFactory.CreateScope();

//        var dbContext = scope
//            .ServiceProvider
//            .GetRequiredService<ApplicationDbContext>();

//        var distributedCache = scope
//            .ServiceProvider
//            .GetRequiredService<IDistributedCache>();

//        while (true)
//        {
//            var articles = await dbContext.Articles
//                .Where(a => a.State == ArticleState.Draft)
//                .ToListAsync(stoppingToken);

//            foreach (var article in articles)
//            {
//                var cachedArticle = await distributedCache.GetAsync<Article>(ArticleCacheKeyFactory.CreateForDraft(article.Id));

//                if (cachedArticle is null)
//                    continue;

//                article.UpdateFromCache(cachedArticle);
//            }

//            await dbContext.SaveChangesAsync(stoppingToken);

//            var delay = (int)new TimeSpan(hours: 1, minutes: 0, seconds: 0).TotalMilliseconds;
//            await Task.Delay(delay, stoppingToken);
//        }
//    }
//}
