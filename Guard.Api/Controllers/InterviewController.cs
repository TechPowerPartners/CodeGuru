using Guard.Api.Domain;
using Guard.Api.DTOs.Interview;
using Guard.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guard.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InterviewController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateInterview(InterviewDto interviewDto)
    {
        var isDateFree = !await dbContext.Interviews.AnyAsync(i => i.StartDate == interviewDto.StartDate);
        if (isDateFree == false) 
            return BadRequest("Дата занята");

        var interviewee = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == interviewDto.Name);
        if (interviewee == null) 
            return BadRequest("Пользователя нет");

        var interview = new Interview()
        {
            Id = Guid.NewGuid(),
            StartDate = interviewDto.StartDate,
            FromTime = interviewDto.FromTime,
            ToTime = interviewDto.ToTime,
            FromRole = interviewDto.FromRole,
            ToRole = interviewDto.ToRole,
            IntervieweeId = interviewee.Id,
            Review = string.Empty
        };

        dbContext.Interviews.Add(interview);
        dbContext.SaveChanges();
        return Ok();
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(InterviewFilterDto interviewFilter)
    {
        var querry = (IQueryable<Interview>)dbContext.Interviews;

        if (interviewFilter.StartDate != null)
            querry = querry.Where(i => i.StartDate == interviewFilter.StartDate.Value);
       
        if (interviewFilter.Name != null) 
            querry = querry.Where(i => i.Interviewee.Name == interviewFilter.Name);

        querry = querry.OrderBy(i => i.StartDate);
        return Ok(querry.ToList());
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> Complete(Guid interviewId, InterviewCompletedDto completedDto )
    {
        var completedInterview = await dbContext.Interviews.FirstOrDefaultAsync(i => i.Id == interviewId);
        
        if(completedInterview == null)
            return BadRequest("Такое собеседование не существует");
        
        if(!(completedInterview.StartDate < DateOnly.FromDateTime(DateTime.UtcNow)))
            return BadRequest("Дата собеса не прошла еще!");

        completedInterview.Review = completedDto.Review;
        completedInterview.IsPassed = completedDto.IsPassed;

        dbContext.SaveChanges();
        return Ok(completedInterview);
    }
}
