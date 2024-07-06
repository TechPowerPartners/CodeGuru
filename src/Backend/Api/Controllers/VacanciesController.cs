using Api.Contracts.Vacancies;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class VacanciesController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost(":get")]
    public async Task<IActionResult> GetAsync(GetVacanciesRequest request)
    {
        var query = dbContext.Vacancies.AsQueryable();

        if (request.LeaderId.HasValue)
            query = query.Where(v => v.LeaderId == request.LeaderId.Value);

        if (request.Keywords is not null && request.Keywords.Count != 0)
            query = query.Where(v => v.Keywords.Any(k => request.Keywords.Contains(k.Value)));

        query = query.ApplyDateFilter(request.PublicationDate, v => v.PublicationDate);
        var vacancies = await query
            .WithPage(request.Page.Number, request.Page.Size)
            .ToListAsync();

        return Ok();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateVacancyRequest request)
    {
        var existingKeywords = await dbContext.VacancyKeywords
            .Where(k => request.Keywords.Contains(k.Value))
            .ToListAsync();

        var currentUser = User.Claims.FirstOrDefault(c => c.Type == "name").Value.ToString();
        
        var findLeadId = dbContext.Users.FirstOrDefault(k => k.Name == currentUser).Id;
        if (existingKeywords.Count != request.Keywords.Count)
            return BadRequest("Указаные не существующие ключевые слова");

        var vacancy = new Vacancy
        {
            Title = request.Title,
            Description = request.Description,
            LeaderId = findLeadId,
            PublicationDate = DateTime.Now,
            Keywords = existingKeywords,
            ClosingDate = request.ClosingDate
        };
        await dbContext.AddAsync(vacancy);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("addkeyword")]
    public async Task<IActionResult> CreateKeyword(string keyword)
    {
        keyword = keyword.ToLower();
        var exists = dbContext.VacancyKeywords.FirstOrDefault(k => k.Value == keyword);

        if (exists == null)
        {
            dbContext.VacancyKeywords.Add(new VacancyKeyword(keyword));
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        return BadRequest("Ключевое слово уже существует");
    }
}
