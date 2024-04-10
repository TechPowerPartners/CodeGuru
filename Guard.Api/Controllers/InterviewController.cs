using EasyNetQ;
using Guard.Api.Contracts.Interviews;
using Guard.Api.Persistence;
using Guard.Domain.Entities;
using Guard.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Queue.Contracts;

namespace Guard.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class InterviewController(ApplicationDbContext dbContext, IBus _bus) : ControllerBase
{
    [HttpPost("CreateTest")]
    public async Task CreateInterviewTest(CreateInterviewRequest request)
    {
        await _bus.PubSub.PublishAsync(new InterviewCreatedMessage(
            request.IntervieweeName,
            request.FromRole,
            request.ToRole,
            request.Date,
            request.FromTime,
            request.ToTime));
    }

    [HttpPost]
    public async Task<IActionResult> CreateInterview(CreateInterviewRequest request)
    {
        var isDateFree = !await dbContext.Interviews.AnyAsync(i => i.Date.Date == request.Date);
        if (!isDateFree) 
            return BadRequest("Дата занята");

        var interviewee = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == request.IntervieweeName);
        if (interviewee is null) 
            return BadRequest("Собеседуемый не найден");

        var interviewDate = InterviewDate.Create(request.Date, request.FromTime, request.ToTime);
        var roleEnhancement = RoleEnhancement.Create(request.FromRole, request.ToRole);
        var interview = new Interview(Guid.NewGuid(), interviewDate, roleEnhancement, interviewee.Id);

        await dbContext.Interviews.AddAsync(interview);
        await dbContext.SaveChangesAsync();

        await _bus.PubSub.PublishAsync(new InterviewCreatedMessage(
           request.IntervieweeName,
           request.FromRole,
           request.ToRole,
           request.Date,
           request.FromTime,
           request.ToTime));

        return Ok();
    }

    [HttpPost(":get")]
    public async Task<IActionResult> GetAll(GetInterviewsRequest request)
    {
        var querry = (IQueryable<Interview>)dbContext.Interviews;

        if (request.StartDate != null)
            querry = querry.Where(i => i.Date.Date == request.StartDate.Value);
       
        if (request.IntervieweeName != null) 
            querry = querry.Where(i => i.Interviewee.Name == request.IntervieweeName);

        querry = querry.OrderBy(i => i.Date);
        return Ok(querry.ToList());
    }

    [HttpPost("{id:guid}:complete")]
    public async Task<IActionResult> Complete(Guid id, CompeteInterviewRequest request)
    {
        var interview = await dbContext.Interviews.FirstOrDefaultAsync(i => i.Id == id);
        
        if(interview is null)
            return BadRequest("Такое собеседование не существует");

        interview.Complete(request.Review, request.Status, DateTime.UtcNow);

        dbContext.SaveChanges();
        return Ok(interview);
    }
}
