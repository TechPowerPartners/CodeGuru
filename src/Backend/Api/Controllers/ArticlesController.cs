using Api.Abstractions;
using Api.Contracts;
using Api.Contracts.Articles;
using Api.Core.Extensions;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ApplicationDbContext _context, ICurrentUser _currentUser) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var article = await _context.Articles
            .Include(a => a.Author)
            .WithStates(ArticleState.Published)
            .FirstOrDefaultAsync(a => a.Id == id);
        var likes = _context.UserLikes.Where(u => u.ArticleId == id);
        return Ok(new GetArticleResponse()
        {
            Title = article.Title,
            Tags = article.Tags,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
            Likes = likes.Count(),
            Author = new ArticleAuthorDto()
            {
                Name = article.Author.Name,
            }
        });
    }
    [HttpPost("like")]
    public async Task<IActionResult> LikeArticle(ArticleLikeDto articleLikeDto)
    {

        var likes = _context.UserLikes.Any(u => u.UserId == _currentUser.Id!.Value && u.ArticleId == articleLikeDto.ArticleId);
        if (likes == false)
        {
            _context.UserLikes.Add(new UserLikes
            {
                UserId = _currentUser.Id.Value,
                ArticleId = articleLikeDto.ArticleId,

            });
            await _context.SaveChangesAsync();
            return Ok();
        }
        _context.UserLikes.Where(u => u.UserId == _currentUser.Id!.Value && u.ArticleId == articleLikeDto.ArticleId).ExecuteDelete();
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("addcomment")]
    public async Task<IActionResult> AddComment(ArticleComment articleComment)
    {
        var findFirst = User.FindAll(ClaimTypes.NameIdentifier);
        var nameId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var naeId = User.FindFirst("nameid")?.Value;
        _context.UserComments.Add(
            new UserComment
            {
                Article = articleComment.articleId,
                UserId = _currentUser.Id!.Value,
                Comment = articleComment.Comment,
            }
            );

        await _context.SaveChangesAsync();
        return Ok("Коммент добавлен");
    }
    [HttpPost("getcomments")]
    public async Task<IActionResult> GetArticleComments(GetCommentsRequest commentsRequest)
    {
        var query = from articles in _context.Set<UserComment>()
                    join user in _context.Set<User>()
                    on articles.UserId equals user.Id
                    where articles.Article == commentsRequest.Article
                    select new
                    {
                        articles.UserId,
                        articles.Comment,
                        user.Name,
                    };
        return Ok(query);
    }
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveComment()
    {
        return Ok();
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
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Tags = article.Tags,
                PublishedAt = article.PublishedAt,
                Author = new ArticleAuthorDto()
                {
                    Name = article.Author.Name,
                },
                Likes = _context.UserLikes.Count(l => l.ArticleId == article.Id),
            })
            .ToListAsync();

        var count = await _context.Articles.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)page.Size);
        var result = new ListPaginations<GetPageOfArticlesResponse>(articles, count, totalPages);

        return Ok(result);
    }
}
