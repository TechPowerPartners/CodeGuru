using Api.Contracts;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ApplicationDbContext _context) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var article = await _context.Articles
            .Include(a => a.Author)
            .WithStates(ArticleState.Published)
            .FirstOrDefaultAsync(a => a.Id == id);

        return Ok(new GetArticleResponse()
        {
            Title = article.Title,
            Tags = article.Tags,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
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
            .WithStates(ArticleState.Published)
            .OrderBy(date => date.CreatedAt)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(article => new GetPageOfArticlesResponse
            {
                Title = article.Title,
                Description = article.Description,
                Tags = article.Tags,
                PublishedAt = article.PublishedAt,
                Author = new ArticleAuthorDto()
                {
                    Name = article.Author.Name,
                }
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)page.Size);
        var result = new ListPaginations<GetPageOfArticlesResponse>(articles, count, totalPages);

        return Ok(result);
    }
}
