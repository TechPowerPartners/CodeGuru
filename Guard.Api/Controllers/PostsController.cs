using Guard.Api.Contracts.Posts;
using Guard.Api.Persistence;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guard.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PostsController(ApplicationDbContext _context) : ControllerBase
{
	[HttpGet]
	public Task<List<GetPostResponse>> GetAllAsync()
		=> _context.Posts
			.Select(p => new GetPostResponse()
			{
				Id = p.Id,
				PostContent = p.PostContent
			})
			.ToListAsync();

	[HttpGet("random")]
	public async Task<IActionResult> GetRandomPost()
	{
		var count = await _context.Posts.CountAsync();

		if (count == 0)
			return NotFound("Нету смехуечик извини брат");

		var randomIndex = new Random().Next(0, count);
		var randomPost = await _context.Posts.Skip(randomIndex).FirstOrDefaultAsync();

		return Ok(randomPost.PostContent);
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreateAsync(string content)
	{
		var isPostWithContentExists = await _context.Posts.AnyAsync(p => p.PostContent == content);

		if (isPostWithContentExists)
			return BadRequest("Алё, будь оригинальней, существует такое уже");

		var post = new Post()
		{
			PostContent = content,
		};

		await _context.Posts.AddAsync(post);
		await _context.SaveChangesAsync();

		return Ok(post);
	}
}
