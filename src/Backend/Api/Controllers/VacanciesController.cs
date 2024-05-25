using Api.Contracts.Vacancies;
using Api.Core.Extensions;
using Api.Persistence;
using Domain.Entities;
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateVacancyRequest request)
    {
        var existingKeywords = await dbContext.VacancyKeywords
            .Where(k => request.Keywords.Contains(k.Value))
            .ToListAsync();

        if (existingKeywords.Count != request.Keywords.Count)
            return BadRequest("Указаные не существующие ключевые слова");

        var vacancy = new Vacancy(Guid.NewGuid(), request.Title, request.Description, request.LeaderId, existingKeywords);
        await dbContext.AddAsync(vacancy);
        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
