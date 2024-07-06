using Api.Contracts.Tests;
using Api.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Api.Controllers;

[Route("[Controller]")]
[ApiController]
public class TestsController(ApplicationDbContext _context) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(CreateTestRequest request)
    {
        var test = new Test()
        {
            Name = request.Name,
            Description = request.Description,
            TravelTime = request.TravelTime,
            Questions = request.Questions,
        };
        await _context.AddAsync(test);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetAsync(GetTestRequest request)
        => Ok(await _context.Tests.Where(t => t.Name == request.Name).ToListAsync());

    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _context.Tests.ToListAsync());
}