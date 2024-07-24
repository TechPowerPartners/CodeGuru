using Api.Abstractions;
using Api.Contracts;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.Controllers;

[Route("api/users/me/articles")]
[ApiController]
[Authorize]
public class UserArticlesController(
    ApplicationDbContext _context,
    //IDistributedCache _distributedCache,
    ICurrentUser _currentUser) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var article = await _context.Articles
            .WithDraftState()
            .FirstOrDefaultAsync(a => a.Id == id && a.AuthorId == _currentUser.Id!.Value);

        if (article is null)
            return NotFound();

        return Ok(article);
    }

    [HttpPost("/page")]
    public async Task<IActionResult> GetPageAsync(PageRequest page)
    {
        var articles = await _context.Articles
            .WithDraftState()
            .Where(a => a.AuthorId == _currentUser.Id!.Value)
            .OrderBy(a => a.CreatedAt)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(article => new GetArticlesResponse
            {
                Title = article.Title,
                Content = article.Content,
                Tags = article.Tags
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync(a => a.State == ArticleState.Draft);
        var totalPages = (int)Math.Ceiling(count / (double)page.Size);
        var result = new ListPaginations<GetArticlesResponse>(articles, count, totalPages);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(SaveDraftArticleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Название статьи не заполнено");

        var author = await _currentUser.GetAsync();

        if (author is null)
            return BadRequest("Ошибка");

        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = author!.Id,
            State = ArticleState.Draft
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return Created(null as string, article.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, SaveDraftArticleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Название статьи не заполнено");

        var author = await _currentUser.GetAsync();

        if (author is null)
            return Unauthorized();

        var existingArticle = await _context.Articles
            .WithDraftState()
            .FirstOrDefaultAsync(a => a.Id == id && a.AuthorId == _currentUser.Id!.Value);

        if (existingArticle is null)
            return NotFound();

        //#region Если статья закеширована, обновляем в кэше.
        //var cachedArticle = await _distributedCache.GetAsync<Article>(ArticleCacheKeyFactory.CreateForDraft(existingArticle.Id));

        //if (cachedArticle is not null)
        //{
        //    cachedArticle.Title = request.Title;
        //    cachedArticle.Content = request.Content;
        //    cachedArticle.Tags = request.Tags;

        //    await _distributedCache.SetAsync(ArticleCacheKeyFactory.CreateForDraft(existingArticle.Id), cachedArticle);
        //    return NoContent();
        //}
        //#endregion

        existingArticle.Title = request.Title;
        existingArticle.Content = request.Content;
        existingArticle.Tags = request.Tags;

        await _context.SaveChangesAsync();
        //await _distributedCache.SetAsync(ArticleCacheKeyFactory.CreateForDraft(existingArticle.Id), existingArticle);

        return NoContent();
    }

    [HttpPost("{id:guid}:submit-for-moderation")]
    public async Task<IActionResult> SubmitForModerationAsync(Guid id)
    {
        var author = await _currentUser.GetAsync();

        if (author is null)
            return Unauthorized();

        var existingArticle = await _context.Articles
            .WithDraftState()
            .FirstOrDefaultAsync(a => a.Id == id && a.AuthorId == _currentUser.Id!.Value);

        if (existingArticle is null)
            return NotFound();

        if (existingArticle.State is not ArticleState.Rejected or ArticleState.Draft)
            return BadRequest("Нельзя отправить на модерацию статью из текущего состояния");

        existingArticle.State = ArticleState.OnModeration;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
