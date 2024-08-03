using Api.Contracts;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/moderation/articles")]
[ApiController]
public class ModerateArticlesController(ApplicationDbContext _context) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var article = await _context.Articles
            .Include(a => a.Author)
            .WithStates(ArticleState.OnModeration)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article is null)
            return NotFound();

        return Ok(new GetArticleForModerationResponse()
        {
            Title = article.Title,
            Tags = article.Tags,
            Content = article.Content,
            Description = article.Description,
            Author = new ArticleAuthorDto()
            {
                Name = article.Author.Name,
            }
        });
    }

    [HttpPost("page")]
    public async Task<IActionResult> GetPageAsync(PageRequest page)
    {
        var articles = await _context.Articles
            .WithStates(ArticleState.OnModeration)
            .OrderBy(date => date.CreatedAt)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(article => new GetPageOfArticlesForModerationResponse
            {
                Title = article.Title,
                Description = article.Description,
                Tags = article.Tags,
                Author = new ArticleAuthorDto()
                {
                    Name = article.Author.Name,
                }
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)page.Size);
        var result = new ListPaginations<GetPageOfArticlesForModerationResponse>(articles, count, totalPages);

        return Ok(result);
    }

    [HttpPost("{id:guid}:approve")]
    public async Task<IActionResult> ApproveAsync(Guid id)
    {
        var article = await _context.Articles
           .Include(a => a.Author)
           .WithStates(ArticleState.OnModeration)
           .FirstOrDefaultAsync(a => a.Id == id);

        if (article is null)
            return NotFound();

        article.State = ArticleState.Published;
        article.PublishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:guid}:reject")]
    public async Task<IActionResult> RejectAsync(Guid id)
    {
        var article = await _context.Articles
           .Include(a => a.Author)
           .WithStates(ArticleState.OnModeration)
           .FirstOrDefaultAsync(a => a.Id == id);

        if (article is null)
            return NotFound();

        article.State = ArticleState.Rejected;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
