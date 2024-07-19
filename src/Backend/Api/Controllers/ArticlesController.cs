using Api.Contracts;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ApplicationDbContext _context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Articles.ToListAsync());
    }

    [HttpPost("/page")]
    public async Task<IActionResult> GetPageAsync(PageRequest page)
    {
        var articles = await _context.Articles
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateArticleRequest request)
    {
        if (request.CheckIfNull() is false)
            return BadRequest("Одна или несколько полей пустое");

        var authorName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var existingAuthor = await _context.Users.FirstOrDefaultAsync(c => c.Name == authorName);

        if (existingAuthor != null)
        {
            _context.Articles.Add(new Article
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = existingAuthor.Id
            });
            _context.SaveChanges();
            return Ok("Статья создана");
        }

        return BadRequest("Ошибка");
    }
}
