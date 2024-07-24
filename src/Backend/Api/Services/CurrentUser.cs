using Api.Abstractions;
using Api.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Services;

/// <inheritdoc cref="ICurrentUser"/>
public class CurrentUser(IHttpContextAccessor _httpContextAccessor, ApplicationDbContext _dbContext) : ICurrentUser
{
    public bool IsAuthenticated => _httpContextAccessor.HttpContext!.User.Claims.Any();
    public string? Name => _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

    public Guid? Id => _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value is null ?
        null : 
        Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

    public Task<User?> GetAsync()
    {
        var name = _httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.Name)?.Value;
        return _dbContext.Users.FirstOrDefaultAsync(c => c.Name == name);
    }
}
