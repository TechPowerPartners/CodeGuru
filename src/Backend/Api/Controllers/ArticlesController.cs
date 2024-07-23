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
        return Ok(await _context.Articles.WithState(ArticleState.Published).FirstOrDefaultAsync(a => a.Id == id));
    }

    [HttpPost("/page")]
    public async Task<IActionResult> GetPageAsync(PageRequest page)
    {
        var articles = await _context.Articles
            .WithState(ArticleState.Published)
            .OrderBy(date => date.CreatedAt)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(article => new GetArticlesResponse
            {
                Title = article.Title,
                Content = article.Content,
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)page.Size);
        var result = new ListPaginations<GetArticlesResponse>(articles, count, totalPages);

        return Ok(result);
    }
}
