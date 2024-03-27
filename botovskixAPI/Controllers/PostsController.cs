using botovskixAPI.Models;
using botovskixAPI.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace botovskixAPI.Controllers
{
    public class PostsController : ControllerBase
    {

        public ApplicationDbContext Context;

        public PostsController(ApplicationDbContext context)
        {
            Context = context;
        }

        [HttpGet("getAll")]
        public List<Posts> GetAllPost()
        {
            var getAll = Context.Posts.ToList();
            return getAll;
        }
        [Authorize]
        [HttpPost("post")]
        public async Task<IActionResult> CreatePost(PostDTO posts)
        {

            Posts post = new()
            {
                PostContent = posts.PostContent,

            };

            if (await Context.Posts.AnyAsync(p => p.PostContent == posts.PostContent))
            {
                return BadRequest("Алё, будь оригинальней, существует такое уже");
            }

            await Context.Posts.AddAsync(post);
            await Context.SaveChangesAsync();

            return Ok(post);
        }

        [HttpGet("getrandom")]
        public async Task<IActionResult> GetRandomPost()
        {
            var count = await Context.Posts.CountAsync();

            if (count == 0)
            {
                return NotFound("Нету смехуечик извини брат");
            }

            var randomIndex = new Random().Next(0, count);

            var randomPost = await Context.Posts.Skip(randomIndex).FirstOrDefaultAsync();

            return Ok(randomPost.PostContent);
        }


    }
}
