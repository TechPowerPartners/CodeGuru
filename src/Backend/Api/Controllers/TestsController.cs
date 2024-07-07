using Api.Contracts.Tests.Dto;
using Api.Contracts.Tests.Requests;
using Api.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        var questions = request.Questions
            .Select(question => new Question()
            {
                Text = question.Text,
                DifficultyLevel = question.DifficultyLevel,
                NumberOfPoints = question.NumberOfPoints,
                Answers = question.Answers
                    .Select(answer => new Answer()
                    {
                        Text = answer.Text,
                        IsCorreсt = answer.IsCorreсt
                    }).ToList(),
            }).ToList();

        var test = new Test()
        {
            Name = request.Name,
            Description = request.Description,
            TravelTime = request.TravelTime,
            Questions = questions,
        };
        await _context.AddAsync(test);
        await _context.SaveChangesAsync();

        return Ok(test.Id);
    }

    [HttpPost("get")]
    public IActionResult Get(Guid id)
    {
        var test = _context.Tests
            .Where(t => t.Id == id)
            .Include(test => test.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault();

        if (test == null) return BadRequest("Test not found");

        var testDto = new GetTestDto()
        {
            Id = test.Id,
            Name = test.Name,
            Description = test.Description,
            TravelTime = test.TravelTime,
            Questions = test.Questions
                .Select(q => new GetQuestionDto()
                {
                    Id = q.Id,
                    Text = q.Text,
                    NumberOfPoints = q.NumberOfPoints,
                    DifficultyLevel = q.DifficultyLevel,
                    Answers = q.Answers.Select(a => new GetAnswerDto()
                    {
                        Id = a.Id, Text = a.Text,
                    }).ToList(),
                }).ToList(),
        };
        return Ok(testDto);
    }

    [HttpPost("getTestIds")]
    public IActionResult GetTestIds()
        => Ok(_context.Tests.Select(t => t.Id));

    [Authorize]
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var test =  await _context.Tests.Where(t => t.Id == id).FirstOrDefaultAsync();

        if (test == null) return BadRequest("Test not found for to delete");

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();

        return Ok();
    }
    /// <summary>
    /// Тоже самое
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetLanguagesList")]
    public async Task<IActionResult> GetLanguages()
    {
        return Ok(await _context.Tests.Select(t => t.Name).ToListAsync());
    }

    /// <summary>
    /// Пример для будущего рефакторинга можно удалять к х*ям собачьим
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    [HttpPost("getrand")]
    public async Task<IActionResult> GetRandomQuestion(string Name)
    {
        Random rand = new();
        int toSkip = rand.Next(0, _context.Tests.Count());
        return Ok(_context.Tests.Skip(toSkip).Take(1).First());
    }

}
