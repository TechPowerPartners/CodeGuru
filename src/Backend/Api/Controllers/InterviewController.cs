using Api.Contracts.Interviews;
using Api.Persistence;
using Domain.Entities;
using Domain.ValueObjects;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Queue.Contracts;

namespace Api.Controllers;

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
			request.FromDate,
			request.ToDate));
	}

	[HttpPost]
	public async Task<IActionResult> CreateInterview(CreateInterviewRequest request)
	{
		var isDateFree = !await dbContext.Interviews.AnyAsync(i => i.Date.From == request.FromDate);
		if (!isDateFree)
			return BadRequest("Дата занята");

		var interviewee = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == request.IntervieweeName);
		if (interviewee is null)
			return BadRequest("Собеседуемый не найден");

		var interviewDate = InterviewDate.Create(request.FromDate, request.ToDate);
		var roleEnhancement = RoleEnhancement.Create(request.FromRole, request.ToRole);
		var interview = new Interview(Guid.NewGuid(), interviewDate, roleEnhancement, interviewee.Id);

		await dbContext.Interviews.AddAsync(interview);
		await dbContext.SaveChangesAsync();

		await _bus.PubSub.PublishAsync(new InterviewCreatedMessage(
		   request.IntervieweeName,
		   request.FromRole,
		   request.ToRole,
		   request.FromDate,
		   request.ToDate));

		return Ok();
	}

	[HttpPost(":get")]
	public async Task<IActionResult> GetAll(GetInterviewsRequest request)
	{
		var querry = (IQueryable<Interview>)dbContext.Interviews;

		if (request.Date is not null)
			querry = querry.Where(i => DateOnly.FromDateTime(i.Date.From) == request.Date.Value);

		if (request.IntervieweeName is not null)
			querry = querry.Where(i => i.Interviewee.Name == request.IntervieweeName);

		querry = querry.OrderBy(i => i.Date);
		return Ok(querry.ToList());
	}

	[HttpPost("{id:guid}:complete")]
	public async Task<IActionResult> Complete(Guid id, CompeteInterviewRequest request)
	{
		var interview = await dbContext.Interviews.FirstOrDefaultAsync(i => i.Id == id);

		if (interview is null)
			return BadRequest("Такое собеседование не существует");

		interview.Complete(request.Review, request.Status, DateTime.UtcNow);

		dbContext.SaveChanges();
		return Ok(interview);
	}

	[HttpPost("{id:guid}:cancel")]
	public async Task<IActionResult> Cancel(Guid id, string message)
	{
		var interview = await dbContext.Interviews.FirstOrDefaultAsync(x => x.Id == id);
		if (interview is null)
			return BadRequest("Такое собеседование не существует");
		interview.Cancel(message);
		dbContext.SaveChanges();
		return Ok(interview);
	}
}
