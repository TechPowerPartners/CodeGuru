using Api.Abstractions;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/users/me/articles")]
[ApiController]
[Authorize]
public class UserArticlesController(
    ApplicationDbContext _context,
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

        return Ok(new GetUserArticleResponse()
        {
            Title = article.Title,
            Tags = article.Tags,
            Content = article.Content,
            State = article.State
        });
    }
    [AllowAnonymous]
    [HttpPost("page")]
    public async Task<IActionResult> GetPageAsync(GetPageOfUserArticlesRequest request)
    {
        var query = _context.Articles
            .Where(a => a.AuthorId == _currentUser.Id!.Value);

        if (request.States is [..])
            query = query.Where(a => request.States.Contains(a.State));

        if (request.Tags is [..])
            query = query.Where(a => a.Tags.Any(t => request.Tags.Contains(t)));

        var articles = await query.OrderBy(a => a.CreatedAt)
            .Skip((request.Page.Number - 1) * request.Page.Size)
            .Take(request.Page.Size)
            .Select(article => new GetPageOfUserArticlesResponse
            {
                Title = article.Title,
                Description = article.Description,
                Tags = article.Tags,
                State = article.State
               
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)request.Page.Size);
        var result = new ListPaginations<GetPageOfUserArticlesResponse>(articles, count, totalPages);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(SaveUserArticleRequest request)
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
            State = ArticleState.Published,//TODO поменять обратно на драфт
            Description = request.Description,
            CreatedAt = new DateTime(),
            Tags = request.Tags
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return Created(null as string, article.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, SaveUserArticleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Название статьи не заполнено");

        var author = await _currentUser.GetAsync();

        if (author is null)
            return Unauthorized();

        var existingArticle = await _context.Articles
            .FirstOrDefaultAsync(a => a.Id == id && a.AuthorId == _currentUser.Id!.Value);

        if (existingArticle is null)
            return NotFound();

        if (existingArticle.State == ArticleState.Published)
            return BadRequest("Нельзя редактировать опубликованную статью");

        if (existingArticle.State == ArticleState.OnModeration)
            return BadRequest("Нельзя редактировать статью отправленную на модерацию");

        existingArticle.Title = request.Title;
        existingArticle.Content = request.Content;
        existingArticle.Tags = request.Tags.ToList();
        existingArticle.Description = request.Description;

        if (existingArticle.State == ArticleState.Rejected)
            existingArticle.State = ArticleState.Draft;

        await _context.SaveChangesAsync();
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

        if (existingArticle.State != ArticleState.Draft)
            return BadRequest("Нельзя отправить на модерацию статью из текущего состояния");

        existingArticle.State = ArticleState.OnModeration;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
