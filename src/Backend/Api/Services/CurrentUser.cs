using Api.Abstractions;
using Api.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Services;

/// <inheritdoc cref="ICurrentUser"/>
public class CurrentUser(HttpContext _httpContext, ApplicationDbContext _dbContext) : ICurrentUser
{
    public bool IsAuthenticated => _httpContext.User.Claims.Any();
    public string? Name => _httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

    public Guid? Id => _httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value is null ?
        null : 
        Guid.Parse(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

    public Task<User?> GetAsync()
    {
        var name = _httpContext.User.Claims.First(c => c.Type == ClaimTypes.Name)?.Value;
        return _dbContext.Users.FirstOrDefaultAsync(c => c.Name == name);
    }
}
